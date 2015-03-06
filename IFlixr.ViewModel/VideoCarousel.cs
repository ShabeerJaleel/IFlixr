using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.ViewModel
{
    public class Carousel : BaseViewModel
    {
        public Carousel()
        {
            TitleUrl = "#";
            DisplayTitle = true;
        }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public bool DisplayTitle { get; set; }
    }

    public class VideoCarousel : Carousel
    {
        public List<ThumbnailVideo> Videos { get; set; }
    }
}
