using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Models
{
    public static class WebList
    {
        public static List<Website> Websites = new List<Website>()
        {
            new Website("ebanglalibrary.com",
                        "entry-title","entry-content","next",
                        ParserType.Class,ParserType.Class,ParserType.Rel),
            new Website("Others")
        };
    }

    public class Website
    {
        internal Website(string url)
        {
            BaseUrl = url;
        }
        internal Website(string url, string header,string content, string next,
                                   ParserType headerT,ParserType contentT, ParserType nextT)
        {
            BaseUrl = url;
            HeaderData = header;
            ContentData = content;
            NextData = next;
            HeaderType = headerT;
            ContentType = contentT;
            NextType = nextT;
        }

        public string BaseUrl { get; }
        public string HeaderData { get; }
        public string ContentData { get; }
        public string NextData { get; }
        public ParserType HeaderType { get; }
        public ParserType ContentType { get; }
        public ParserType NextType { get; }
    }
}
