using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class CiudadController : Controller
    {
        public ActionResult Ciudades()
        {
            ViewBag.titulo = "Ciudades";
            ViewBag.ciudades = UtilitiesAndStuff.GetCiudades();
            return View();
        }
    }
}