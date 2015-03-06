using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using ScrapySharp.Extensions;
using Google.GData.Client;
using System.Diagnostics;

namespace IFlixr.WebScraper
{
    public class GoogleWebScraper : Scraper
    {
        private static readonly string SearchUrl = "http://www.google.co.uk/search?q={0}{1}&hl=en&safe=off&sa=X";

        public virtual List<VideoInfo> Search(QueryParam param)
        {
            return new List<VideoInfo>(); ;
        }

        protected List<Uri> Search(QueryParam param, string site)
        {
            string completeUrl = String.Format(SearchUrl, param.FullSearhText.Trim(),
                  String.IsNullOrWhiteSpace(site) ? String.Empty : "+site:" + site);

            var document = GetDocument(completeUrl);
            var nodes = document.DocumentNode.CssSelect("li.g").ToList();

            var links = new List<Uri>();
            foreach (var node in nodes)
            {
                var anchorNode = node.CssSelect("h3>a").FirstOrDefault();
                var title = HttpUtility.HtmlDecode(anchorNode.InnerText).ToLowerInvariant();

                if (title.ContainsAnyString(param.KeywordTokens))
                {
                    var link = anchorNode.Attributes["href"].Value;
                    Uri uri;
                    if (Uri.TryCreate(link, UriKind.Absolute, out uri))
                    {
                        links.Add(new Uri(link));
                    }else
                    {
                        var start = link.IndexOf("/url?q=");
                        if (start == -1)
                        {
                            Debug.Assert(false);
                        }
                        start = start + 7;
                        link.Substring(start, link.IndexOf("&amp") - start);
                        links.Add(new Uri(link));
                    }
                }
            }

            return links;
        }
    }
}
