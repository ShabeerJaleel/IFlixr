using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.WebScraper.Client.Model;
using System.IO;
using IFlixr.Data;
using System.Windows.Forms;
using IFlixr.Base;

namespace IFlixr.WebScraper.Client
{
    public static class MapperExtension
    {
        #region Movie

        public static IFlixr.Data.Movie Map(this Movie movie)
        {
            return new Data.Movie()
            {
                Year = movie.Year.Value,
                CinematographyBy = movie.Cinematography.ToString(),
                Country = (byte)movie.Country.Value,
                DirectedBy = movie.DirectedBy.ToString(),
                DistributedBy = movie.DirectedBy.ToString(),
                EditingBy = movie.EditingBy.ToString(),
                Language = (byte)movie.Language.Value,
                MusicBy = movie.MusicBy.ToString(),
                ProducedBy = movie.ProducedBy.ToString(),
                ReleaseData = movie.ReleaseDate.Value,
                ScreenplayBy = movie.ScreenplayBy.ToString(),
                SongsBy = movie.SongsBy.ToString(),
                Starring = movie.Starring.ToString(),
                Studio = movie.Studio.ToString(),
                Title = movie.Title.ToString(),
                WrittenBy = movie.WrittenBy.ToString(),
                RunningTime = (short)movie.RunningTime.Value,
                Type = (byte)VideoType.Movie,
                UniqueToken = movie.WikiUrl,
                Description = movie.Description.Value
            };
        }

        public static void Map(this Movie movie, IFlixr.Data.Movie dbMovie)
        {
            dbMovie.Year = movie.Year.Value;
            dbMovie.CinematographyBy = movie.Cinematography.ToString();
            dbMovie.Country = (byte)movie.Country.Value;
            dbMovie.DirectedBy = movie.DirectedBy.ToString();
            dbMovie.DistributedBy = movie.DirectedBy.ToString();
            dbMovie.EditingBy = movie.EditingBy.ToString();
            dbMovie.Language = (byte)movie.Language.Value;
            dbMovie.MusicBy = movie.MusicBy.ToString();
            dbMovie.ProducedBy = movie.ProducedBy.ToString();
            dbMovie.ReleaseData = movie.ReleaseDate.Value;
            dbMovie.ScreenplayBy = movie.ScreenplayBy.ToString();
            dbMovie.SongsBy = movie.SongsBy.ToString();
            dbMovie.Starring = movie.Starring.ToString();
            dbMovie.Studio = movie.Studio.ToString();
            dbMovie.Title = movie.Title.ToString();
            dbMovie.WrittenBy = movie.WrittenBy.ToString();
            dbMovie.RunningTime = (short) movie.RunningTime.Value;
            dbMovie.Description = movie.Description.Value;
            
        }


        public static void Map(this Movie movie, Movie otherMovie)
        {
            
            movie.WikiUrl = otherMovie.WikiUrl;
            movie.Cinematography = otherMovie.Cinematography;
            movie.Country = otherMovie.Country;
            movie.DirectedBy = otherMovie.DirectedBy;
            movie.DistributedBy = otherMovie.DirectedBy;
            movie.EditingBy = otherMovie.EditingBy;
            movie.Language = otherMovie.Language;
            movie.MusicBy = otherMovie.MusicBy;
            movie.ProducedBy = otherMovie.ProducedBy;
            movie.ReleaseDate = otherMovie.ReleaseDate;
            movie.ScreenplayBy = otherMovie.ScreenplayBy;
            movie.SongsBy = otherMovie.SongsBy;
            movie.Starring = otherMovie.Starring;
            movie.Studio = otherMovie.Studio;
            movie.Title = otherMovie.Title;
            movie.WrittenBy = otherMovie.WrittenBy;
            movie.RunningTime = otherMovie.RunningTime.Value != 0 ? otherMovie.RunningTime : movie.RunningTime;
            movie.Year = otherMovie.Year;


        }


        public static Movie MapToMovie(this IFlixr.Data.Movie movie)
        {
            return new Movie(new MovieEntity<string>(MovieEntityType.Title, movie.Title),
                new MovieEntity<short>(MovieEntityType.Year,movie.Year), 
                movie.UniqueToken)
            {
                DirectedBy = new MovieEntity<string>(MovieEntityType.DirectedBy, movie.DirectedBy.Split(',')),
                ProducedBy = new MovieEntity<string>(MovieEntityType.ProducedBy, movie.ProducedBy.Split(',')),
                WrittenBy = new MovieEntity<string>(MovieEntityType.WrittenBy, movie.WrittenBy.Split(',')),
                Starring = new MovieEntity<string>(MovieEntityType.Starring, movie.Starring.Split(',')),
                MusicBy = new MovieEntity<string>(MovieEntityType.MusicBy, movie.MusicBy.Split(',')),
                SongsBy = new MovieEntity<string>(MovieEntityType.SongsBy, movie.SongsBy.Split(',')),
                Cinematography = new MovieEntity<string>(MovieEntityType.Cinematography, movie.CinematographyBy.Split(',')),
                EditingBy = new MovieEntity<string>(MovieEntityType.EditingBy, movie.EditingBy.Split(',')),
                Studio = new MovieEntity<string>(MovieEntityType.Studio, movie.Studio.Split(',')),
                DistributedBy = new MovieEntity<string>(MovieEntityType.DistributedBy, movie.DistributedBy.Split(',')),
                ScreenplayBy = new MovieEntity<string>(MovieEntityType.ScreenplayBy, movie.ScreenplayBy.Split(',')),
                
                RunningTime = new MovieEntity<int>(MovieEntityType.Duration, movie.RunningTime),
                ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate, movie.ReleaseData),
                Country = new MovieEntity<Country>(MovieEntityType.Country,(Country)movie.Country),
                Language = new MovieEntity<Language>(MovieEntityType.Language, (Language)movie.Language)
            };
        }

        public static MovieModel Map(this IFlixr.Data.Movie movie)
        {
            var movieModel = new MovieModel()
            {
                Movie = movie.MapToMovie(),
                Videos = movie.VideoLinks.Map(),
                Music = movie.Children.Where(x => ((VideoType)x.Type) == VideoType.Music).ToList().MapMusic(),
                Trailor = movie.Children.Where(x => ((VideoType)x.Type) == VideoType.Trailer).ToList().MapTrailer(),
                Clip = movie.Children.Where(x => ((byte)x.Type) >= (byte)VideoType.Clip).ToList().MapClip(),
                Images = movie.Images.Map(),
                
            };

            return movieModel;
        }

        public static MovieModel Map(this IFlixr.Data.Movie movie, List< IFlixr.Data.Scraper.Video> videos)
        {
            var movieModel = new MovieModel()
            {
                Movie = movie.MapToMovie(),
                Videos = movie.VideoLinks.Map(),
                Music = movie.Children.Where(x => ((VideoType)x.Type) == VideoType.Music).ToList().MapMusic(),
                Trailor = movie.Children.Where(x => ((VideoType)x.Type) == VideoType.Trailer).ToList().MapTrailer(),
                Clip = movie.Children.Where(x => ((byte)x.Type) >= (byte)VideoType.Clip).ToList().MapClip(),
                Images = movie.Images.Map(),
                SearchResults = videos.Map()

            };

            return movieModel;
        }

        #endregion

        #region Image

        public static IFlixr.Data.Image Map(this ImageModel imageModel)
        {
            return new Data.Image()
            {
                FullPath = Path.GetFileName( imageModel.FullPath),
                Height = imageModel.Height,
                Width = imageModel.Width,
                SupportedDimensions = imageModel.SupportedDimensions,
                IsDefault = imageModel.IsDefault
            };
        }

        public static ImageModel Map(this IFlixr.Data.Image dbImage)
        {
            var path = Path.Combine(Application.StartupPath, Helper.RootImagePath, dbImage.FullPath);
            return new ImageModel()
            {
                FullPath = path,
                Height = dbImage.Height,
                Width = dbImage.Width,
                SupportedDimensions = dbImage.SupportedDimensions,
                IsDefault = dbImage.IsDefault
            };
        }

        public static List<ImageModel> Map(this ICollection<IFlixr.Data.Image> dbImages)
        {
            var images = new List<ImageModel>();
            foreach (var dbImage in dbImages)
            {
                images.Add(dbImage.Map());
            }
            return images;
        }

        #endregion

        #region Video

        public static IFlixr.Data.VideoLink Map(this VideoModel videoModel)
        {
            return new Data.VideoLink()
            {
               Url = videoModel.Url,
               UniqueToken = videoModel.VideoId,
               IndexNumber = videoModel.Index,
               Sequence = videoModel.PlaylistSequence,
               Duration = videoModel.Duration,
               PublishedDate = videoModel.PublishedDate,
               Title = videoModel.Title,
               Uploader = videoModel.Uploader,
               Source = (byte)videoModel.Source
            };
        }

        public static IFlixr.Data.Scraper.Video Map(this VideoModel videoModel, string url)
        {
            return new Data.Scraper.Video()
            {
                
                Url = videoModel.Url,
                VideoId = videoModel.VideoId,
                Duration = videoModel.Duration,
                PublishedDate = videoModel.PublishedDate,
                Title = videoModel.Title,
                Uploader = videoModel.Uploader,
                Source = (byte)videoModel.Source,
                Type = (byte)videoModel.Type,
                MovieUrl = url
            };
        }

        public static void Map(this VideoModel videoModel,  IFlixr.Data.VideoLink videoLink)
        {
            videoLink.Url = videoModel.Url;
        }

        public static VideoModel Map(this IFlixr.Data.VideoLink videoLink)
        {
            return new VideoModel()
            {
                Duration = videoLink.Duration,
                Index = videoLink.IndexNumber,
                PublishedDate = videoLink.PublishedDate,
                PlaylistSequence = videoLink.Sequence,
                Title = videoLink.Title,
                Type = (VideoType) videoLink.Show.Type,
                Url = videoLink.Url,
                VideoId = videoLink.UniqueToken,
                Selected = true,
                Uploader = videoLink.Uploader,
                Source = (VideoSource)videoLink.Source
            };
        }

        public static VideoModel Map(this IFlixr.Data.Scraper.Video video)
        {
            return new VideoModel()
            {
                Duration = video.Duration,
                PublishedDate = video.PublishedDate,
                Title = video.Title,
                Type = (VideoType)video.Type,
                Url = video.Url,
                VideoId = video.VideoId,
                Uploader = video.Uploader,
                Source = (VideoSource)video.Source
            };
        }

        public static SortableBindingList<VideoModel> Map(this ICollection<IFlixr.Data.Scraper.Video> video)
        {
            var items = new SortableBindingList<VideoModel>();
            foreach (var x in video)
                items.Add(x.Map());
            return items;
        }

       
        public static SortableBindingList<VideoModel> Map(this ICollection<IFlixr.Data.VideoLink> videoLinks)
        {
            var items = new SortableBindingList<VideoModel>();
            foreach(var x in videoLinks)
                items.Add(x.Map());
            return items;
        }

        public static List<IFlixr.Data.VideoLink> Map(this SortableBindingList<VideoModel> videos)
        {
            var items = new List<IFlixr.Data.VideoLink>();
            foreach (var video in videos)
                items.Add(video.Map());
            return items;
        }

        #endregion

        #region Music

        public static IFlixr.Data.Show Map(this MusicModel musicModel)
        {
            return new Data.Show()
            {
                Title = musicModel.Title,
                Type = (byte)VideoType.Music,
                VideoLinks = musicModel.Videos.Map()
            };
            
        }

        public static List<MusicModel> MapMusic(this ICollection<IFlixr.Data.Show> children)
        {
            return new List<MusicModel>(children.Select(x => x.MapMusic()));
        }

        public static MusicModel MapMusic(this IFlixr.Data.Show show)
        {
            return new MusicModel(show.Title, show.UniqueToken, show.Year)
            {
                Videos = show.VideoLinks.Map()

            };
        }

        #endregion

        #region Trailer
        public static IFlixr.Data.Show Map(this TrailorModel trailerModel)
        {
            return new Data.Show()
            {
                Title = trailerModel.Title,
                Type = (byte)VideoType.Trailer,
                VideoLinks = trailerModel.Videos.Map()
            };
        }

        public static List<TrailorModel > MapTrailer(this ICollection<IFlixr.Data.Show> children)
        {
            return new List<TrailorModel >(children.Select(x => x.MapTrailer()));
        }

        public static TrailorModel MapTrailer(this IFlixr.Data.Show show)
        {
            return new TrailorModel(show.Title, show.UniqueToken, show.Year)
            {
                Videos = show.VideoLinks.Map()

            };
        }

        #endregion

        #region Clip

        public static IFlixr.Data.Show Map(this ClipModel clipModel)
        {
            return new Data.Show()
            {
                Title = clipModel.Title,
                Type = (byte)clipModel.Type,
                VideoLinks = clipModel.Videos.Map()
            };
        }

        public static List<ClipModel> MapClip(this ICollection<IFlixr.Data.Show> children)
        {
            return new List<ClipModel>(children.Select(x => x.MapClip()));
        }

        public static ClipModel MapClip(this IFlixr.Data.Show show)
        {
            return new ClipModel(show.Title, show.UniqueToken, (VideoType) show.Type, show.Year)
            {
                Videos = show.VideoLinks.Map()

            };
        }

        #endregion

       

       
    }
}
