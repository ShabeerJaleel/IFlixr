using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;

namespace IFlixr.Cooking.Scraper
{
    public class IndianSimmer : RecipeScraperBase
    {
        public override string RootUrl
        {
            get { return "http://www.indiansimmer.com/"; }
        }

        public override List<ScrapedRecipe> ScrapeRecipies()
        {
            var recipies = new List<ScrapedRecipe>();
            var dom = GotoUrl(RootUrl);
            foreach (var elem in SelectItems(dom, "ul#menu li ul li a"))
            {
                dom = GotoUrl(ReadAttribute(elem, "href")); 
                foreach (var bElem in SelectItems(dom, "div.post-body div ul li a"))
                {
                    var sr = new ScrapedRecipe { Title = ReadText(bElem), 
                        Url = ReadAttribute(bElem, "href") };
                    dom = GotoUrl(sr.Url);
                    foreach (var cElem in SelectItems(dom, "div.post-body * img"))
                        sr.ImageUrls.Add(cElem.Attributes["src"]);
                    recipies.Add(sr);
                }

            }
            return recipies;
        }
    }

    public class AayisRecipes : RecipeScraperBase
    {
        public override string RootUrl
        {
            get { return "http://www.aayisrecipes.com/all-recipes-and-posts/"; }
        }

        public override List<ScrapedRecipe> ScrapeRecipies()
        {
            var recipies = new List<ScrapedRecipe>();
            var dom = GotoUrl(RootUrl);
            foreach (var elem in SelectItems(dom, ".tocc_blog_post a"))
            {
                var sr = new ScrapedRecipe
                {
                    Title = ReadText(elem),
                    Url = ReadAttribute(elem, "href")
                };

                dom = GotoUrl(sr.Url);


                foreach (var cElem in SelectItems(dom, ".entry-content * img"))
                    sr.ImageUrls.Add(cElem.Attributes["src"]);
                recipies.Add(sr);
            }
            return recipies;
        }
    }
}
