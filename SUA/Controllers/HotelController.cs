using SUA.Filters;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class HotelController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Hotel(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Hotel";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Hotel";
                var service = new SalaService();
                var sala = service.GetSalaById(id);
                return View(sala);
            }
            return View();
        }
}