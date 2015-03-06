using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFlixr.Cooking.Web.Controllers
{
    public class BrowseController : BaseController
    {
        //
        // GET: /Browse/All

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Recipe()
        {
            return View(new object { });
        }

    }
}
