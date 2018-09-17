using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //https://fullcalendar.io/docs/events-json-feed
            //aca paso la info
            ViewBag.titulo = "Inicio";
            return View();
        }
    }
}