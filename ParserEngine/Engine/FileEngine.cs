using ParserEngine.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ParserEngine.Engine
{
    class FileEngine
    {
        public async Task CreateBook(Book book, string exportPath)
        {
            var uuid = Guid.NewGuid().ToString();
            var opfPath = Path.Combine(book.FilePath, "content.opf");
            var titlePath = Path.Combine(book.FilePath, "titlepage.xhtml");
            var tocPath = Path.Combine(book.FilePath, "toc.ncx");
            await CreateOpf(opfPath, book.Author, book.Bookname, uuid);
            CreateTOC(tocPath, uuid);
            CreateTitle(titlePath);
        }

        private async Task CreateOpf(string filePath, string customAuthor,string customTitle, string customUUID)
        {
            var opf = FileConstants.OpfContent
                                   .Replace(FileConstants.AuthorReplace, customAuthor)
                                   .Replace(FileConstants.TitleReplace, customTitle)
                                   .Replace(FileConstants.UUIDReplace, customUUID);
            await WriteTextAsync(filePath, opf);         
        }

        private async Task CreateTitle(string filePath)
        {
            var title = FileConstants.TitleContent;
            await WriteTextAsync(filePath,title);
        }

        private void CreateTOC(string filePath, string uuid)
        {

        }

        public static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
