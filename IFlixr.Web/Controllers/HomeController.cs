using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Document;
using IFlixr.ViewModel;

namespace IFlixr.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(DocumentService.GetHomePage());
        }


        public ActionResult BrowseVideo()
        {
            return View(DocumentService.GetAllVideos());
        }

    }
}
