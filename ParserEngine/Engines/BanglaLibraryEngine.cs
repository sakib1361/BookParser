using AngleSharp;
using AngleSharp.Dom;
using DesktopParser.Engine;
using ParserEngine.Engine;
using ParserEngine.Models;
using System.Text.RegularExpressions;

namespace ParserEngine.Engines
{
    public class BanglaLibraryEngine : CoreEngine
    {
        //new Website("ebanglalibrary.com",
        //                "entry-title","entry-content","ld-button-transparent", "scriptlesssocialsharing mbfp-btn",
        //               ParserType.Class, ParserType.Class, ParserType.Class, ParserType.Class),

        private const string Header = "ld-focus-content";
        private const string Content = "entry-content";
        private const string Next = "ld-button-transparent";
        private const string Arbitary = "scriptlesssocialsharing mbfp-btn simplefavorite-button ftwp-float-right ftwp-fixed-to-post";

        public async Task<bool> Parse(Book book, IUrlParser parser)
        {
            var temp = Path.Combine(Path.GetTempPath(), book.Bookname);
            if (Directory.Exists(temp)) Directory.Delete(temp, true);
            Directory.CreateDirectory(temp);

            book.FilePath = temp;
            var img = new ImageEngine();
            book.EncodedImage = await img.Parse(book.Bookname, book.Author, book.ImagePath);

            var chapter = await GetChapter(book, book.Url, parser);
            if (chapter == null) return false;
            book.Chapters.Add(chapter);
            while (!string.IsNullOrWhiteSpace(chapter.NextUrl))
            {
                chapter = await GetChapter(book, chapter.NextUrl, parser);
                if (chapter == null) break;
                book.Chapters.Add(chapter);
                if(book.Chapters.Any(x => x.CurrentUrl == chapter.NextUrl)) break;
            }
            return true;
        }

        public Book GetBookInfo(IDocument document)
        {
            if (document == null) return null;
            var allParts = new List<string>();
            //using var document = await parser.OpenAsync(url);
            var data = GetElement(document, "breadcrumb", ParserType.Class, true);
            if (data == null) return null;

            var ld = GetElement(document, "learndash", ParserType.Class, true);
            if (ld == null) return null;

            var all = data.GetElementsByTagName("span");
            foreach (var node in all)
            {
                if (node.HasAttribute("property") && node.GetAttribute("property") == "name")
                {
                    allParts.Add(node.InnerHtml.Trim());
                }

            }

            var name = string.Empty;
            var element = GetElement(document, "page-header-title", ParserType.Class, true);
            if (element != null)
                name = element.InnerHtml.Trim();
            
            var author = allParts.LastOrDefault()??string.Empty;
            if(name == author)
            {
                name = name.Split('–').First();
                author = author.Split('–').Last();
            }
            else if (name.Contains(author))
            {
                name = name.Replace(author, string.Empty);
            }
            name = Regex.Replace(name, @"<[^>]+>|&nbsp;", "").Trim();
            author = Regex.Replace(author, @"<[^>]+>|&nbsp;", "").Trim();

            name = string.Concat(name.Split(Path.GetInvalidFileNameChars()));
            var urlData = GetUrl(document, "ld-item-name", ParserType.Class);

            var imgElement = GetImage(document, "entry-image", ParserType.Class);
            //var img = new ImageEngine();
            //var imgData = await img.Parse(name, author, imgElement);
            return new Book()
            {
                Url = urlData,
                Author = author,
                Bookname = name,
                ImagePath = imgElement,
                //EncodedImage = imgData,
            };
        }
        //ftwp-in-post ftwp-float-right
        //

        private async Task<Chapter> GetChapter(Book book, string url, IUrlParser parser)
        {
            using var document = await parser.OpenAsync(url);
            if (document == null)
            {
                LogEngine.Error("Failed to Parse Information");
                return null;
            }
            var info = string.Format("{0}.{1}", book.Chapters.Count + 1, url);
            LogEngine.Data(info);
            var titleElement = GetElement(document, Header, ParserType.Class, true);
            string title = null;
            if (titleElement != null)
            {
                title = titleElement.FirstElementChild.InnerHtml;
            }
            var content = GetData(document, Content, ParserType.Class);
            var nextUrl = GetUrl(document, Next, ParserType.Class, false);
            foreach (var ar in Arbitary.Split(' '))
            {
                var arbitaryData = GetData(document, ar, ParserType.Class);
                if (arbitaryData != null)
                    content =  content.Replace(arbitaryData, "");
            }

            content = content.Replace("<br />", "<br/>");


            var chapterName = string.Format("index_split_{0:D3}.xhtml", book.Chapters.Count + 1);
            var chapter = new Chapter()
            {
                Name = title,
                CurrentUrl = url,
                FileName = Path.Combine(book.FilePath, chapterName),
                NextUrl = nextUrl
            };
            var fileContent = FileConstants.CustomContent
                                           .Replace(FileConstants.TitleReplace, chapter.Name)
                                           .Replace(FileConstants.ContentReplace, content);
            await FileEngine.WriteTextAsync(chapter.FileName, fileContent);
            return chapter;
        }
    }
}
