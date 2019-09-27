﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApps.Areas.BookParser.Models;
using MyApps.Models.Core;
using ParserEngine.Engine;
using ParserEngine.Models;

namespace MyApps.Areas.BookParser.Controllers
{
    [Area(ASPConstants.BookParserArea)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        public async Task<IActionResult> CreateBook(HomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book()
                {
                    Bookname = viewModel.Bookname,
                    Url = viewModel.Url,
                    Author = viewModel.Author,
                    ContentInfo = new ParseInfo(viewModel.ContentIdType, viewModel.ContentId),
                    NextChapterInfo = new ParseInfo(viewModel.NextPageIdType, viewModel.NextPageId),
                    TitleInfo = new ParseInfo(viewModel.HeaderParserType, viewModel.HeaderId)
                };
                try
                {
                    var folderPath = Path.Combine(Path.GetTempPath(), AppConstants.AppName);
                    Directory.CreateDirectory(folderPath);
                    var bookFactory = Path.Combine(folderPath, book.Bookname + "_Factory");
                    if (Directory.Exists(bookFactory))
                    {
                        Directory.Delete(bookFactory, true);
                    }
                    Directory.CreateDirectory(bookFactory);
                    book.FilePath = bookFactory;

                    using var coreEngine = new CoreEngine();
                    var fileEngine = new FileEngine();
                    var parseRes = await coreEngine.Parse(book);
                    if (parseRes)
                    {
                        var exportFile = Path.Combine(folderPath, book.Bookname + ".epub");
                        await fileEngine.CreateBook(book, exportFile);
                        _logger.LogInformation("Book Created {0}", book.Bookname);
                    }
                    book.DownloadLink = book.Bookname;
                    return PartialView("_SuccessBook", book);
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex.Message;
                    return PartialView("_PartialForm", viewModel);
                }
            }
            else return PartialView("_PartialForm", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadBook(string data)
        {
            var folderPath = Path.Combine(Path.GetTempPath(), AppConstants.AppName);
            //if (Directory.Exists(folderPath)) Directory.Delete(folderPath);
            Directory.CreateDirectory(folderPath);

            var exportFile = Path.Combine(folderPath, data + ".epub");
            if (System.IO.File.Exists(exportFile))
            {
                var bData = await System.IO.File.ReadAllBytesAsync(exportFile);
                var content = new MemoryStream(bData);
                var contentType = "APPLICATION/octet-stream";
                var fileName = Path.GetFileName(exportFile);
                return File(content, contentType, fileName);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}