using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SUA
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "inicio",
                defaults: new { controller = "Home", action = "Index" }
            );


            //============================standupero=================================
            routes.MapRoute(
                name: "Standupero",
                url: "standupero",
                defaults: new { controller = "Standupero", action = "Standupero" }
            );

            routes.MapRoute(
                name: "Standuperos",
                url: "standuperos",
                defaults: new { controller = "Standupero", action = "Standuperos" }
            );



            //============================produtor=======================================
            routes.MapRoute(
                name: "Productor",
                url: "productor",
                defaults: new { controller = "Productor", action = "Productor" }
            );

            routes.MapRoute(
                name: "Productores",
                url: "productores",
                defaults: new { controller = "Productor", action = "Productores" }
            );



            //============================produtor=======================================
            routes.MapRoute(
                name: "Show",
                url: "show",
                defaults: new { controller = "Show", action = "Show" }
            );

            routes.MapRoute(
                name: "Shows",
                url: "shows",
                defaults: new { controller = "Show", action = "Shows" }
            );






            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
