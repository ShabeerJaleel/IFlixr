using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.ViewModel
{
    public class MovieBrowsePage : BaseViewModel
    {
        public ListEx<ThumbnailMovie> Movies { get; set; }
    }
}
