using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using Raven.Imports.Newtonsoft.Json;

namespace IFlixr.Data.Model
{
    [JsonObject(Title="Video")]
    public class Video : RavenBase
    {
        public Video()
        {
            Types = new List<VideoType>();
            Tags = new List<string>();
            Languages = new List<string>();
            Links = new List<VideoLink>();
            Images = new List<VideoImage>();
            RelatedVideos = new List<string>();
        }
        public string Title { get; set; }
        public List<VideoType> Types { get; set; }
        public List<string> Tags { get; set; }
        public Country Country { get; set; }
        public List<string> Languages { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public short Duration { get; set; }
        public string DescriptionUrl { get; set; }
        public string PostedById { get; set; }
        public List<VideoLink> Links { get; set; }
        public List<VideoImage> Images { get; set; }
        public List<string> RelatedVideos { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public CensorRating Rating { get; set; }


        #region Helper 
        [JsonIgnore]
        public string PostedByName { get; set; }
        [JsonIgnore]
        public string DurationText
        {
            get
            {
                if (Duration > 0)
                {
                    var ts = new TimeSpan(0, Duration, 0);
                    return String.Format("{0} hrs {1} min", ts.Hours,
                                ts.Minutes);
                }

                return String.Empty;
            }

        }
        [JsonIgnore]
        public string ShortDescription { 
            get 
            {
                if (String.IsNullOrWhiteSpace(Description))
                    return Title;
                return Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
            } 
        }
        #endregion
    }

}
