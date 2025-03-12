using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserEngine.Engines
{
    public interface IUrlParser
    {
        public Task<IDocument> OpenAsync(string url);
    }
}
