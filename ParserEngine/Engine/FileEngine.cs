using ParserEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserEngine.Engine
{
    public class FileEngine
    {
        public async Task CreateBook(Book book, string exportPath)
        {
            var uuid = Guid.NewGuid().ToString();
            var opfPath = Path.Combine(book.FilePath, "content.opf");
            var titlePath = Path.Combine(book.FilePath, "titlepage.xhtml");
            var tocPath = Path.Combine(book.FilePath, "toc.ncx");
            var allFiles = Directory.GetFiles(book.FilePath);
            var xhmlFiles = allFiles.Select(x => Path.GetFileName(x))
                                    .Where(m=>m.StartsWith("index"));

            var mimeFile = Path.Combine(book.FilePath, "mimetype");

            var metaDir = Path.Combine(book.FilePath, "META-INF");
            Directory.CreateDirectory(metaDir);
            var containerFile = Path.Combine(metaDir, "container.xml");


            await CreateOpf(opfPath, book.Author, book.Bookname, uuid, xhmlFiles);
            await CreateTOC(tocPath, uuid);
            await CreateTitle(titlePath);
            await WriteTextAsync(mimeFile, "application/epub+zip");
            await WriteTextAsync(containerFile, FileConstants.Container);
            LogEngine.Data("Create Completed");
            await Export(book.FilePath, exportPath);
            LogEngine.Data("Epub Created");
        }

        private async Task Export(string source, string file)
        {
            await Task.Run(() => ZipFile.CreateFromDirectory(source, file));
        }

        private async Task CreateOpf(string filePath, string customAuthor,string customTitle, string customUUID,IEnumerable<string> files)
        {
            var contentBuilder = new StringBuilder();
            var refBuilder = new StringBuilder();
            int id = 10;
            foreach (var item in files)
            {
                var manifest = FileConstants.OpfManifest
                                            .Replace(FileConstants.ContentReplace, item)
                                            .Replace(FileConstants.IDReplace, id.ToString());
                var opfRef = FileConstants.OpfRef
                                       .Replace(FileConstants.IDReplace, id.ToString());
                contentBuilder.AppendLine(manifest);
                refBuilder.AppendLine(opfRef);
                id++;
            }
            var opf = FileConstants.OpfContent
                                   .Replace(FileConstants.AuthorReplace, customAuthor)
                                   .Replace(FileConstants.TitleReplace, customTitle)
                                   .Replace(FileConstants.UUIDReplace, customUUID)
                                   .Replace(FileConstants.ContentReplace,contentBuilder.ToString())
                                   .Replace(FileConstants.ReferenceReplace,refBuilder.ToString());
           
            await WriteTextAsync(filePath, opf);         
        }

        private async Task CreateTitle(string filePath)
        {
            var title = FileConstants.TitleContent;
            await WriteTextAsync(filePath,title);
        }

        private async Task CreateTOC(string filePath, string uuid)
        {
            var toc = FileConstants.TOCContent
                                   .Replace(FileConstants.UUIDReplace, uuid);
            await WriteTextAsync(filePath, toc);
        }

        public static async Task WriteTextAsync(string filePath, string text)
        {
            var encodedText = Encoding.UTF8.GetBytes(text.Trim());

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
