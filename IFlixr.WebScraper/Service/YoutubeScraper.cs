using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.YouTube;
using Google.GData.Client;
using Google.GData.YouTube;
using System.Diagnostics;
using IFlixr.Base;
using System.Net;

namespace IFlixr.WebScraper
{
    public class YouTubeScraper : GoogleVideoScraper
    {
        #region Fields

        private static readonly YouTubeRequestSettings settings = new YouTubeRequestSettings("test", null) { AutoPaging = true, Maximum = 100 };
        
        #endregion


        public List<VideoInfo> GetMovies(QueryParam param)
        {
            var items = GetVideos(param)
                    .Select(x => new VideoInfo()
                    {
                        Duration = Int32.Parse(x.Media.Duration.Seconds),
                        Title = x.Title,
                        Uploader = x.Uploader,
                        PublishedDate = x.AtomEntry.Published,
                        Url = x.WatchPage.AbsoluteUri,
                        VideoId = x.VideoId,
                         Source = VideoSource.Youtube
                    })
                    .Where(x => x.DurationInMinutes > 1 )
                    .OrderByDescending(x => x.DurationInMinutes)
                    .ToList();

            items = items.Where(x => !(x.Title.ToLower()
                .ContainsAnyString(new string[]{"music", "song", "trailer"}) && x.DurationInMinutes < 5)).ToList();

            //var playLists = SearchPlaylists(param.Keyword).ToList(); ;

            //foreach (var item in impItems)
            //{
            //    if (!String.IsNullOrWhiteSpace(item.PlaylistId))
            //        continue;

            //    var playLists = request.GetPlaylistsFeed(item.Uploader);
            //    foreach (var pl in playLists.Entries)
            //    {
            //        var plMembers = request.GetPlaylist(pl);
            //        for (var i = 0; i < plMembers.Entries.Count(); i++)
            //        {
            //            var plMember = plMembers.Entries.ElementAt(i);
            //            var currentItem = items.FirstOrDefault(x => x.VideoId == plMember.Id);
            //            if (currentItem != null)
            //                currentItem.PlaylistId = pl.Id;

            //            currentItem.PlaylistSequence = i;
            //        }

            //    }
            //}

           return items.Where(x => items.Where(y => x.Uploader == y.Uploader).Sum(z => z.DurationInMinutes) > 10).ToList();
        }

        public List<VideoInfo> GetMusic(string fullSearhText, string keyword,  string uploader)
        {
            return GetVideos(new QueryParam() { FullSearhText = fullSearhText, Keyword = keyword, Uploader = uploader })
                    .Select(x => new VideoInfo()
                    {
                        Duration = Int32.Parse(x.Media.Duration.Seconds),
                        Title = x.Title,
                        Uploader = x.Uploader,
                        PublishedDate = x.AtomEntry.Published,
                        Url = x.WatchPage.AbsoluteUri,
                        VideoId = x.VideoId,
                        Source = VideoSource.Youtube
                    })
                    .Where(x => x.DurationInMinutes > 1 && x.DurationInMinutes < 10)
                    .ToList();
        }

        public List<VideoInfo> GetTrailors(string fullSearhText, string keyword,  string uploader)
        {
            return GetVideos(new QueryParam() { FullSearhText = fullSearhText, Keyword = keyword, Uploader = uploader })
                    .Select(x => new VideoInfo()
                    {
                        Duration = Int32.Parse(x.Media.Duration.Seconds),
                        Title = x.Title,
                        Uploader = x.Uploader,
                        PublishedDate = x.AtomEntry.Published,
                        Url = x.WatchPage.AbsoluteUri,
                        VideoId = x.VideoId,
                        Source = VideoSource.Youtube
                    })
                    .Where(x => x.DurationInMinutes < 10)
                    .ToList();
        }

        private IEnumerable<Video> GetVideos(QueryParam param)
        {
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
            query.OrderBy = "relevance";
            query.Query = !String.IsNullOrWhiteSpace(param.Uploader) ?  param.Keyword.ToLower() : param.FullSearhText.ToLower() ;
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
            if (!String.IsNullOrWhiteSpace(param.Uploader))
                query.Author = param.Uploader;


            var request = new YouTubeRequest(settings);
            Feed<Video> videoFeed = request.Get<Video>(query);

            var items = videoFeed.Entries;

            if (param.KeywordTokens.Count > 0)
                items = items.Where(x => x.Title.ToLower().ContainsAnyString(param.KeywordTokens));
            
            
            return items;
        }

        public VideoInfo GetVideoById(string id)
        {
            var request = new YouTubeRequest(settings);
            Feed<Video> videoFeed = request.Get<Video>(new Uri(YouTubeQuery.CreateVideoUri(id)));
            try
            {
                var x = videoFeed.Entries.First();
                return new VideoInfo()
                {
                    Duration = Int32.Parse(x.Media.Duration.Seconds),
                    Title = x.Title,
                    Uploader = x.Uploader,
                    PublishedDate = x.AtomEntry.Published,
                    Url = x.WatchPage.AbsoluteUri,
                    VideoId = x.VideoId,
                    Source = VideoSource.Youtube
                };
                
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException as WebException;
                if (inner == null || inner.Status != WebExceptionStatus.ProtocolError)
                    throw;
            }
            return null;
        }

        public List<VideoInfo> GetVideosByPlaylistId(string id)
        {
            var videos = new List<VideoInfo>();
            try
            {
                var request = new YouTubeRequest(settings);
                Feed<PlayListMember> playlistFeed = request.Get<PlayListMember>(new Uri(String.Format("https://gdata.youtube.com/feeds/api/playlists/{0}?v=2", id)));

                for (var i = 0; i < playlistFeed.Entries.Count(); i++)
                {
                    var x = playlistFeed.Entries.ElementAt(i);
                    if (x.Media.Duration == null)
                        continue;


                    videos.Add(new VideoInfo()
                    {
                        Duration = Int32.Parse(x.Media.Duration.Seconds),
                        Title = x.Title,
                        Uploader = x.Uploader,
                        PublishedDate = x.AtomEntry.Published,
                        Url = x.WatchPage.AbsoluteUri,
                        VideoId = x.VideoId,
                        Source = VideoSource.Youtube
                    });
                }
            }
            catch(Exception ex)
            {
                var inner = ex.InnerException as WebException;
                if (inner == null || inner.Status != WebExceptionStatus.ProtocolError)
                    throw;
            }
            return videos;
        }

        private IEnumerable<Playlist> SearchPlaylists(string search)
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("test", null);
            settings.AutoPaging = true;
            settings.Maximum = 100;

            FeedQuery query = new FeedQuery("https://gdata.youtube.com/feeds/api/playlists/snippets");
            //query.OrderBy = "relevance";
            query.Query = search;
            //query.SafeSearch = YouTubeQuery.SafeSearchValues.None;

            var request = new YouTubeRequest(settings);
            return request.Get<Playlist>(query).Entries;

        }
    }
}
