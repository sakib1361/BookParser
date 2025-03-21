﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParserEngine.Models
{
    public class Book
    {
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string Bookname { get; set; }
        public string Author { get; set; }
        public List<Chapter> Chapters { get;}
        //public ParseInfo TitleInfo { get; set; }
        //public ParseInfo ContentInfo { get; set; }
        //public ParseInfo NextChapterInfo { get; set; }
        //public ParseInfo ArbitaryInfo { get; set; }
        public string EncodedImage { get; set; }
        public string DownloadLink { get; set; }
        public string ImagePath { get; internal set; }

        public Book()
        {
            Chapters = new List<Chapter>();
            //TitleInfo = new ParseInfo();
            //ContentInfo = new ParseInfo();
            //NextChapterInfo = new ParseInfo();
            //ArbitaryInfo = new ParseInfo();
        }
    }

    //public class ParseInfo
    //{
    //    public string ParseValue { get; set; }
    //    public ParserType ParserType { get; set; }
    //    public ParseInfo()
    //    {

    //    }
    //    public ParseInfo(ParserType parserType, string value)
    //    {
    //        ParserType = parserType;
    //        ParseValue = value;
    //    }
    //}

    public enum ParserType
    {
        Id,
        Class,
        Rel
    }

    public class Chapter
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string NextUrl { get; set; }
        public string CurrentUrl { get; internal set; }
    }
}
