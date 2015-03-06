using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;
using IFlixr.Base;

namespace IFlixr.Data.Model
{
    [JsonObject(Title = "VideoLink")]
    public class VideoLink
    {
        public VideoLink()
        {
            Parts = new List<VideoPart>();
        }
        public int Index { get; set; }
        public string PrimaryUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PostedById { get; set; }
        public VideoSource Source { get; set; }
        public List<VideoPart> Parts { get; set; }

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public string PostedByName { get; set; }

        [JsonIgnore]
        public List<string> EmbedSnippets
        {
            get
            {
                var snippets = new List<string>();
                foreach (var part in Parts)
                {
                    if (Source == VideoSource.Youku)
                        snippets.Add(String.Format("//player.youku.com/embed/{0}?wmode=opaque", part.UniqueId));
                    else if (Source == VideoSource.Youtube)
                        snippets.Add(String.Format("//www.youtube.com/embed/{0}?autoplay=0&autohide=1&fs=1&rel=0&hd=1&wmode=opaque&enablejsapi=1&layout=button_count&show_faces=true&action=like&font&colorscheme=light", part.UniqueId));
                    else if (Source == VideoSource.DailyMotion)
                        snippets.Add(String.Format("//www.dailymotion.com/embed/video/{0}?wmode=opaque", part.UniqueId));
                    else if (Source == VideoSource.Putlocker)
                        snippets.Add(String.Format("//www.putlocker.com/embed/{0}?wmode=opaque", part.UniqueId));
                }
                return snippets;
            }
        }


        public string GetEmbedSnippet(string uniqueId)
        {
            var id = !String.IsNullOrWhiteSpace(uniqueId) && Parts.Any(x => x.UniqueId == uniqueId)
                ? uniqueId : Parts[0].UniqueId;


            if (Source == VideoSource.Youku)
                return String.Format("//player.youku.com/embed/{0}?wmode=opaque", id);
            else if (Source == VideoSource.Youtube)
                return (String.Format("//www.youtube.com/embed/{0}?autoplay=0&autohide=1&fs=1&rel=0&hd=1&wmode=opaque&enablejsapi=1&layout=button_count&show_faces=true&action=like&font&colorscheme=light", id));
            else if (Source == VideoSource.DailyMotion)
                return (String.Format("//www.dailymotion.com/embed/video/{0}?wmode=opaque", id));
            else if (Source == VideoSource.Putlocker)
                return (String.Format("//www.putlocker.com/embed/{0}?wmode=opaque", id));
            else
                return "#";

        }

    }
}
