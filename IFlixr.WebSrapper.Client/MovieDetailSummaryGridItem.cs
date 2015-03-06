using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper.Client
{
    public class MovieDetailSummaryGridItem
    {
        public MovieEntityType Type { get; set; }
        public string Header { get { return Type.ToString(); } }
        public string Text { get; set; }
        public object Data { get; set; }
    }
}
