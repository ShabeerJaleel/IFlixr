using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;

namespace IFlixr.ViewModel
{
    public class BannerModel : BaseModel
    {
        public IEnumerable<BannerItem> Items { get; set; }
    }
}
