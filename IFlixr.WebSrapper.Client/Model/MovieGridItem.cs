using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper.Client.Model
{
    public class MovieGridItem
    {
        public int Index { get; set; }
        public bool IsInDB { get; set; }
        public bool IsMovieVideoAvailable { get; set; }
        public bool IsMusicVideoAvailable { get; set; }
        public bool IsTrailerVideoAvailable { get; set; }
        public bool IsImageAvailable { get; set; }
        public bool IsClipAvailable { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Language { get; set; }
        public string Link { get; set; }
        public MovieName Entity { get; set; }
    }
}
