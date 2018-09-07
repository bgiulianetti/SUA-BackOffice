using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class ShowController : Controller
    {
        [HttpGet]
        public ActionResult Show(string id)
        {
            ViewBag.mensaje = "";
            var standuperoService = new StanduperoService();
            ViewBag.standuperos = standuperoService.GetStanduperos();

            var productoresService = new ProductorService();
            ViewBag.productores = productoresService.GetProductores();
            
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Show";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Show";
                var showService = new ShowService();
                var show = showService.GetShowById(id);
                return View(show);
            }
            return View();
        }
    }
}