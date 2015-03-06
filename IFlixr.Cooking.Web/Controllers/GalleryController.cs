using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web.Controllers
{
    public class GalleryController : BaseController
    {
        //
        // GET: /Gallery/

        //
        // GET: /Gallery/
        public ActionResult Index(string profileUrl, string sort, string duration)
        {
            var user = Repository.GetUserByProfileUrl(profileUrl);
            RecipePageViewModel model = Repository.GetRecipesByAuthor(user.Id, sort, duration);
            model.Title = "Recipe gallery of " + user.ProfileName;

            
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
