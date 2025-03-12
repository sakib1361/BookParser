using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranBuilder.Models
{
    class ApiResponse<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
    }

    class JuzResponse
    {
        public int Number { get;set; }

    }

    class Ayahs
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public Surah Surah { get; set; }
        public int Page { get;set; }
        public int Juz { get; set; }
    }

    class Surah
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string RevelationType { get; set; }
    }
}
