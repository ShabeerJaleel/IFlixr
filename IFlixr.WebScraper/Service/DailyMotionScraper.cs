using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using Newtonsoft.Json;
using IFlixr.Base;
using System.Diagnostics;
using Google.GData.Client;

namespace IFlixr.WebScraper
{
    public class DailyMotionScraper : GoogleVideoScraper
    {
        private static readonly string SearchUrl = "https://api.dailymotion.com/videos?fields=duration,id%2Cowner.created_time%2Cowner.username%2Ctitle%2Curl&sort=relevance&limit=100{0}{1}";
        private static readonly string IDSearchUrl = "https://api.dailymotion.com/videos?fields=duration%2Cid%2Cowner.created_time%2Cowner.username%2Ctitle%2Curl&ids={0}";
        private static readonly string PlaylistUrl = "https://api.dailymotion.com/video/{0}/playlists";
        private static readonly string VideoByPlaylistUrl = "https://api.dailymotion.com/playlist/{0}/videos?fields=duration,id%2Cowner.created_time%2Cowner.username%2Ctitle%2Curl&limit=100";
        
        private static readonly string SiteUrl = "dailymotion.com";

        public override List<VideoInfo> Search(QueryParam param)
        {
            List<VideoInfo> videos = new List<VideoInfo>();
            if (!param.FilterByUploader)
            {
                param.Duration = VideoDuration.Long;
                var gVideos = base.Search(param, SiteUrl);
                gVideos.ForEach(x => videos.AddRange(Search(String.Format(IDSearchUrl, x.VideoId))));

                param.Duration = VideoDuration.Medium;
                gVideos = base.Search(param, SiteUrl);
                gVideos.ForEach(x =>
                {
                    if (!videos.Any(y => y.VideoId == x.VideoId))
                        videos.AddRange(Search(String.Format(IDSearchUrl, x.VideoId)));
                });
            }

            var searchText = param.FullSearhText.Trim().Replace(" ", "+").Replace("&", "%26"); ;
            var url = String.Format(SearchUrl,
                !String.IsNullOrWhiteSpace(searchText) ? "&search=" + searchText : String.Empty,
                param.FilterByUploader ? "&owner=" + param.Uploader : String.Empty);

            var dmVideos = Search(url);
            dmVideos.ForEach(x =>
            {
                if (!videos.Any(y => y.VideoId == x.VideoId))
                    videos.Add(x);
            });

            videos = videos.Where(x => x.Title.ContainsAnyString(param.KeywordTokens))
                         .ToList();
            var allVideos = videos.ToList();
            
            videos.ForEach(x =>
            {
                if (String.IsNullOrWhiteSpace(x.PlaylistId))
                {
                    var pid = GetPlaylistId(x.VideoId);
                    if (pid == "err")
                    {
                        allVideos.Remove(x);
                    }
                    else if (!String.IsNullOrWhiteSpace(pid))
                    {
                       x.PlaylistId = pid;
                       var relatedVideos =  Search(String.Format(VideoByPlaylistUrl, pid));
                       var duration = relatedVideos.Sum(sm => sm.DurationInMinutes);
                       if (duration > 60 && duration < 210 && relatedVideos.Count < 20)
                       {
                           for (var i = 0; i < relatedVideos.Count; i++)
                           {
                               var rvideo = relatedVideos[i];
                               rvideo.PlaylistId = pid;
                               
                               var video = allVideos.FirstOrDefault(y => y.VideoId == rvideo.VideoId);
                               if (video != null)
                                   video.PlaylistId = rvideo.PlaylistId;
                               else
                               {
                                   video = rvideo;
                                   allVideos.Add(rvideo);
                               }
                               video.PlaylistSequence = i + 1;
                           }
                       }
                       else
                       {
                           allVideos.Remove(x);
                       }
                    }
                }
            });

            return allVideos;

        }

        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }


        protected override string GetId(string url)
        {
            var start = url.IndexOf("video/") + 6;
            var end = url.IndexOf("_", start);
            return url.Substring(start, end - start);
        }

        protected override string GetUserName(string url)
        {
            return "DM_User";
        }
       
        protected override VideoSource GetSource(string url)
        {
            return VideoSource.DailyMotion;
        }

        private List<VideoInfo> Search(string url)
        {
            var videos = new List<VideoInfo>();
            var data = DownloadJson(url);
            for (int i = 0; i < data.list.Count; i++)
            {

                dynamic item = (dynamic)data.list[i];
                //string title = ((string)(((dynamic)item).title)).ToLowerInvariant();
                string title = item.title.ToString();
                if(videos.Any(x => x.VideoId == item.id.ToString()))
                    continue;

                videos.Add(new VideoInfo()
                {
                    Duration = item.duration,
                    PublishedDate = FromUnixTime((long)item["owner.created_time"].Value),
                    Source = VideoSource.DailyMotion,
                    Title = item.title,
                    Uploader = item["owner.username"],
                    Url = item.url,
                    VideoId = item.id

                });
            }

            return videos;
        }

        private dynamic DownloadJson(string url)
        {
            using(var client = new WebClient())
            {
                return JsonConvert.DeserializeObject<dynamic>(client.DownloadString(url));
            }
        }

        private string GetPlaylistId(string videoId)
        {
            try
            {
                var data = DownloadJson(String.Format(PlaylistUrl, videoId));
                if (data.list.Count > 0)
                    return data.list[0].id;
            }
            catch { return "err"; }
            return String.Empty;
        }

        public List<VideoInfo> GetVideosByPlaylistId(string id)
        {
            var relatedVideos = Search(String.Format(VideoByPlaylistUrl, id));
            for (var i = 0; i < relatedVideos.Count; i++)
            {
                relatedVideos[i].PlaylistId = id;
                relatedVideos[i].PlaylistSequence = i +1;
            }

            return relatedVideos;
        }
    }

   
    
}
