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
    
    public class GoogleVideoScraper : Scraper
    {
        private static readonly string SearchUrl = "http://www.google.co.uk/search?q={0}{1}&hl=en&safe=off&tbm=vid&source=lnt&{2}&sa=X";


        public virtual List<VideoInfo> Search(QueryParam param)
        {
            return Search(param, String.Empty);
        }

        protected List<VideoInfo> Search(QueryParam param, string site)
        {
            string completeUrl = String.Format(SearchUrl, param.FullSearhText.Trim(),
                  String.IsNullOrWhiteSpace(site) ? String.Empty: "+site:" + site,
                  param.Duration == VideoDuration.Any ? String.Empty :
                  (param.Duration == VideoDuration.Long ? "tbs=dur:l" :
                  (param.Duration == VideoDuration.Medium ? "tbs=dur:m" : "tbs=dur:s")));

           var document = GetDocument(completeUrl);
           var nodes = document.DocumentNode.CssSelect("li.videobox").ToList();

            var videos = new List<VideoInfo>();
            foreach (var node in nodes)
            {
                var anchorNode = node.CssSelect("h3>a").FirstOrDefault();
                var title = HttpUtility.HtmlDecode(anchorNode.InnerText).ToLowerInvariant();

                if (title.ContainsAnyString(param.KeywordTokens))
                        videos.Add(PrepareVideoInfo(node));
               
            }
            
            return videos;
        }

        protected virtual VideoInfo PrepareVideoInfo(HtmlNode node)
        {
            var anchorNode = node.CssSelect("h3>a").FirstOrDefault();
            var title = HttpUtility.HtmlDecode(anchorNode.InnerText).ToLowerInvariant();
            var link = anchorNode.Attributes["href"].Value;
            var durationNode = node.CssSelect("span.vdur").FirstOrDefault();
            var duration = 0;
            if (durationNode != null)
            {
                try
                {
                    var tokens = durationNode.InnerText.Substring(13, durationNode.InnerText.Length - 13).Split(':');
                    if (!String.IsNullOrWhiteSpace(tokens[0]))
                    {
                        if (tokens.Length == 2) //minute:sec
                            duration = Int32.Parse(tokens[0]) * 60;
                        else
                            duration = Int32.Parse(tokens[0]);
                    }

                    if (tokens.Length == 2 && !String.IsNullOrWhiteSpace(tokens[1]))
                        duration = duration + Int32.Parse(tokens[1]);
                }
                catch { duration = -1; }
            }

            var publishedDate = DateTime.Now;
            var dateNode = node.CssSelect("span.f > span").FirstOrDefault();
            if (dateNode != null && !String.IsNullOrWhiteSpace(dateNode.InnerText))
                publishedDate = DateTime.Parse(dateNode.InnerText);

            return new VideoInfo()
            {
                Duration = duration,
                Source = GetSource(link),
                Title = title,
                Url = link,
                Uploader = GetUserName(link),
                VideoId = GetId(link),
                PublishedDate = publishedDate
            };
        }

        protected virtual string GetId(string url)
        {
            return url;
        }

        protected virtual string GetUserName(string url)
        {
            return url;
        }

        protected virtual VideoSource GetSource(string url)
        {
            return VideoSource.Unknown;
        }
      
    }

   
}
