using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ParserEngine.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParserEngine.Engine
{
    public class CoreEngine
    {
        private readonly IBrowsingContext Context;
        public CoreEngine()
        {
           var config = Configuration.Default
                                     .WithDefaultLoader()
                                     .WithCss()
                                     .WithJs();
            Context = BrowsingContext.New(config);
        }
        public async Task<bool> Parse(Book book)
        {
            await Task.Delay(2000);
            using (var document = await Context.OpenAsync(book.Url))
            {
                var chapter = await GetChapter(book, document);
                if (chapter == null) return false;
                book.Chapters.Add(chapter);

                while (!string.IsNullOrWhiteSpace(chapter.NextUrl))
                {
                    await Task.Delay(1000);
                    var navDocument = Context.Active;
                    chapter = await GetChapter(book, document);
                    if (chapter == null) break;
                    else book.Chapters.Add(chapter);
                    navDocument.Dispose();
                }
                return true;
            }
        }

        private async Task<Chapter> GetChapter(Book book, IDocument document)
        {
            if (document == null)
            {
                LogEngine.Error("Failed to Parse Information");
                return null;
            }
            var info = string.Format("{0}.{1}", book.Chapters.Count + 1, document.Url);
            LogEngine.Data(info);
            var title = GetData(document, book.TitleInfo.ParseValue, book.TitleInfo.ParserType);
            var content = GetData(document, book.ContentInfo.ParseValue, book.ContentInfo.ParserType);
            var nextUrl = GetUrl(document, book.NextChapterInfo.ParseValue, book.NextChapterInfo.ParserType);
            var chapterName = string.Format("index_split_{0:D3}.xhtml", book.Chapters.Count + 1);
            var chapter = new Chapter()
            {
                Name = title,
                FileName = Path.Combine(book.FilePath, chapterName),
                NextUrl = nextUrl
            };
            var fileContent = FileConstants.CustomContent
                                           .Replace(FileConstants.TitleReplace, chapter.Name)
                                           .Replace(FileConstants.ContentReplace, content);
            await FileEngine.WriteTextAsync(chapter.FileName, fileContent);
            return chapter;
        }

        private string GetUrl(IDocument document, string parser, ParserType parserType)
        {
            var element = GetElement(document, parser, parserType);
            if (element != null && element is IHtmlElement html)
            {
                html.DoClick();
                return element.BaseUri;
            }
            else return string.Empty;
        }

        private string GetData(IDocument document, string parser, ParserType parserType)
        {
            var element = GetElement(document, parser, parserType);
            return element?.InnerHtml.Trim();
        }

        private IElement GetElement(IDocument document, string parser, ParserType parserType)
        {
            switch(parserType)
            {
                case ParserType.Class:
                    var cData = document
                         .All
                         .FirstOrDefault(m => m.ClassList.Contains(parser));
                    return cData;
                case ParserType.Id:
                    var iData = document.All.FirstOrDefault(x => x.Id == parser);
                    return iData;
                case ParserType.Rel:
                    var rData = document.All
                                        .Where(x => x.HasAttribute("rel") && x.GetAttribute("rel") == parser)
                                        .FirstOrDefault();
                    return rData;
            }
            return null;
        }
    }
}
