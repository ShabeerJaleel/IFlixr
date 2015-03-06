using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper
{
    public class Selector
    {
        public string Text { get; set; }
        public SelectorType Type { get; set; }

    }

    public enum SelectorType
    {
        Css,
        XPath
    }
}
