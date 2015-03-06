using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Service;
using IFlixr.Data;
using IFlixr.ViewModel;
using IFlixr.Base;

namespace IFlixr.Controllers
{
    public class HomeController : BaseController
    {
        private readonly FlixrService flixrService;
        public HomeController()
        {
            this.flixrService = new FlixrService();
        }

        public ActionResult Index(string id)
        {
            return View(this.flixrService.GetHomePage());
        }

        public ActionResult HoverBox(int id)
        {

            return View(this.flixrService.GetHoverBox(id));
        }
    }
}
