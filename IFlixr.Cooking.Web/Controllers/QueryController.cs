using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web.Controllers
{
    public class QueryController : BaseApiController
    {
        [HttpGet]
        [ActionName("Recipe")]
        public RecipeQueryViewModel Recipe(string query = "", int page = 2)
        {
            query = query == null ? String.Empty : query;
            return  Repository.Query(query, 20);
        }

      
    }
}
