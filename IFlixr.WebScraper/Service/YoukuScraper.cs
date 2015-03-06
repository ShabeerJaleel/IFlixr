using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Google.GData.Client;
using IFlixr.Base;

namespace IFlixr.WebScraper
{
    public class YoukuScraper : GoogleVideoScraper
    {
        private static readonly string VideoUrl = "http://v.youku.com/v_show/id_{0}.html";
        public override List<VideoInfo> Search(QueryParam param)
        {
            return base.Search(param, "youku.com");
        }

        protected override string GetId(string url)
        {
            var start = url.IndexOf("id_") + 3;
            var end = url.IndexOf(".", start);
            return url.Substring(start, end - start);
        }

        protected override string GetUserName(string url)
        {
            return "Youku_User";
        }

        protected override VideoSource GetSource(string url)
        {
            return VideoSource.Youku;
        }

        public VideoInfo GetVideoByUrl(string url, string title, string uploader)
        {
            var start = url.LastIndexOf("sid/") + 4;
            var id = url.Substring(start, 13);

            return new VideoInfo()
            {
                Source = VideoSource.Youku,
                VideoId = id,
                Title = title,
                Url = String.Format(VideoUrl, id),
                Uploader = uploader,
                PublishedDate = DateTime.Now.Date
            };
        }
    }
}
