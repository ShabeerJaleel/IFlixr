using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.Scraper
{
    public class ScraperLogic
    {
        public string Url { get; set; }
        public string Css { get; set; }
        public ScraperStepType StepType { get; set; }
        public ScraperLogic NextStep { get; set; }
    }

    public enum ScraperStepType
    {
        MoveToUrl,
        SelectList,
        Loop,
        ReadAttribute,
        ReadText,
        ReadUrl
        
    }
}
