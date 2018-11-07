using Newtonsoft.Json;
using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class VotarController : Controller
    {
        [HttpGet]
        public ActionResult Votar(string id)
        {
            ViewBag.mensaje = "";
            ViewBag.ciudades = GetCiudadesParaMostrar();
            var show = id.ToLower().Trim();
            ViewBag.fileNameShow = show;
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
                ViewBag.mensaje = "Esta url no existe :(";
                ViewBag.show = "";
                ViewBag.image = "";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Votar(Votacion votacion, string ciudad, string fileNameShow)
        {
            //seteo ViewBags
            ViewBag.ciudades = GetCiudadesParaMostrar();
            ViewBag.fileNameShow = fileNameShow;
            if (fileNameShow == "sanata")
            {
                ViewBag.image = "sanata.png";
                ViewBag.show = "Sanata Stand Up";
            }
            else if (fileNameShow == "nicolasdetracy")
            {
                ViewBag.image = "nicolasdetracy.png";
                ViewBag.show = "Nicolas de Tracy";
            }
            else if (fileNameShow == "lailaygonzo")
            {
                ViewBag.image = "lailaygonzo.png";
                ViewBag.show = "Laila y Gonzo";
            }
            else if (fileNameShow == "elinnombrable")
            {
                ViewBag.image = "elinnombrable.png";
                ViewBag.show = "El Innombrable";
            }
            else
            {
                ViewBag.mensaje = "Esta url no existe :(";
                ViewBag.show = "";
                ViewBag.image = "";
                return View();
            }


            var ciudadObtenida = GetCiudadByName(ciudad.Split('-')[0].Trim());
            if (ciudadObtenida == null)
            {
                ViewBag.mensaje = "No pudimos reconocer tu ciudad";
                return View();
            }
            var service = new VotacionService();
            try
            {
                votacion.Ciudad = ciudadObtenida;
                votacion.Fecha = DateTime.Now;
                service.AddVotacion(votacion);
                ViewBag.mensaje = "creado";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Votaciones()
        {

            return View();
        }

        private List<string> GetCiudadesParaMostrar()
        {
            var ciudades = new List<string>();
            var ciudadesFull = UtilitiesAndStuff.GetCiudades();
            foreach (var item in ciudadesFull)
            {
                if (item.Pais == "Argentina")
                    ciudades.Add(item.Nombre + " - " + item.Estado);
                else
                    ciudades.Add(item.Nombre + " - " + item.Pais);
            }
            return ciudades;
        }


        public Ciudad GetCiudadByName(string name)
        {
            Ciudad ciudad = null;
            var ciudades = UtilitiesAndStuff.GetCiudades();
            foreach (var item in ciudades)
            {
                if (item.Nombre == name)
                {
                    ciudad = item;
                    break;
                }

            }
            return ciudad;
        }
    }
}