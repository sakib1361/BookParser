﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Xhtml;
using ParserEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ParserEngine.Engine
{
    public class CoreEngine:IDisposable
    {
        private IBrowsingContext Context;
        public CoreEngine()
        {
            var config = Configuration.Default.WithDefaultLoader();
            Context = BrowsingContext.New(config);
        }
        public async Task<bool> Parse(Book book)
        {
            var chapter = await GetChapter(book, book.Url);
            if (chapter == null) return false;
            book.Chapters.Add(chapter);
            while (!string.IsNullOrWhiteSpace(chapter.NextUrl))
            {
                chapter = await GetChapter(book, chapter.NextUrl);
                if (chapter == null) break;
                else book.Chapters.Add(chapter);
            }
            return true;
        }

        public async Task<Book> GetBookInfo(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;
            var allParts = new List<string>();
            using var document = await Context.OpenAsync(url);
            var data = GetElement(document, "breadcrumb", ParserType.Class);
            var all = data.GetElementsByTagName("span");
            foreach (var node in all)
            {
                if (node.HasAttribute("property") && node.GetAttribute("property") == "name")
                {
                    allParts.Add(node.InnerHtml.Trim());
                }

            }

            var name = string.Empty;
            var element = GetElement(document, "page-header-title", ParserType.Class);
            if (element != null)
                name = element.InnerHtml.Trim();

            var author = allParts.LastOrDefault();
            var urlData = GetUrl(document, "entry-title-link", ParserType.Class);

            return new Book()
            {
                Url = urlData,
                Author = author,
                Bookname = name
            };
        }

        private async Task<Chapter> GetChapter(Book book, string url)
        {
            using (var document = await Context.OpenAsync(url))
            {
                if (document == null)
                {
                    LogEngine.Error("Failed to Parse Information");
                    return null;
                }
                var info = string.Format("{0}.{1}", book.Chapters.Count + 1, url);
                LogEngine.Data(info);
                var title = GetData(document, book.TitleInfo.ParseValue, book.TitleInfo.ParserType);
                var content = GetData(document, book.ContentInfo.ParseValue, book.ContentInfo.ParserType);
                var nextUrl = GetUrl(document, book.NextChapterInfo.ParseValue, book.NextChapterInfo.ParserType);
                foreach (var ar in book.ArbitaryInfo.ParseValue.Split(' '))
                {
                    var arbitaryData = GetData(document, ar, book.ArbitaryInfo.ParserType);
                    if (arbitaryData != null)
                       content =  content.Replace(arbitaryData, "");
                }


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
        }

        private string GetUrl(IDocument document, string parser, ParserType parserType)
        {
            var element = GetElement(document, parser, parserType);
            if (element != null && element.IsLink())
                return element.GetAttribute("href");
            else return null;
        }

        private string GetData(IDocument document, string parser, ParserType parserType)
        {
            var element = GetElement(document, parser, parserType);
            return element?.ToHtml(XhtmlMarkupFormatter.Instance).Trim();
        }

        private IElement GetElement(IDocument document, string parser, ParserType parserType)
        {
            switch (parserType)
            {
                case ParserType.Class:
                    var cData = document.All
                                        .FirstOrDefault(m => m.ClassList.Contains(parser));
                    return cData;
                case ParserType.Id:
                    var iData = document.All.FirstOrDefault(x => x.Id == parser);
                    return iData;
                case ParserType.Rel:
                    var rData = document.All
                                        .Where(x =>x.LocalName == "a" &&
                                                   x.HasAttribute("rel") && x.GetAttribute("rel") == parser)
                                        .FirstOrDefault();
                    return rData;
            }
            return null;
        }

        public void Dispose()
        {
            try
            {
                Context = null;
            }
            catch { }
        }
    }
}
