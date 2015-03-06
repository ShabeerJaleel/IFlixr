using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.ViewModel
{
    public class HomePage : BaseViewModel
    {
        public HomePage()
        {
            VideoCarousels = new List<VideoCarousel>();
            MovieCarousels = new List<MovieCarousel>();
        }
        public List<VideoCarousel> VideoCarousels { get; set; }
        public List<MovieCarousel> MovieCarousels { get; set; }
    }
}
