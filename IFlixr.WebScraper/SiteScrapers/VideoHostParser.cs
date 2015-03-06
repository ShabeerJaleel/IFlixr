using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using System.Diagnostics;
using System.Reflection;

namespace IFlixr.WebScraper
{
    public class VideoHostParser
    {
        public VideoHostParser(string url)
        {
            Url = url;
            Source = FindSource(url);
            if (Source == VideoSource.Youtube)
            {
                if (url.Contains("list"))
                {
                    var start = url.IndexOf("list=") + 5;
                    Id = url.Substring(start, url.Length - start);
                    IsPlayList = true;
                }
                else
                    Id = GetVideoId(url, Source);
            }
            else if (Source == VideoSource.DailyMotion)
            {
                if (url.Contains("playlist"))
                {
                    var start = url.LastIndexOf("/") + 1;
                    Id = url.Substring(start, url.Length - start);
                    IsPlayList = true;
                }
                else
                    Id = GetVideoId(url, Source);
            }
            else
                Id = GetVideoId(url, Source);
        }

        public VideoSource Source { get; private set; }
        public string Id { get; private set; }
        public string Url { get; private set; }
        public bool IsPlayList { get; private set; }

        private string GetVideoId(string url, VideoSource source)
        {
            switch (source)
            {
                case VideoSource.NowVideo:
                case VideoSource.Youtube:
                case VideoSource.Putlocker:
                case VideoSource.Novamov:
                case VideoSource.SockShare:
                case VideoSource.VideoWeed:
                case VideoSource.MovShare:
                case VideoSource.DivxStage:
                case VideoSource.Zinwa:
                case VideoSource.DailyMotion:
                case VideoSource.VideoTT:
                    {
                        var start = url.LastIndexOf("/") + 1;
                        if (url.Contains("v="))
                            start = url.LastIndexOf("v=") + 2;
                        return url.Substring(start, url.Length - start);
                    }
                case VideoSource.Cloudy:
                    {
                        var start = url.LastIndexOf("/") + 1;
                        if (url.Contains("id="))
                            start = url.LastIndexOf("id=") + 3;
                        return url.Substring(start, url.Length - start);
                    }
                case VideoSource.Youku:
                    {
                        var start = url.LastIndexOf("sid/") + 4;
                        return url.Substring(start, 13);
                    }
                case VideoSource.NosVideo:
                    {
                        var start = url.LastIndexOf("embed/") + 6;
                        return url.Substring(start, url.LastIndexOf("/") - start);
                    }
                case VideoSource.GoogleDocs:
                    {
                        return url;
                    }
                default:
                    Debug.Assert(false);
                    return String.Empty;
            }
        }

        public static VideoSource FindSource(string url)
        {
            foreach (var en in Enum.GetValues(typeof(VideoSource)))
            {
                FieldInfo fi = en.GetType().GetField(en.ToString());

                SourceUrlAttribute attribute =
                    (SourceUrlAttribute)fi.GetCustomAttributes(
                    typeof(SourceUrlAttribute),
                    false)[0];

                if (attribute.IsValidUrl(url))
                    return (VideoSource)en;
            }
            
            return VideoSource.Unknown;
        }

        public static bool IgnoreHost(string url)
        {
            return url.Contains("megavideoz.") || url.Contains("flashx.");
        }
    }
}
