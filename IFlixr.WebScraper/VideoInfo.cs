using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;

namespace IFlixr.WebScraper
{
    public class VideoInfo
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int DurationInMinutes { get { return Duration / 60; } }
        public string Uploader { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Url { get; set; }
        public string VideoId { get; set; }
        public VideoSource Source { get; set; }
        public string PlaylistId { get; set; }
        public int PlaylistSequence { get; set; }
    }
}
