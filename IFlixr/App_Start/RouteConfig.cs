using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IFlixr
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
              name: "Browse",
              url: "browse/{action}/{id}",
              defaults: new { controller = "Browse", action = "Movies", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "Movie",
             url: "movie/{language}/{year}/{id}/{title}/",
             defaults: new { controller = "Movie", action="Index" }
         );


            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action="Index", id = UrlParameter.Optional }
          );

           
        }
    }
}