using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.Scraper
{
    public class ScrapedRecipe
    {
        public ScrapedRecipe()
        {
            ImageUrls = new List<string>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<string> Tags { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
