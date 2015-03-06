using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFlixr.Web.Controllers
{
    public class MovieController : BaseController
    {
        // GET: /Movie/Malayalam/2005/1/1/xgsox/casanova

        public ActionResult Index(string language,int year,int id,string uniqueId, string title)
        {
            return View(DocumentService.GetMoviePage(id, uniqueId));
        }

       

    }
}
