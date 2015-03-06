using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web.Controllers
{
    public class ApiController : BaseController
    {
        //
        // GET: /Api/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecipeViewCount(string id)
        {
            try
            {

                return Json(new JsonResponse { Success = true, Data = Repository.IncrementViewCount(id, DateTime.Now.Date) });
            }
            catch
            {
                return Json(new JsonResponse { Success = false, Message = "Unknown" });
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToggleFavourite(string id)
        {
            try
            {
                bool isFavourite;
                var count = Repository.ToggleFavourite(id, AuthenticatedUser.UserId, out isFavourite);
                return Json(new JsonResponse { Success = true, Data = new { IsFavourite = isFavourite, Count = count } });
            }
            catch
            {
                return Json(new JsonResponse { Success = false, Message = "Unknown" });
            }
        }

    }
}
