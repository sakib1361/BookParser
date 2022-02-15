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
                        "entry-title","entry-content","next", "scriptlesssocialsharing mbfp-btn",
                        ParserType.Class,ParserType.Class,ParserType.Rel, ParserType.Class),
            new Website("Others")
        };
    }

    public class Website
    {
        internal Website(string url)
        {
            BaseUrl = url;
        }
        internal Website(string url, string header,string content, string next,string arbitary,
                                   ParserType headerT,ParserType contentT, ParserType nextT, ParserType aType)
        {
            BaseUrl = url;
            HeaderData = header;
            ContentData = content;
            NextData = next;
            HeaderType = headerT;
            ContentType = contentT;
            NextType = nextT;
            ArbitaryData = arbitary;
            ArbitaryType = aType;
        }

        public string BaseUrl { get; }
        public string HeaderData { get; }
        public string ContentData { get; }
        public string NextData { get; }
        public ParserType HeaderType { get; }
        public ParserType ContentType { get; }
        public ParserType NextType { get; }
        public string ArbitaryData { get; set; }
        public ParserType ArbitaryType { get; set; }
    }
}
