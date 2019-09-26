using Microsoft.AspNetCore.Mvc.Rendering;
using ParserEngine.Engine;
using ParserEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookParserWeb.Models
{
    public class HomeViewModel
    {
        private Website _website;
       
        [Required]
        public string Url { get; set; }
        public string UrlPlaceholder { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Bookname { get; set; }
        [Required]
        public string ContentId { get; set; }
        [Required]
        public ParserType ContentIdType { get; set; }
        [Required]
        public string HeaderId { get; set; }
        [Required]
        public ParserType HeaderParserType { get; set; }
        [Required]
        public string NextPageId { get; set; }
        [Required]
        public ParserType NextPageIdType { get; set; }
        public SelectList ParserTypes { get; set; }

        public HomeViewModel()
        {
            var eLibrary = WebList.Websites.First();
            ContentId = eLibrary.ContentData;
            ContentIdType = eLibrary.ContentType;
            HeaderId = eLibrary.HeaderData;
            HeaderParserType = eLibrary.HeaderType;
            NextPageId = eLibrary.NextData;
            NextPageIdType = eLibrary.NextType;
        }
    }
}
