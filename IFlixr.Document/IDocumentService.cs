using System;
using IFlixr.Data.Model;
using System.Collections.Generic;
using IFlixr.ViewModel;
using IFlixr.Base;
namespace IFlixr.Document
{
    public interface IDocumentService
    {
        //void Add(System.Collections.Generic.IEnumerable<IFlixr.Data.Movie> movies);
        //void AddVideos(System.Collections.Generic.IEnumerable<IFlixr.Data.Show> videos);
        void CreateUser(User user);
        ListEx<ThumbnailMovie> GetAllMovies();
        ListEx<ThumbnailVideo> GetAllVideos();
        Movie GetMovie(int index);
        MoviePage GetMoviePage(int index, string uniqueId="");
        VideoPage GetVideoPage(int index, string uniqueId = "");
        HomePage GetHomePage();

        //Search
        ListEx<ThumbnailMovie> GetMovies(string queryString, List<string> languages,
            int startYear, int endYear, CensorRating rating, int page);

    }
}
