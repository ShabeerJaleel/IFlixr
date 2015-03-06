using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using HtmlAgilityPack;
using System.Threading.Tasks;
using IFlixr.Base;

namespace IFlixr.WebScraper
{
    public abstract class Scraper
    {
        public static readonly string[] UserAgents = new string[]
        {
            "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11\r\n",
            "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/16.0\r\n",
            //"User-Agent: Surf/0.4.1 (X11; U; Unix; en-US) AppleWebKit/531.2+ Compatible (Safari; MSIE 9.0)\r\n",
            "User-Agent: Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25\r\n",
            "User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; AOL 9.7; AOLBuild 4343.19; Windows NT 6.1; WOW64; Trident/5.0; FunWebProducts)",
            "User-Agent: Opera/12.80 (Windows NT 5.1; U; en) Presto/2.10.289 Version/12.02",
            //"User-Agent: Mozilla/5.0 (Windows NT 5.2; RW; rv:7.0a1) Gecko/20091211 SeaMonkey/9.23a1pre"

        };
        public static DateTime LastFetchTime;
        private static int lastIndex = -1;

        static Scraper()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;
        }

        public HtmlAgilityPack.HtmlDocument GetDocument(string url, bool useBrowser = true)
        {
            if (useBrowser)
            {
                //WbeBrowser to avoid getting blocked from Google.WebBrowser is IE
                var seconds = (int)DateTime.Now.Subtract(LastFetchTime).TotalSeconds;
                if(seconds < 5)
                    Thread.Sleep(5000 - (seconds * 1000));
                LastFetchTime = DateTime.Now;
                var mainDoc = new HtmlAgilityPack.HtmlDocument();
                using (WebBrowser browser = new WebBrowser())
                {
                    browser.ScriptErrorsSuppressed = true;
                    lastIndex = lastIndex == 4 ? 0 : ++lastIndex;
                    browser.Navigate(url, null, null, UserAgents[lastIndex]);
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }

                    if (browser.Url.AbsolutePath.Contains("Captcha"))
                    {
                        MessageBox.Show("Captcha");
                    }

                    mainDoc.LoadHtml(browser.DocumentText);
                }
                
                return mainDoc;
            }
            else
                return new HtmlWeb().Load(url);
        }


        public static List<VideoInfo> ScrapMovies(QueryParam param)
        {
            param.Duration = VideoDuration.Any;
            var allVideos = new List<VideoInfo>();
            if ((param.Source == VideoSource.Any || param.Source == VideoSource.Youku) && !param.FilterByUploader)
            {
                param.Duration = VideoDuration.Long;
                var youkuVideos = new YoukuScraper().Search(param);
                allVideos.AddRange(youkuVideos);
            }

            if (param.Source == VideoSource.Any  && !param.FilterByUploader)
            {
                var abcVideos = new ABCMalayalamScraper().Search(param);
                allVideos.AddRange(abcVideos);
            }
            
            if (param.Source == VideoSource.Any || param.Source == VideoSource.DailyMotion)
            {
                var dmVideos = new DailyMotionScraper().Search(param);
                allVideos.AddRange(dmVideos);
            }
            
            if (param.Source == VideoSource.Any || param.Source == VideoSource.Youtube)
            {
                var youtubeVideos = new YouTubeScraper().GetMovies(param);
                allVideos.AddRange(youtubeVideos);
            }

           var repeatedItems = allVideos.Where(x => x.Duration == 0)
                                        .ToList();
           repeatedItems.ForEach(x =>
               {
                   if (allVideos.Any(y => y.VideoId == x.VideoId && y.Duration > 0))
                       allVideos.Remove(x);
               });

            

            //Parallel.Invoke(() =>
            //{
            //    var youkuVideos = new YoukuScraper().Search(fullSearhText, keyword, VideoDuration.Long);
            //    var dmVideos = new DailyMotionScraper().Search(fullSearhText, keyword, VideoDuration.Long);
            //    lock (allVideos) { allVideos.AddRange(dmVideos); allVideos.AddRange(youkuVideos); }
            //},
            //() =>
            //{
            //    var dmVideos = new DailyMotionScraper().Search(fullSearhText, keyword, VideoDuration.Long);
            //    lock (allVideos) { allVideos.AddRange(dmVideos); }
            //},
            //() =>
            //{
            //    var youtubeVideos = new YouTubeScraper().GetMovies(fullSearhText, keyword, uploader);
            //    lock (allVideos) { allVideos.AddRange(youtubeVideos); }
            //});

     
            return allVideos;
        }
    }
}
