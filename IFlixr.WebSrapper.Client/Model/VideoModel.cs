using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using IFlixr.Base;

namespace IFlixr.WebScraper.Client.Model
{
    public class VideoModel 
    {
        [IgnoreMap]
        public bool Selected { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int DurationInMinutes { get { return Duration < 60 ? 0 : Duration/60; } }
        public string Uploader { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Url { get; set; }
        public string VideoId { get; set; }
        public int Index { get; set; }
        public int PlaylistSequence { get; set; }
        public string PlaylistId { get; set; }
        

        [IgnoreMap]
        public VideoType Type { get; set; }
        public VideoSource Source { get; set; }

        public VideoModel Clone()
        {
           var vm = (VideoModel)this.MemberwiseClone();
           vm.Index = 0;
           vm.PlaylistSequence = 0;
           return vm;
        }
    }

   
}
