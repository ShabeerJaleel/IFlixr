using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using System.Web.Mvc;

namespace IFlixr.ViewModel
{
    public class HoverBoxModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; }
        public Language Language { get; set; }
        public int Duration { get; set; }
        public string DurationText 
        {
            get
            {
                if (Duration > 0)
                {
                    var ts = new TimeSpan(0, Duration, 0);
                    return string.Format("Movie ({0} hrs {1} min)", ts.Hours,
                                ts.Minutes);
                }

                return String.Format("Movie");
            }
        }
        public MvcHtmlString DetailsText
        {
            get
            {
                var details = "<span>";
                if (ReleaseDate.Day != 1 && ReleaseDate.Month != 1)
                    details += ReleaseDate.ToShortDateString();
                else
                    details += ReleaseDate.Year.ToString();
                details += "</span>";
                if(!String.IsNullOrWhiteSpace(Rating))
                    details += String.Format("<span style=\"color:green\">{0}</span>", Rating);
                return new MvcHtmlString(details);
                
            }
        }
        public MvcHtmlString Description { get; set; }
        public string Footer { get; set; }

    }
}
