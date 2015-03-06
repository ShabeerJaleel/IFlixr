using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index(string sort, string duration)
        {
            RecipePageViewModel model = Repository.GetRecipes(sort, duration);
           
            //set fav
            if (User.Identity.IsAuthenticated)
            {
                foreach (var recp in model.Recipes)
                    recp.IsFavourite = AuthenticatedUser.User.FavouredRecipes.Contains(recp.Id);
            }
            
           return View(model);
        }

       
       
    }
}
