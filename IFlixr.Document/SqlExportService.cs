using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using System.Diagnostics;

namespace IFlixr.Document
{
    public class SqlExportService
    {
          private readonly DocumentStore store;

          public SqlExportService()
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
        #region Commented
        public void AddVideos(IEnumerable<IFlixr.Data.Show> videos)
        {
            var rVideos = new List<IFlixr.Data.Model.Video>();
            foreach (var video in videos)
            {
                var rVideo = new IFlixr.Data.Model.Video
                {
                    Country = IFlixr.Base.Country.India,
                    CreatedDate = DateTime.Now,
                    Description = video.Description,
                    Duration = 0,
                    Languages = new List<string> { "ml" },
                    Tags = new List<string> { "malayalam" },
                    Title = video.Title,
                    Types = new List<IFlixr.Base.VideoType> { (IFlixr.Base.VideoType)video.Type },
                    DescriptionUrl = video.UniqueToken,
                    PostedById = "users/1"
                };
                rVideos.Add(rVideo);

                var lastIndex = -1;
                IFlixr.Data.Model.VideoLink link = null;
                foreach (var l in video.VideoLinks.OrderBy(x => x.IndexNumber).ThenBy(x => x.Sequence))
                {
                    if (lastIndex != l.IndexNumber)
                    {
                        link = new IFlixr.Data.Model.VideoLink
                        {
                            CreatedDate = DateTime.Now,
                            Index = l.IndexNumber,
                            PostedById = "users/1",
                            Source = (IFlixr.Base.VideoSource)l.Source,
                            PrimaryUrl = l.Url
                        };
                        rVideo.Links.Add(link);
                    }

                    link.Parts.Add(new IFlixr.Data.Model.VideoPart
                    {
                        Duration = l.Duration,
                        Sequence = l.Sequence,
                        Title = l.Title,
                        Url = l.Url,
                        UniqueId = l.UniqueToken
                    });
                    lastIndex = 0;
                }

                foreach (var i in video.Images)
                {
                    rVideo.Images.Add(new IFlixr.Data.Model.VideoImage
                    {
                        FileName = i.FullPath,
                        Height = i.Height,
                        IsDefault = i.IsDefault,
                        SupportedDimensions = (IFlixr.Base.ImageDimension)i.SupportedDimensions,
                        Width = i.Width,
                    });
                }


                try
                {
                    foreach (var rM in rVideos)
                    {
                        using (var session = store.OpenSession("iFlixr"))
                        {
                            session.Store(rM);
                            session.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public void Add(IEnumerable<IFlixr.Data.Movie> movies)
        {

            var rMovies = new List<IFlixr.Data.Model.Movie>();
            foreach (var movie in movies)
            {
                var rMovie = new IFlixr.Data.Model.Movie
                {
                    CinematographyBy = movie.CinematographyBy,
                    Country = IFlixr.Base.Country.India,
                    CreatedDate = DateTime.Now,
                    Description = movie.Description,
                    DirectedBy = movie.DirectedBy,
                    DistributedBy = movie.DistributedBy,
                    Duration = movie.RunningTime,
                    EditingBy = movie.EditingBy,
                    Languages = new List<string>{ IFlixr.Base.KnownLanguages.Malayalam},
                    MusicBy = movie.MusicBy,
                    ProducedBy = movie.ProducedBy,
                    ReleaseDate = movie.ReleaseData,
                    ScreenplayBy = movie.ScreenplayBy,
                    SongsBy = movie.SongsBy,
                    Starring = movie.Starring,
                    Studio = movie.Studio,
                    Tags = new List<string> { "malayalam" },
                    Title = movie.Title,
                    Types = new List<IFlixr.Base.VideoType> { (IFlixr.Base.VideoType)movie.Type },
                    WrittenBy = movie.WrittenBy,
                    DescriptionUrl = movie.UniqueToken,
                    PostedById = "users/1"

                };
                rMovies.Add(rMovie);

                var lastIndex = 0;
                IFlixr.Data.Model.VideoLink link = null;
                foreach (var l in movie.VideoLinks.OrderBy(x => x.IndexNumber).ThenBy(x => x.Sequence))
                {
                    if (lastIndex != l.IndexNumber)
                    {
                        link = new IFlixr.Data.Model.VideoLink
                        {
                            CreatedDate = DateTime.Now,
                            Index = l.IndexNumber,
                            PostedById = "users/1",
                            Source = (IFlixr.Base.VideoSource)l.Source,
                            PrimaryUrl = l.Url
                        };
                        rMovie.Links.Add(link);
                    }

                    link.Parts.Add(new IFlixr.Data.Model.VideoPart
                    {
                        Duration = l.Duration,
                        Sequence = l.Sequence,
                        Title = l.Title,
                        Url = l.Url,
                        UniqueId = l.UniqueToken
                    });
                    lastIndex = l.IndexNumber;
                }

                foreach (var i in movie.Images)
                {
                    rMovie.Images.Add(new IFlixr.Data.Model.VideoImage
                    {
                        FileName = i.FullPath,
                        Height = i.Height,
                        IsDefault = i.IsDefault,
                        SupportedDimensions = (IFlixr.Base.ImageDimension)i.SupportedDimensions,
                        Width = i.Width,
                    });
                }


                try
                {
                    foreach (var rM in rMovies)
                    {
                        using (var session = store.OpenSession("iFlixr"))
                        {
                            session.Store(rM);
                            session.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        #endregion
    }
}
