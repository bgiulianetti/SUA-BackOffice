using SUA.Models;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class VotarCiudadController : Controller
    {
        [HttpGet]
        public ActionResult Votar(string id)
        {
            ViewBag.mensaje = "";
            ViewBag.ciudades = GetCiudadesParaMostrar();
            var show = id.ToLower().Trim();
            if (show == "sanata")
            {
                ViewBag.image = "sanata.png";
                ViewBag.show = "Sanata Stand Up";
            }
            else if (show == "nicolasdetracy")
            {
                ViewBag.image = "nicolasdetracy.png";
                ViewBag.show = "Nicolas de Tracy";
            }
            else if (show == "lailaygonzo")
            {
                ViewBag.image = "lailaygonzo.png";
                ViewBag.show = "Laila y Gonzo";
            }
            else if (show == "elinnombrable")
            {
                ViewBag.image = "elinnombrable.png";
                ViewBag.show = "El Innombrable";
            }
            else
            {
                ViewBag.mensaje = "show_inválido";
                ViewBag.show = "";
                ViewBag.image = "";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Votar(Votacion votacion, string ip)
        {
            return View();
        }

        private List<string> GetCiudadesParaMostrar()
        {
            var ciudades = new List<string>();
            var ciudadesFull = UtilitiesAndStuff.GetCiudades();
            foreach (var item in ciudadesFull)
            {
                ciudades.Add(item.Nombre + " - " + item.Pais);
            }
            return ciudades;
        }

    }
}