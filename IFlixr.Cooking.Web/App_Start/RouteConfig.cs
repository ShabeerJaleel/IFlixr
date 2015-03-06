using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IFlixr.Cooking.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //    name: "Default1",
        //    url: "Gallery/{action}/{profileUrl}",
        //    defaults: new {controller="Gallery", action="Index", profileUrl = UrlParameter.Optional }
        //);
            routes.MapRoute(
           name: "Gallery",
           url: "Gallery/{profileUrl}/",
           defaults: new { controller = "Gallery", action = "Index", profileUrl = UrlParameter.Optional }
       );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}",
              defaults: new { controller = "Home", action = "Index" }
          );

          


           
        }
    }
}