using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public enum VideoSource : byte
    {
        [SourceUrl("")]
        Unknown = 255,
        [SourceUrl("")]
        Any = 254,
        [SourceUrl("youtube.")]
        Youtube = 0,
        [SourceUrl("dailymotion.")]
        DailyMotion = 1,
        [SourceUrl("youku.")]
        Youku = 2,
        [SourceUrl("vimeo.")]
        Vimeo = 3,
        [SourceUrl("nosvideo.")]
        NosVideo = 4,
        [SourceUrl("putlocker.")]
        Putlocker = 5,
        [SourceUrl("nowvideo.")]
        NowVideo = 6,
        [SourceUrl("novamov.")]
        Novamov = 7,
        [SourceUrl("sockshare.")]
        SockShare = 8,
        [SourceUrl("videoweed.")]
        VideoWeed = 9,
        [SourceUrl("cloudy.")]
        Cloudy = 10,
        [SourceUrl("movshare.")]
        MovShare = 11,
        [SourceUrl("divxstage.")]
        DivxStage = 12,
        [SourceUrl("zinwa.")]
        Zinwa = 13,
        [SourceUrl("video.tt")]
        VideoTT = 14,
        [SourceUrl("docs.google.com")]
        GoogleDocs = 15,
    }

    public enum VideoDuration
    {
        Any,
        Short,
        Medium,
        Long
    }

    public class SourceUrlAttribute : Attribute
    {
        private List<string> validUrls = new List<string>();
        public SourceUrlAttribute(params string[] urls)
        {
            validUrls = new List<string>(urls);
        }
        public bool IsValidUrl(string url)
        {
            foreach (var validUrl in validUrls)
            {
                if (String.IsNullOrWhiteSpace(validUrl))
                    continue;

                if (url.ToLower().Contains(validUrl))
                    return true;
            }
            return false;
        }
    }
}
