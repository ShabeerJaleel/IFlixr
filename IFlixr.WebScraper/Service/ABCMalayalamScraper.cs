using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using ScrapySharp.Extensions;
using System.Diagnostics;

namespace IFlixr.WebScraper
{
    public class ABCMalayalamScraper : GoogleWebScraper
    {
        private static readonly string SiteUrl = "abcmalayalam.com";

        public override List<VideoInfo> Search(QueryParam param)
        {
           var videos = new List<VideoInfo>();
           var links = Search(param,SiteUrl);
           foreach (var link in links)
           {
               if (link.Host == SiteUrl && link.AbsolutePath.Where(x => x == '/').Count() == 1)
               {
                   var document = GetDocument(link.AbsoluteUri, false);
                   var playerNodes = document.DocumentNode.CssSelect("div.avPlayerContainer").ToList();
                   foreach (var pNode in playerNodes)
                   {
                       var url = String.Empty;
                       var src = pNode.CssSelect("iframe").FirstOrDefault();//.Attributes["src"].Value;
                       if (src != null)
                       {
                          url = src.Attributes["src"].Value;
                       }
                       else
                       {
                           src = pNode.CssSelect("object").FirstOrDefault();
                           url = src.Attributes["data"].Value;
                       }
                       videos.AddRange(PrepareVideoInfo(url, param.Keyword));

                   }
               }
           }
           return videos;
        }

        private List<VideoInfo> PrepareVideoInfo(string url, string title)
        {
            var list = new List<VideoInfo>();

            if (url.Contains("youtube"))
            {
                if (url.Contains("list"))
                {
                    var start = url.IndexOf("list=") + 5;
                    var videos = new YouTubeScraper().GetVideosByPlaylistId(url.Substring(start, url.Length - start));
                    if (videos != null)
                        list.AddRange(videos);
                }
                else
                {
                    var id = GetVideoId(url, VideoSource.Youtube);
                    if (!String.IsNullOrWhiteSpace(id))
                        list.Add(new YouTubeScraper().GetVideoById(id));
                }
            }
            else if (url.Contains("dailymotion"))
            {
                if (url.Contains("playlist"))
                {
                    var start = url.LastIndexOf("/") +1;
                    var videos = new DailyMotionScraper().GetVideosByPlaylistId(url.Substring(start, url.Length - start));
                    if (videos != null)
                        list.AddRange(videos);
                }
                else
                {
                    //var id = GetVideoId(url, VideoSource.Youtube);
                    //if (!String.IsNullOrWhiteSpace(id))
                    //    list.Add(new YouTubeScraper().GetVideoById(id));
                    Debug.Assert(false);
                }

            }
            else if (url.Contains("youku"))
            {
                list.Clear();
                var vd = new YoukuScraper().GetVideoByUrl(url, title, "ABCMalayalam");
                list.Add(vd);
            }
            else if (url.Contains("nosvideo"))
            {
                var id = GetVideoId(url, VideoSource.NosVideo);

                list.Add(new VideoInfo
                {
                    Source = VideoSource.NosVideo,
                    VideoId = id,
                    Url = String.Format("http://nosvideo.com/?v={0}", id),
                    Uploader = "ABC_" + id,
                    Title = title,
                    PublishedDate = DateTime.Now

                });


            }
            else if (url.Contains("putlocker"))
            {
                var id = GetVideoId(url, VideoSource.Putlocker);
                list.Add(new VideoInfo
                {
                    Source = VideoSource.NosVideo,
                    VideoId = id,
                    Url = String.Format("http://www.putlocker.com/file/{0}", id),
                    Uploader = "ABC_" + id,
                    Title = title,
                    PublishedDate = DateTime.Now
                });
            }
            else if (url.Contains("nowvideo"))
            {
                var id = GetVideoId(url, VideoSource.NowVideo);
                list.Add(new VideoInfo
                {
                    Source = VideoSource.NosVideo,
                    VideoId = GetVideoId(url, VideoSource.NowVideo),
                    Url = String.Format("http://www.nowvideo.eu/video/{0}", id),
                    Uploader = "ABC_" + id,
                    Title = title,
                    PublishedDate = DateTime.Now

                });
            }
            else if (url.Contains("novamov"))
            {
                var id = GetVideoId(url, VideoSource.Novamov);

                list.Add(new VideoInfo
                {
                    Source = VideoSource.Novamov,
                    VideoId = id,
                    Url = String.Format("http://www.novamov.com/video/{0}", id),
                    Uploader = "ABC_" + id,
                    Title = title,
                    PublishedDate = DateTime.Now
                });


            }
            else if (url.Contains("sockshare"))
            {
                var id = GetVideoId(url, VideoSource.Novamov);
                list.Add(new VideoInfo
                {
                    Source = VideoSource.SockShare,
                    VideoId = id,
                    Url = String.Format("http://www.sockshare.com/file/{0}", id),
                    Uploader = "ABC_" + id,
                    Title = title,
                    PublishedDate = DateTime.Now
                });
            }
            else if (url.Contains("veoh")) { }
            else
                Debug.Assert(false);


            return list.Where(x => x != null).ToList();
        }

        private string GetVideoId(string url, VideoSource source)
        {
            switch (source)
            {
                case VideoSource.NowVideo:
                case VideoSource.Youtube:
                case VideoSource.Putlocker:
                case VideoSource.Novamov:
                case VideoSource.SockShare:
                    {
                        var start = url.LastIndexOf("/") + 1;
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
                default:
                    Debug.Assert(false);
                    return String.Empty;
            }
        }
    }
}
