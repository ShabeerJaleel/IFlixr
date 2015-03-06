using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Service;

namespace IFlixr.Controllers
{
    public class MovieController : BaseController
    {
        private readonly FlixrService flixrService;
        public MovieController()
        {
            this.flixrService = new FlixrService();
        }


        //
        // GET: /Movie/Malayalam/2005/1/casanova

        public ActionResult Index(string language,int year,int id, string title)
        {
            return View(this.flixrService.GetMoviePage(id) );
        }

       

    }
}
