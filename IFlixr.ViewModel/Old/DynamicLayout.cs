using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.ViewModel
{
    public class DynamicLayout
    {
        public DynamicLayout()
        {
            Carousels = new List<CarouselModel>();
        }
        public List<CarouselModel> Carousels { get; set; }
    }
}
