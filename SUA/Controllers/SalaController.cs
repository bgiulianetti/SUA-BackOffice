using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class SalaController : Controller
    {
        [HttpGet]
        public ActionResult Sala(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Sala";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Sala";
                var service = new SalaService();
                var sala = service.GetSalaById(id);
                return View(sala);
            }
            return View();
        }
    }
}