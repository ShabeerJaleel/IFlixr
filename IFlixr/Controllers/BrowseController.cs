using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Service;

namespace IFlixr.Controllers
{
    public class BrowseController : BaseController
    {
        private readonly FlixrService flixrService;
        public BrowseController()
        {
            this.flixrService = new FlixrService();
        }


        //
        // GET: /Movie/

        public ActionResult Movies(string id)
        {
            return View(this.flixrService.GetMovieBrowsePage(id) );
        }

    }
}
