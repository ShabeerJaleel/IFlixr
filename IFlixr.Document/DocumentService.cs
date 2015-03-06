using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Database.Server;
using System.Diagnostics;
using Raven.Client.Document;
using IFlixr.ViewModel;
using IFlixr.Data.Model;
using IFlixr.Base;
using Raven.Client;

namespace IFlixr.Document
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentStore store;

        public DocumentService()
        {
            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
            //store = new EmbeddableDocumentStore
            //{
            //    ConnectionStringName = "RavenDB",
            //    UseEmbeddedHttpServer = true,


            //};
            //store.Configuration.HostName = "localhost";
            //store.Configuration.Port = 8080;

            store = new DocumentStore
            {
                ConnectionStringName = "RavenDB",
            };
            store.Conventions.SaveEnumsAsIntegers = true;
            store.Initialize();
        }


        public ListEx<ThumbnailMovie> GetAllMovies()
        {
            using (var session = store.OpenSession("iFlixr"))
            {
                var results = new List<ThumbnailMovie>();
                session.Query<Movie>()
                    .Customize(x => x.Include <Movie> (o => o.PostedById))
                    .Where(x => x.Images.Count > 0 && x.Links.Count > 0)
                    .ToList().ForEach(x => {
                                    results.Add(
                                        new ThumbnailMovie
                                        {
                                            Id = x.Id,
                                            Description = x.ShortDescription,
                                            ImageUrl = x.GenerateImageUrl(),
                                            PostedBy = session.Load<User>(x.PostedById).Name,
                                            PostedDate = x.CreatedDate,
                                            ReleasedDate = x.ReleaseDate,
                                            UniqueId = x.Links.SelectMany(y => y.Parts).First().UniqueId,
                                            Title = x.Title,
                                            DescriptionUrl = x.DescriptionUrl,
                                            Languages = x.Languages
                                        }
                                    );
                                });

                return new ListEx<ThumbnailMovie>(results);
            }
        }


        public ListEx<ThumbnailMovie> GetMovies(string queryString, List<string> languages,
            int startYear, int endYear, CensorRating rating, int page)
        {
            var skip = (page - 1) * GlobalSettings.PageSizeInBrowseMovie;
            using (var session = store.OpenSession("iFlixr"))
            {
                var results = new ListEx<ThumbnailMovie>();
                var query = session.Query<Movie>()
                    .Customize(x => x.Include<Movie>(o => o.PostedById))
                    .Select(x => x);


                if (!String.IsNullOrWhiteSpace(queryString))
                    query = query.Where<Movie>(x => x.Title.StartsWith(queryString));

                if (startYear != 0 && endYear != 0)
                {
                    var start = new DateTime(startYear, 1, 1, 0, 0, 0);
                    var end = new DateTime(endYear, 12, 31, 23, 59, 59);
                    query = query.Where<Movie>(x => x.ReleaseDate >= start && x.ReleaseDate <= end);
                }

                
                foreach(var lang in languages)
                    query = query.Where<Movie>(x => x.Languages.Contains(lang));
               
                
                if (rating != CensorRating.Any)
                   query = query.Where<Movie>(x => x.Rating == rating);

                query.Where(x => x.Images.Count > 0 && x.Links.Count > 0)
                    .Skip(skip)
                    .Take(GlobalSettings.PageSizeInBrowseMovie)
                    .ToList().ForEach(x =>
                    {
                        results.Add(
                            new ThumbnailMovie
                            {
                                Id = x.Id,
                                Description = x.ShortDescription,
                                ImageUrl = x.GenerateImageUrl(),
                                PostedBy = session.Load<User>(x.PostedById).Name,
                                PostedDate = x.CreatedDate,
                                ReleasedDate = x.ReleaseDate,
                                UniqueId = x.Links.SelectMany(y => y.Parts).First().UniqueId,
                                Title = x.Title,
                                DescriptionUrl = x.DescriptionUrl,
                                Languages = x.Languages
                            }
                        );
                    });
                return results;
            }
        }

        public MoviePage GetMoviePage(int index, string uniqueId="")
        {
            var movies = GetAllMovies();
            var movie = GetMovie(index);
            return new MoviePage { Movie = movie,
                StartVideoId = GetUniqueId(movie,uniqueId),
                Related = new List<MovieCarousel>{  
                    new  MovieCarousel{ Title = "You Might Also Like", Movies = movies },
                    new  MovieCarousel{ Title = "Newly Released", Movies =  movies},
                    new  MovieCarousel{ Title = "Newly Released", Movies =  movies }
                }
            };
        }

        public VideoPage GetVideoPage(int index, string uniqueId = "")
        {
            var videos = GetAllVideos();
            var video = GetVideo(index);
            return new VideoPage
            {
                Video = video,
                Related = new List<VideoCarousel>{  
                    new  VideoCarousel{ Title = "You Might Also Like", Videos = videos },
                    new  VideoCarousel{ Title = "Newly Released", Videos =  videos},
                    new  VideoCarousel{ Title = "Newly Released", Videos =  videos }
                }
            };
        }

        private string GetUniqueId(Movie movie, string uniqueId)
        {
            var part = movie.Links
                .SelectMany(x => x.Parts.Where(y => y.UniqueId == uniqueId))
                .FirstOrDefault();
            return (part != null ? part.UniqueId : movie.Links[0].Parts[0].UniqueId);
        }

        public Movie GetMovie(int index)
        {
            
            using (var session = store.OpenSession("iFlixr"))
            {
                var movie = session
                            .Include<Movie>(x => x.PostedById)
                            .Load("movies/" + index);
                movie.PostedByName = session.Load<User>(movie.PostedById).Name;
                foreach (var vid in movie.Links)
                    vid.PostedByName = movie.PostedByName;
                return movie;
            }
        }

        public Video GetVideo(int index)
        {

            using (var session = store.OpenSession("iFlixr"))
            {
                var video = session
                            .Include<Video>(x => x.PostedById)
                            .Load("videos/" + index);
                video.PostedByName = session.Load<User>(video.PostedById).Name;
                foreach (var vid in video.Links)
                    vid.PostedByName = video.PostedByName;
                return video;
            }
        }

        public ListEx<ThumbnailVideo> GetAllVideos()
        {
            using (var session = store.OpenSession("iFlixr"))
            {
                var results = new List<ThumbnailVideo>();
                session.Advanced.LuceneQuery<Video>()
                    .Include(x => x.PostedById)
                    .ToList().ForEach(x =>
                    {
                        results.Add(
                            new ThumbnailVideo
                            {
                                Id = x.Id,
                                UniqueId = x.Links.SelectMany(y => y.Parts).First().UniqueId,
                                Description = x.ShortDescription,
                                ImageUrl = x.GenerateImageUrl(),
                                PostedBy = session.Load<User>(x.PostedById).Name,
                                PostedDate = x.CreatedDate,
                                Types = x.Types,
                                Tags = x.Tags,
                                Title = x.Title,
                                Languages = x.Languages
                            }
                        );
                    });

                return new ListEx<ThumbnailVideo>(results);
            }
        }

        public void CreateUser(User user)
        {
            using (var session = store.OpenSession("iFlixr"))
            {
                session.Store(user);
                session.SaveChanges();
            }
        }

        public HomePage GetHomePage()
        {
            var movies = GetAllMovies().Take(8).ToList();
            var videos = GetAllVideos().Take(8).ToList();
            return new HomePage
            {
                MovieCarousels = new List<MovieCarousel>{  
                    new  MovieCarousel{ Title = "You Might Also Like", Movies = movies },
                    new  MovieCarousel{ Title = "Newly Released", Movies =  movies},
                },
                VideoCarousels = new List<VideoCarousel>{  
                    new  VideoCarousel{ Title = "You Might Also Like", Videos = videos, DisplayTitle = false },
                },

            };
        }


       


    }
}
