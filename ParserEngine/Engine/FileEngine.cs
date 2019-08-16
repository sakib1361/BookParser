using ParserEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParserEngine.Engine
{
    class FileEngine
    {
        public void CreateBook(Book book, string exportPath)
        {
            var uuid = Guid.NewGuid().ToString();
            var opfPath = Path.Combine(book.FilePath, "content.opf");
            var titlePath = Path.Combine(book.FilePath, "titlepage.xhtml");
            var tocPath = Path.Combine(book.FilePath, "toc.ncx");
            CreateOpf(opfPath, book.Author, book.Bookname, uuid);
            CreateTOC(tocPath, uuid);
            CreateTitle(titlePath);
        }

        private void CreateOpf(string filePath, string customAuthor,string customTitle, string customUUID)
        {

        }

        private void CreateContent(string filePath,string title, string content)
        {

        }

        private void CreateTitle(string filePath)
        {

        }

        private void CreateTOC(string filePath, string uuid)
        {

        }
    }
}
