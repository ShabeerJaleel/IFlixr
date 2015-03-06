using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Data;
using System.Threading;
using System.Net;
using System.IO;
using IFlixr.ImageProcessor;
using IFlixr.Cooking.Base;

namespace IFlixr.Cooking.Scraper
{
    public class FeedScraperService
    {
        #region Fields
        public event EventHandler<FeedScrapeProgessEventArgs> FeedScrapeProgess;
        private static FeedScraperService service;
        private static volatile bool scraping;
        private static volatile bool stopScraping;
        private static string imageFolder;
        #endregion

        private FeedScraperService()
        {

        }

        public static FeedScraperService Create()
        {
            if (service == null)
                service = new FeedScraperService();
            return service;
        }
        

        public void ScrapeAll(string imgFolder, DateTime? after = null)
        {
            if (scraping)
                throw new Exception("Scraping in progress");
            try
            {
                scraping = true;
                stopScraping = false;
                imageFolder = imgFolder;
                if (after == null)
                    after = DateTime.Now.AddDays(-2);
                var repo = RavenRepository.Create();

                foreach (var author in repo.GetAllUsersWithFeedScraping())
                {
                    foreach (var site in author.Sites)
                    {
                        if (!stopScraping && site.LastFeededTime <= after)
                        {
                            DoScrape(author, site, after);
                            site.LastFeededTime = DateTime.Now;
                        }
                    }
                    repo.SaveUser(author);

                }
            }
            catch
            {
                throw;
            }
            finally
            {
                scraping = false;
            }

        }

        public void ScrapeAll(string userId, string imgFolder, DateTime? after = null)
        {
            if (scraping)
                throw new Exception("Scraping in progress");

            try
            {
                scraping = true;
                stopScraping = false;
                imageFolder = imgFolder;

                if (after == null)
                    after = DateTime.Now.AddDays(-2);

                var repo = RavenRepository.Create();
                var user = repo.GetUser(userId);
                foreach (var site in user.Sites)
                {
                    if (site.LastFeededTime <= after)
                    {
                        DoScrape(user, site, after);
                        site.LastFeededTime = DateTime.Now;
                    }
                    
                }
                repo.SaveUser(user);
            }
            catch
            {
                throw;
            }
            finally
            {
                scraping = false;
            }
        }

        public void StopScraping()
        {
            if (scraping)
            {
                stopScraping = true;
                while (!scraping)
                    Thread.Sleep(10);
                stopScraping = false;
            }
        }

        private void DoScrape(User author, WebSite site, DateTime? after = null)
        {
            ReportProgess(author, "Scraping " + site.Url);

            var repo = RavenRepository.Create();
            var data = new RssReader().Read(site.FeedUrl, site.ImageCss);
            foreach (var d in data)
            {
                if (stopScraping)
                    return;

                if (!repo.IsRecipeExists(d.Url))
                {
                    ReportProgess(author, "Loading recipe " + d.Title);
                    var images = d.ImageUrls.Select(x => new RecipeImage
                                    {
                                        OriginalUrl = x
                                    }).ToList();
                    var removeItems = new List<RecipeImage>();
                    foreach (var img in images)
                        if (!ProcessImage(img))
                            removeItems.Add(img);
                    foreach (var item in removeItems)
                        images.Remove(item);
                    if (images.Count > 0)
                    {
                        var recipe = new Recipe
                         {
                             PostedById = author.Id,
                             PostedDate = d.PublishedDate,
                             Title = d.Title,
                             Url = d.Url,
                             ActiveTags = author.Tags,
                             Tags = d.Tags,
                             Images = images,
                         };
                        repo.SaveRecipe(recipe);
                    }
                }
                else
                {
                    ReportProgess(author, "Skipping recipe " + d.Title);
                }
            }
        }

        private void ReportProgess(User author, string message)
        {
            if (FeedScrapeProgess != null)
            {
                try
                {
                    FeedScrapeProgess.Invoke(this, new FeedScrapeProgessEventArgs(author, message));
                }
                catch { }
            }
        }

        private bool ProcessImage(RecipeImage ri)
        {
            var imgUtil = new ImageUtil();
            using (WebClient client = new WebClient())
            {
                var id = SecurityHelper.GenerateShortID(ri.OriginalUrl);
                var defaultImg = id + "_o.jpg";
                var thumbImg = id + "_s.jpg";
                var oPath = Path.Combine(imageFolder, defaultImg);
                var sPath = Path.Combine(imageFolder, thumbImg);


                if (!File.Exists(oPath))
                {

                    for (var i = 0; i < 3; i++)
                    {
                        try
                        {
                            client.DownloadFile(ri.OriginalUrl, oPath);
                            break;
                        }
                        catch { }
                    }
                }
                if (!File.Exists(oPath))
                    return false;

                if (!File.Exists(sPath))
                {
                    var img = imgUtil.Process(oPath, new CropDimension(), new ResizeDimension(270, 270, 270, 270), 125, 200);
                     if (img != null)
                         img.Write(sPath);
                }
                if (File.Exists(sPath))
                {
                    ri.ImageOriginal = defaultImg;
                    ri.ImageThumbnail = thumbImg;
                    return true;
                }
            }
            return false;
        }

        
    }

    public class FeedScrapeProgessEventArgs : EventArgs
    {
        public FeedScrapeProgessEventArgs(User author, string message)
        {
            Author = author;
            Message = message;

        }
        public User Author { get; private set; }
        public string Message { get; private set; }
    }
}
