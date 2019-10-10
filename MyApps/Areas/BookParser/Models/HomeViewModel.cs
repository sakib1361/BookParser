using Microsoft.AspNetCore.Mvc.Rendering;
using ParserEngine.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyApps.Areas.BookParser.Models
{
    public class HomeViewModel
    {
        [Required]
        [Url]
        public string Url { get; set; }
        public string UrlPlaceholder { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Bookname { get; set; }
        [Required]
        public string ContentId { get; set; }
        [Required]
        public string ArbitaryInfo { get; set; }
        [Required]
        public ParserType ArbitaryType { get; set; }
        [Required]
        public ParserType ContentIdType { get; set; }
        [Required]
        public string HeaderId { get; set; }
        [Required]
        public ParserType HeaderParserType { get; set; }
        [Required]
        public string NextPageId { get; set; }
        [Required]
        [Display(Name = "Next Page Id Type")]
        public ParserType NextPageIdType { get; set; }

        public HomeViewModel()
        {
            var eLibrary = WebList.Websites.First();
            ContentId = eLibrary.ContentData;
            ContentIdType = eLibrary.ContentType;
            HeaderId = eLibrary.HeaderData;
            HeaderParserType = eLibrary.HeaderType;
            NextPageId = eLibrary.NextData;
            NextPageIdType = eLibrary.NextType;
            ArbitaryInfo = eLibrary.ArbitaryData;
            ArbitaryType = eLibrary.ArbitaryType;
            UrlPlaceholder = "https://" + eLibrary.BaseUrl;
        }
    }
}
