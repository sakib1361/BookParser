﻿using ParserEngine.Engine;
using ParserEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LogEngine.ErrorOccured += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            };
            LogEngine.InfoOccured += (s, e) => Console.WriteLine(e);
            RunLibraryDownload();
            Console.ReadLine();
        }
        private async static void RunPratilipiDownload()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = Path.GetTempPath() + "Test";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var book = new Book()
            {
                Author = "Bankim",
                Bookname = "AnondoMoth",
                ContentInfo = new ParseInfo(ParserType.Class, "content-section lh-md p-lr-15 fontStyleObject"),
                FilePath = path,
                NextChapterInfo = new ParseInfo(ParserType.Rel, "next"),
                TitleInfo = new ParseInfo(ParserType.Class, "chapter-title p-lr-15"),
                Url = "https://www.ebanglalibrary.com/anubadboi/%e0%a7%a6%e0%a7%a7-%e0%a6%ac%e0%a6%bf%e0%a6%b6%e0%a7%8d%e0%a6%ac%e0%a6%9c%e0%a7%81%e0%a6%a1%e0%a6%bc%e0%a7%87-%e0%a6%b8%e0%a6%ae%e0%a6%be%e0%a6%a6%e0%a7%83%e0%a6%a4-%e0%a6%86%e0%a6%87%e0%a6%ab/"
            };

            var core = new CoreEngine();
            await core.Parse(book);
            var fEngine = new FileEngine();
            await fEngine.CreateBook(book, Path.Combine(desktop, book.Bookname + ".epub"));
        }
        private async static void RunLibraryDownload()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = Path.GetTempPath() + "Test";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var book = new Book()
            {
                Author = "Test",
                Bookname = "Test",
                ContentInfo = new ParseInfo(ParserType.Class, "entry-content"),
                FilePath = path,
                NextChapterInfo = new ParseInfo(ParserType.Rel, "next"),
                TitleInfo = new ParseInfo(ParserType.Class, "entry-title"),
                Url = "https://www.ebanglalibrary.com/anubadboi/%e0%a7%a6%e0%a7%a7-%e0%a6%ac%e0%a6%bf%e0%a6%b6%e0%a7%8d%e0%a6%ac%e0%a6%9c%e0%a7%81%e0%a6%a1%e0%a6%bc%e0%a7%87-%e0%a6%b8%e0%a6%ae%e0%a6%be%e0%a6%a6%e0%a7%83%e0%a6%a4-%e0%a6%86%e0%a6%87%e0%a6%ab/"
            };

            var core = new CoreEngine();
            await core.Parse(book);
            var fEngine = new FileEngine();
            await fEngine.CreateBook(book, Path.Combine(desktop, book.Bookname + ".epub"));
        }
    }
}
