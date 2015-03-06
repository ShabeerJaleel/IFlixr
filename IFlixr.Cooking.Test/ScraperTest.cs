using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.Cooking.Scraper;
using IFlixr.Cooking.Data;
using IFlixr.Cooking.Repository;
using ImageMagick;
using IFlixr.ImageProcessor;
using System.Net;
using System.IO;

namespace IFlixr.Cooking.Test
{
    [TestClass]
    public class ScraperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repo = RavenRepository.Create();
            var author = repo.GetAuthorByUrl("http://www.indiansimmer.com/");
            var data = RecipeScraperBase.ScrapeRecipies(author);
            foreach (var d in data)
            {
                repo.SaveRecipe(new Recipe
                 {
                     PostedById = author.Id,
                     Images = d.ImageUrls.Select(x => new RecipeImage
                     {
                         ImageThumbnail = x
                     }).ToList(),
                     PostedDate = DateTime.Now,
                     Title = d.Title,
                     Url = d.Url,

                 });
            }

           // var data = new AayisRecipes().ScrapeRecipies();
        }

       
        [TestMethod]
        public void PatchUpdate()
        {
            var repo = RavenRepository.Create();
            foreach (var recipe in repo.GetAllUsers())
            {
                recipe.ProfileUrl = recipe.ProfileName.Replace(' ', '-');
                repo.SaveUser(recipe);
            }
        }

        [TestMethod]
        public void RssReadTest()
        {

            var repo = RavenRepository.Create();

           foreach (var author in repo.GetAllUsersWithFeedScraping())
           {
               var data = new RssReader().Read(author.Sites.First().FeedUrl, author.Sites.First().ImageCss);
              data.Select(x => new Recipe
              {
                  PostedById = author.Id,
                  PostedDate = x.PublishedDate ,
                  Title = x.Title,
                  Url = x.Url,
                  ActiveTags = x.Tags,
                  Images = x.ImageUrls.Select(y => Create(y)).ToList()
              }).ToList().ForEach(x =>
              {
                  repo.SaveRecipe(x);
              });
           }
           
            //new RssReader().Read("http://www.hookedonheat.com/feed/", ".post * img");
            //new RssReader().Read("http://aapplemint.com/feed/", ".entry-content * img");
            //new RssReader().Read(" http://feeds.feedburner.com/WhatsForLunchHoney?format=xml", ".post-body * img");
            //new RssReader().Read("http://feeds.feedburner.com/QuickIndianCooking?format=xml", ".postContent * img");
            //new RssReader().Read("http://sinfullyspicy.com/feed/", ".entry-content * img");
            
        }

        private RecipeImage Create(string imgUrl)
        {
            var imgUtil = new ImageUtil(); 
            using(WebClient client = new WebClient())
            {
                var file = "temp.jpg";
                var path = Path.Combine(@"C:\Cooking\Images",file);
               client.DownloadFile(imgUrl, path);
               var img = imgUtil.Process(path, new CropDimension(), new ResizeDimension(270,270,270,270), 120, 20);
                file = Guid.NewGuid().ToString();
                path = Path.Combine(@"C:\Cooking\Images",file);
                img.Write(path);
                return new RecipeImage
                  {
                      OriginalUrl = imgUrl,
                      ImageThumbnail = file,
                  };
            }
          
        }

        [TestMethod]
        public void ImageManipulationTest()
        {
            var img = new MagickImage();
           // img.Resize(
        }
    }
}
