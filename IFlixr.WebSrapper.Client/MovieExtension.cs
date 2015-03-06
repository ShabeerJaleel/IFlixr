using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.WebScraper.Client.Model;
using IFlixr.Data;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using System.IO;
using System.Windows.Forms;
using IFlixr.Data.Scraper;
using IFlixr.Base;

namespace IFlixr.WebScraper.Client
{
    public static class MovieExtension
    {
        public static void Validate(this MovieModel movieModel)
        {
            //an index with  sequenceindex  0 should not have any other number sequenceindex
            var singleLinkItems = movieModel.Videos.Where(x => x.PlaylistSequence == 0);
            if(singleLinkItems.Where(x => movieModel.Videos.Where(y => y.Index == x.Index).Count() > 1).Any())
                throw new Exception("Validation failed. An index with  sequenceindex  0 " +
                    "should not have any other number sequenceindex. Movie: " + movieModel.Title);
            //an index should not have repeating sequence index
            if (movieModel.Videos.Where(x => movieModel.Videos
                .Where(y => (x.Index == y.Index) && (x.PlaylistSequence == y.PlaylistSequence))
                .Count() > 1).Any())
                throw new Exception("Validation failed. An index should not have repeating sequence index. Movie: " + movieModel.Title);

            if(movieModel.Clip.Any(x => (byte)x.Type < (byte)VideoType.Clip))
                throw new Exception("Clip should have proper video type. Movie: " + movieModel.Title);

            if (movieModel.Images.Count > 0 && movieModel.Images.Where(x => x.IsDefault).Count() != 1)
                throw new Exception("There should be exactly one default image. Movie: " + movieModel.Title);
           
        }

        public static void Save(this MovieModel movieModel)
        {

           
            movieModel.Validate();
            using (var context = new IFlixrContext())
            {

                //Movie & video
                var dbMovie = context.Movies.Where(x => movieModel.Movie.WikiUrl.Equals
                    (x.UniqueToken, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                if (dbMovie == null)
                {
                    dbMovie = movieModel.Movie.Map();
                    context.Movies.Add(dbMovie);
                }
                else
                    movieModel.Movie.Map(dbMovie);

                DeleteVideos(dbMovie.VideoLinks.ToList(), context);

                movieModel.Videos.Each(x => dbMovie.VideoLinks.Add(x.Map()));
                context.SaveChanges();

                DeleteChildren(dbMovie, context);

                //images
                movieModel.Images.Each(x =>
                {
                    var dbImage = x.Map();
                    var destPath = Path.Combine(Application.StartupPath, Helper.RootImagePath, dbImage.FullPath);
                    if (File.Exists(x.FullPath))
                    {
                        if (!File.Exists(destPath))
                            File.Copy(x.FullPath, destPath);
                        dbMovie.Images.Add(dbImage);
                    }
                    else
                        throw new Exception("Image does not exists: " + x.FullPath);
                });

                context.SaveChanges();

                //Music & video
                movieModel.Music.Each(x => dbMovie.Children.Add(x.Map()));

                context.SaveChanges();

                //Trailer & video
                movieModel.Trailor.Each(x => dbMovie.Children.Add(x.Map()));

                //Clip & video
                movieModel.Clip.Each(x => dbMovie.Children.Add(x.Map()));


                context.SaveChanges();
            }

            movieModel.SearchResults.Save(movieModel.Movie.WikiUrl);

           

        }

        public static void Save(this SortableBindingList<VideoModel> videos, string movieUrl)
        {
            using (var scraperContext = new ScraperContext())
            {
                //DeleteVideos(scraperContext.Videos.Where(x => x.MovieUrl == movieUrl).ToList(),
                //    scraperContext);
                videos.Each(x =>
                {
                    if(!scraperContext.Videos.Any(y => x.VideoId == y.VideoId &&
                        x.Type == (VideoType)y.Type))
                        scraperContext.Videos.Add(x.Map(movieUrl));
                });
                scraperContext.SaveChanges();
            }
        }

        private static void DeleteVideos(IEnumerable<VideoLink> videos, IFlixrContext context)
        {
            var objContext = (context as IObjectContextAdapter).ObjectContext;
            foreach (var video in videos)
                objContext.DeleteObject(video);
            context.SaveChanges();
        }

        private static void DeleteVideos(IEnumerable<Video> videos, ScraperContext context)
        {
            var objContext = (context as IObjectContextAdapter).ObjectContext;
            foreach (var video in videos)
                objContext.DeleteObject(video);
            context.SaveChanges();
        }

        private static void DeleteChildren(Show show, IFlixrContext context)
        {
            var objContext = (context as IObjectContextAdapter).ObjectContext;
            var children = show.Children.ToList();
            foreach (var child in children)
            {
                DeleteVideos(child.VideoLinks.ToList(), context);
                objContext.DeleteObject(child);
            }

            var images = show.Images.ToList();
            foreach (var img in images)
            {
                objContext.DeleteObject(img);
            }

            context.SaveChanges();
        }

    }
}
