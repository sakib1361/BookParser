using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookParserWeb.Models;
using ParserEngine.Models;
using ParserEngine.Engine;
using System.IO;

namespace BookParserWeb.Controllers
{
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CreateBook(HomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book()
                {
                    Bookname = viewModel.Bookname
                };
                await Task.Delay(1000);
                //using var coreEngine = new CoreEngine();
                //var fileEngine = new FileEngine();
                //var parseRes = await coreEngine.Parse(book);
                //if (parseRes)
                //{
                //    var folderPath = Path.Combine(Path.GetTempPath(), AppConstants.AppName);
                //    //if (Directory.Exists(folderPath)) Directory.Delete(folderPath);
                //    Directory.CreateDirectory(folderPath);

                //    var exportFile = Path.Combine(folderPath, book.Bookname + ".epub");
                //    await fileEngine.CreateBook(book, exportFile);
                //    _logger.LogInformation("Book Created {0}", book.Bookname);
                //}
                book.Url = book.Bookname;
                return PartialView("_SuccessBook", book);
            }
            else return View("_PartialForm",viewModel);
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
                using var content = new MemoryStream(bData);
                var contentType = "APPLICATION/octet-stream";
                var fileName = data;
                return File(content, contentType, fileName);
            }
            else
            {
                return BadRequest();
            }
          
        }
    }
}
