using Newtonsoft.Json;
using SUA.Filters;
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
                ViewBag.color = "#1cadc3";
            }
            else if (show == "nicolasdetracy")
            {
                ViewBag.image = "nicolasdetracy.png";
                ViewBag.show = "Nicolas de Tracy";
                ViewBag.color = "brown";
            }
            else if (show == "lailaygonzo")
            {
                ViewBag.image = "lailaygonzo.png";
                ViewBag.show = "Laila y Gonzo";
                ViewBag.color = "#b10959";
            }
            else if (show == "elinnombrable")
            {
                ViewBag.image = "elinnombrable.png";
                ViewBag.show = "El Innombrable";
                ViewBag.color = "#b10959";
            }
            else if (show == "magalitajes")
            {
                ViewBag.image = "magalitajes.png";
                ViewBag.show = "#LOSOTROS Magalí Tajes";
                ViewBag.color = "#9f0f81";
            }
            else
            {
                ViewBag.mensaje = "Esta url no existe :(";
                ViewBag.show = "";
                ViewBag.image = "";
                ViewBag.color = "black";
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
                ViewBag.color = "#1cadc3";
            }
            else if (fileNameShow == "nicolasdetracy")
            {
                ViewBag.image = "nicolasdetracy.png";
                ViewBag.show = "Nicolas de Tracy";
                ViewBag.color = "brown";
            }
            else if (fileNameShow == "lailaygonzo")
            {
                ViewBag.image = "lailaygonzo.png";
                ViewBag.show = "Laila y Gonzo";
                ViewBag.color = "#b10959";
            }
            else if (fileNameShow == "elinnombrable")
            {
                ViewBag.image = "losotros.png";
                ViewBag.show = "El Innombrable";
                ViewBag.color = "#b10959";
            }
            else if (fileNameShow == "magalitajes")
            {
                ViewBag.image = "magalitajes.png";
                ViewBag.show = "#LOSOTROS - Magalí Tajes";
                ViewBag.color = "#9f0f81";
            }
            else
            {
                ViewBag.mensaje = "Esta url no existe :(";
                ViewBag.show = "";
                ViewBag.image = "";
                ViewBag.color = "black";
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
                if (votacion.Notificaciones == null)
                    votacion.Notificaciones = "off";

                if (votacion.Descuentos == null)
                    votacion.Descuentos = "off";

                service.AddVotacion(votacion);
                ViewBag.mensaje = "creado";
                return RedirectToAction("Ranking", new { show = fileNameShow });
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Ranking(string show, string listado)
        {
            var showNombreCorrecto = "";
            if (show == "sanata")
            {
                ViewBag.image = "sanata.png";
                ViewBag.show = "Sanata Stand Up";
                showNombreCorrecto = "Sanata Stand Up";
                ViewBag.color = "#1cadc3";
            }
            else if (show == "nicolasdetracy")
            {
                ViewBag.image = "nicolasdetracy.png";
                ViewBag.show = "Nicolas de Tracy";
                showNombreCorrecto = "Nicolas de Tracy";
                ViewBag.color = "brown";
            }
            else if (show == "lailaygonzo")
            {
                ViewBag.image = "lailaygonzo.png";
                ViewBag.show = "Laila y Gonzo";
                showNombreCorrecto = "Laila y Gonzo";
                ViewBag.color = "#b10959";
            }
            else if (show == "elinnombrable")
            {
                ViewBag.image = "elinnombrable.png";
                ViewBag.show = "El Innombrable";
                showNombreCorrecto = "El Innombrable";
                ViewBag.color = "#b10959";
            }
            else
            {
                ViewBag.mensaje = "Esta url no existe :(";
                ViewBag.show = "";
                ViewBag.image = "";
                ViewBag.color = "black";
                return View();
            }
            var service = new VotacionService();
            var ranking = service.GetRankingByShow(showNombreCorrecto, listado);
            ViewBag.ranking = ranking;
            return View();
        }

        [HttpGet]
        [UserValidationFilter]
        public ActionResult Votaciones()
        {
            ViewBag.titulo = "Votaciones";
            var service = new VotacionService();
            try
            {
                ViewBag.votaciones = service.GetVotaciones();
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Votaciones", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        private List<string> GetCiudadesParaMostrar()
        {
            var ciudades = new List<string>();
            var ciudadesFull = UtilitiesAndStuff.GetCiudades();
            var ciudadesFullOrdenadas = ciudadesFull.OrderBy(f => f.Nombre);
            foreach (var item in ciudadesFullOrdenadas)
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