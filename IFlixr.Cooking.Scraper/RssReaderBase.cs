using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Diagnostics;
using CsQuery;

namespace IFlixr.Cooking.Scraper
{
    public class RssReader
    {
        public List<ScrapedRecipe> Read(string url, string cssForImage = "img")
        {
            var recipes = new List<ScrapedRecipe>();
            using (var reader = XmlReader.Create(url))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                foreach (SyndicationItem item in feed.Items)
                {
                    recipes.Add(new ScrapedRecipe{
                         Title = item.Title.Text,
                         ImageUrls = ReadImages(item, cssForImage),
                         Tags = item.Categories.Select(x => x.Name.ToLower()).ToList(),
                         Url = ReadUrl(item),
                         PublishedDate = item.PublishDate.ToLocalTime().DateTime
                    });
                }
                reader.Close();
            }
            return recipes;
        }

       

        protected virtual List<string> ReadImages(SyndicationItem item,string css)
        {
            var dom = CQ.CreateFromUrl(ReadUrl(item));
            return dom.Select(css).Elements.Select(x => x.Attributes["src"]).ToList();
        }

        protected virtual string ReadUrl(SyndicationItem item)
        {
            if (item.ElementExtensions != null)
            {
                var link = item.ElementExtensions.Where(x => x.OuterName == "origLink").FirstOrDefault();
                if(link != null)
                    return link.GetObject<XElement>().Value;
            }
            return item.Links.First().Uri.AbsoluteUri;
        }


    }
}
