using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper
{
    public static class Creator
    {
        public static Movie CreateMovie(MovieName movieName)
        {
          var name = new MovieEntity<string>(MovieEntityType.Title,
                new MovieEntityItem<string>(movieName.Name, 
                new MovieEntityLink(LinkType.Wiki, movieName.WikiUrl)));
                 
          var year = new MovieEntity<short>(MovieEntityType.Year,
                new MovieEntityItem<short>(movieName.Year, 
                new MovieEntityLink(LinkType.Wiki, movieName.WikiUrl)));

          return new Movie(name, year, movieName.WikiUrl);
        }

        public static MovieEntity<T> CreateMovieEntity<T>(MovieEntityType type)
        {
            return new MovieEntity<T>(type);
        }

       

        public static MovieEntityItem<T> CreateMovieEntityItem<T>(T data, string url, LinkType type)
        {
            return new MovieEntityItem<T>(data, new MovieEntityLink(type, url));
        }

        public static MovieEntityItem<T> CreateMovieEntityItem<T>(T data)
        {
            return new MovieEntityItem<T>(data);
        }
    }
}
