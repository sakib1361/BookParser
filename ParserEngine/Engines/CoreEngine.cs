using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Xhtml;
using ParserEngine.Models;

namespace ParserEngine.Engine
{
    public abstract class CoreEngine
    {

        protected string GetUrl(IDocument document, string parser, ParserType parserType, bool pos = true)
        {
            var element = GetElement(document, parser, parserType, pos);
            if (element != null && element.IsLink())
                return element.GetAttribute("href");
            else return null;
        }

        protected string GetImage(IDocument document, string parser, ParserType parserType, bool pos = true)
        {
            var element = GetElement(document, parser, parserType, pos);
            if (element != null)
                return element.GetAttribute("src");
            else return null;
        }

        protected string GetData(IDocument document, string parser, ParserType parserType, bool pos = true)
        {
            var element = GetElement(document, parser, parserType, pos);
            return element?.ToHtml(XhtmlMarkupFormatter.Instance).Trim();
        }

        protected IElement GetElement(IDocument document, string parser, ParserType parserType, bool isFirst)
        {
            switch (parserType)
            {
                case ParserType.Class:
                    if (isFirst) return document.All.FirstOrDefault(m => m.ClassList.Contains(parser));
                    else return document.All.LastOrDefault(m => m.ClassList.Contains(parser));
                case ParserType.Id:
                    if (isFirst) return document.All.FirstOrDefault(x => x.Id == parser);
                    else return document.All.LastOrDefault(x => x.Id == parser);
                case ParserType.Rel:
                    var rDatas = document.All
                                        .Where(x => x.LocalName == "a" && x.HasAttribute("rel") && x.GetAttribute("rel") == parser);
                    if (isFirst) return rDatas.FirstOrDefault();
                    else return rDatas.LastOrDefault();
            }
            return null;
        }
    }
}
