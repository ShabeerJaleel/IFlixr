using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;

namespace IFlixr.ViewModel
{
    public class SearchResult : BaseModel
    {
        public IEnumerable<CarouselItemModel> Items { get; set; }
    }

    public class SearchResultItem : BaseModel
    {

    }
}
