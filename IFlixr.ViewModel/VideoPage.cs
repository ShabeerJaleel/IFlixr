using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using IFlixr.Data.Model;

namespace IFlixr.ViewModel
{
    public class VideoPage 
    {
        public VideoPage()
        {
            Related = new List<VideoCarousel>();
        }
        public Video Video { get; set; }
        public List<VideoCarousel> Related { get; set; }
        public string UniqueId { get { return Part.UniqueId; } }

        public string GetUrl()
        {
            return Video.GenerateVideoUrl();
        }

        public VideoPart Part
        {
            get { return Video.Links[0].Parts[0]; }
        }

        public VideoLink Link
        {
            get { return Video.Links[0]; }
        }
        
    }

}
