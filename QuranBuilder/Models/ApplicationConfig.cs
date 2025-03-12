using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranBuilder.Models
{
    class ApplicationConfig
    {
        public bool Preview { get; set; } //Only loads the first chapter
        public bool HasBangla { get; set; }
        public bool HasTranscript { get; set; }
    }
}
