using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using IFlixr.Data.Model;

namespace IFlixr.ViewModel
{
    public class MoviePage
    {
        public MoviePage()
        {
            Related = new List<MovieCarousel>();
        }
        public Movie Movie { get; set; }
        public List<MovieCarousel> Related { get; set; }
        public string StartVideoId { get; set; }
        public bool IsSelectedVideo(string uniqueId)
        {
            if (StartVideoId == uniqueId)
                return true;
            foreach (var link in Movie.Links)
            {
                if (link.Parts.Any(x => x.UniqueId == StartVideoId))
                    continue;
                if (uniqueId == link.Parts[0].UniqueId)
                    return true;
            }
            return false;
        }

        public string GetUrl(string uniqueId)
        {
            return Movie.GenerateVideoUrl(uniqueId);
        }

        public VideoLink ActiveLink
        {
            get
            {
                return Movie.Links.First(x => x.Parts.Any(y => y.UniqueId == StartVideoId));
            }
        }
        
    }

}
