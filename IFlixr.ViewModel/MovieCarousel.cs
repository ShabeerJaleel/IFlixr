using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.ViewModel
{

    public class MovieCarousel : Carousel
    {
        public List<ThumbnailMovie> Movies { get; set; }
    }
}
