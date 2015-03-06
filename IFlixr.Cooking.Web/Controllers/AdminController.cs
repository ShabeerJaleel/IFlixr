using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View(new UserManagementModel());
        }

    }
}
