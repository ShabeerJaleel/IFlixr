using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;

namespace IFlixr.ViewModel
{
    public class ThumbnailVideo : BaseViewModel
    {
        public ThumbnailVideo()
        {
            Types = new List<VideoType>();
            Languages = new List<string>();
            Tags = new List<string>();
        }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public DateTime PostedDate { get; set; }
        public string PostedDateText
        {
            get
            {
                return (PostedDate == DateTime.MinValue ? String.Empty :  PostedDate.ToString("dd MMM, yyyy"));
            }
        }
        public string PostedBy { get; set; }
        public List<string> Languages { get; set; }
        public string PrimaryLanguage { get { return Languages.Count > 0 ? Languages.First().ToString().ToLower() : String.Empty; } }
        public string Description { get; set; }
        public List<VideoType> Types { get; set; }
        public List<string> Tags { get; set; }
        public string UniqueId { get; set; }
        public virtual string Url { get { return this.GenerateVideoUrl(); } }
        public ThumbnailSize Size { get; set; }

        public List<string> TypeTexts {
            get
            {
                var data = new List<string>();
                foreach (var t in Types)
                    data.Add(t.ToString().ToLower());
                return data;
            }
        }
    }

    public enum ThumbnailSize
    {
        Small,
        Normal,
        Large
    }
}
