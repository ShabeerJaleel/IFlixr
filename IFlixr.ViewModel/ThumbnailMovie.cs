using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;

namespace IFlixr.ViewModel
{

    public class ThumbnailMovie : ThumbnailVideo
    {
        public ThumbnailMovie()
        {
            Size = ThumbnailSize.Normal;
        }
        public DateTime ReleasedDate { get; set; }
        public string DescriptionUrl { get; set; }
        public override string Url
        {
            get
            {
                return this.GenerateVideoUrl();
            }
        }

        public string ReleaseYearText { get { return ReleasedDate.Year.ToString(); } }
    }
}
