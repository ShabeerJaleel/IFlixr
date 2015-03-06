using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IFlixr.Cooking.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{query}/{language}/{year}/{rating}/{page}",
                defaults: new
                {
                    query = RouteParameter.Optional,
                    language = RouteParameter.Optional,
                    year = RouteParameter.Optional,
                    rating = RouteParameter.Optional,
                    page = RouteParameter.Optional
                }
            );

           

        }
    }
}
