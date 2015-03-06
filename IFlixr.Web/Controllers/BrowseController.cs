using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.ViewModel;
using IFlixr.Base;

namespace IFlixr.Web.Controllers
{
    public class BrowseController : BaseController
    {
        //
        // GET: /Browse/All

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Movie()
        {
            return View(new MovieBrowsePage { Movies = 
                DocumentService.GetMovies(String.Empty, 
                new List<string>{"ml"}, 
                GlobalSettings.InitialYearInBrowseMovie,
                GlobalSettings.InitialYearInBrowseMovie, CensorRating.General, 1)
            });
        }

    }
}
