using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IFlixr.ViewModel;
using IFlixr.Base;

namespace IFlixr.Web.Controllers
{
    public class QueryController : BaseApiController
    {
        [HttpGet]
        [ActionName("Movie")]
        public MovieBrowsePage Movie(string query = "", string language = "",
            string year = "", CensorRating rating = CensorRating.General, int page = 2)
        {
            language = language == null ? String.Empty : language;
            query = query == null ? String.Empty : query;

            var langs = new List<string>(language.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            var startYear = 0;
            var endYear = 0;
            if (!String.IsNullOrWhiteSpace(year))
            {
                var years = new List<int>();
                foreach (var token in year.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    years.Add(Convert.ToInt32(token));
                startYear = years.OrderBy(x => x).First();
                endYear = years.OrderByDescending(x => x).First();
                if (endYear < 2010)
                    endYear = endYear + 9;

            }

            return new MovieBrowsePage { Movies = DocumentService.GetMovies(query, langs, startYear, endYear, rating, page) };
           
        }
    }
}
