using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFlixr.Web.Controllers
{
    public class VideoController : BaseController
    {
        // GET: /Video/1/xvrgs/casanova

        public ActionResult Index(int id,string uniqueId, string title)
        {
            return View(DocumentService.GetVideoPage(id, uniqueId));
        }

       

    }
}
