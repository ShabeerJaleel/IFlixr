using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IFlixr.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           
            routes.MapRoute(
              name: "Video",
              url: "video/{id}/{uniqueId}/{title}/",
              defaults: new { controller = "Video", action = "Index" }
          );

            routes.MapRoute(
               name: "Movie",
               url: "movie/{language}/{year}/{id}/{uniqueId}/{title}/",
               defaults: new { controller = "Movie", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}