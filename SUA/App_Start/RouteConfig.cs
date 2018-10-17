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
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Home", action = "Logout" }
            );

            routes.MapRoute(
                name: "getCalenars",
                url: "getCalendars",
                defaults: new { controller = "Home", action = "GetCalendars" }
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



            //============================Show=======================================
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


            //============================sala=======================================
            routes.MapRoute(
                name: "Sala",
                url: "sala",
                defaults: new { controller = "Sala", action = "Sala" }
            );

            routes.MapRoute(
                name: "Salas",
                url: "salas",
                defaults: new { controller = "Sala", action = "Salas" }
            );


            //============================fecha=======================================
            routes.MapRoute(
                name: "Fecha",
                url: "fecha",
                defaults: new { controller = "Fecha", action = "Fecha" }
            );

            routes.MapRoute(
                name: "Fechas",
                url: "fechas",
                defaults: new { controller = "Fecha", action = "Fechas" }
            );

            routes.MapRoute(
                name: "Bordereaux",
                url: "bordereaux",
                defaults: new { controller = "Fecha", action = "Bordereaux" }
            );


            routes.MapRoute(
                name: "FechasCerradas",
                url: "fechas-cerradas",
                defaults: new { controller = "Fecha", action = "FechasCerradas", id = UrlParameter.Optional }
            );


            //============================Usuario========================================
            routes.MapRoute(
                name: "User",
                url: "user",
                defaults: new { controller = "User", action = "Usuario" }
            );

            routes.MapRoute(
                name: "Users",
                url: "users",
                defaults: new { controller = "Users", action = "Usuarios" }
            );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "printBordereaux",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Fecha", action = "PrintBordereaux", id = UrlParameter.Optional }
            );
        }
    }
}
