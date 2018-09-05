using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class ProductorController : Controller
    {
        [HttpGet]
        public ActionResult Productor(string dni)
        {
            ViewBag.mensaje = "";
            ViewBag.bancos = UtilitiesAndStuff.GetBancos();
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            if (string.IsNullOrEmpty(dni))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Standupero";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Standupero";
                var service = new StanduperoService();
                var standupero = service.GetStanduperoByDni(dni);
                return View(standupero);
            }
            return View();
        }
    }
}