﻿using System;
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

            routes.MapRoute(
                name: "Standupero",
                url: "standupero",
                defaults: new { controller = "Home", action = "Standupero" }
            );

            routes.MapRoute(
                name: "Standuperos",
                url: "standuperos",
                defaults: new { controller = "Home", action = "Standuperos" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
