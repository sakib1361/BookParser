using AngleSharp;
using AngleSharp.Dom;
using ParserEngine.Models;
using System;
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
            var config = Configuration.Default.WithDefaultLoader();
            Context = BrowsingContext.New(config);
        }
        public async Task Parse(Book book)
        {
            var chapter = await GetChapter(book, book.Url);
            book.Chapters.Add(chapter);
            while (!string.IsNullOrWhiteSpace(chapter.NextUrl))
            {
                chapter = await GetChapter(book, chapter.NextUrl);
                if (chapter == null) break;
                else book.Chapters.Add(chapter);
            }
        }

        private async Task<Chapter> GetChapter(Book book, string url)
        {
            using (var document = await Context.OpenAsync(url))
            {
                var title = GetData(document, book.TitleInfo.ParseValue, book.TitleInfo.ParserType);
                var content = GetData(document, book.ContentInfo.ParseValue, book.ContentInfo.ParserType);
                var nextUrl = GetData(document, book.ContentInfo.ParseValue, book.ContentInfo.ParserType);
                var chapterName = string.Format("{000:0}.xhtml", book.Chapters.Count + 1);
                var chapter = new Chapter()
                {
                    Name = title,
                    FileName = Path.Combine(book.FilePath, chapterName),
                    NextUrl = nextUrl
                };
                var fileContent = FileConstants.CustomContent
                                               .Replace("{CustomTitle}", book.Author)
                                               .Replace("{CustomContent}", content);
                File.WriteAllText(chapter.FileName, fileContent);
                return chapter;
            }
        }

        private string GetData(IDocument document, string parser, ParserType parserType)
        {
            if (parserType == ParserType.Class)
            {
                var data = document
                          .All
                          .FirstOrDefault(m => m.ClassList.Contains(parser));
                if (data != null) return data.InnerHtml;
            }
            else
            {
                var data = document.All.FirstOrDefault(x => x.Id == parser);
                if (data != null) return data.InnerHtml;
            }
            return null;
        }

    }
}
