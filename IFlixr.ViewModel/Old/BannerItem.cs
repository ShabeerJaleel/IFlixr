using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;

namespace IFlixr.ViewModel
{
    public  class BannerItem : BaseModel
    {
        public BannerItem()
        {
            Height = 400;
        }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public abstract string ViewName { get; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
