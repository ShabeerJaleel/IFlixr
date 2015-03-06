using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper
{
    public class MovieName
    {
        public string Name { get; set; }
        public string WikiTitle { get; set; }
        public string WikiUrl { get; set; }
        public bool IsValidUrl { get; set; }
        public short Year { get; set; }
    }
}
