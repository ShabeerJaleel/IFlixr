using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;

namespace IFlixr.ViewModel
{
    public class CarouselItemModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }
        public string ModuleSizeCSSClass { get; set; }
        public bool DisplayYear { get; set; }
        public bool DisplayLanguage { get; set; }
        public string ImageUrl { get; set; }
        public string NavigationLink { get; set; }
        public bool DisplayHoverBox { get; set; }
        public int Year { get; set; }
    }

}
