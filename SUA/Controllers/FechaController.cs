using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class FechaController : Controller
    {
        [HttpGet]
        public ActionResult Fecha(string dni)
        {
            ViewBag.mensaje = "Get";
            ViewBag.salas = new SalaService().GetSalas();
            ViewBag.shows = new ShowService().GetShows();
            ViewBag.productores = new ProductorService().GetProductores();
            if (string.IsNullOrEmpty(dni))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Fecha";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Fecha";
                var service = new StanduperoService();
                var standupero = service.GetStanduperoByDni(dni);
                return View(standupero);
            }
            return View();
        }


        [HttpGet]
        public ActionResult Fechas()
        {
            ViewBag.titulo = "Fechas";
            var service = new StanduperoService();
            try
            {
                var standuperos = service.GetStanduperos();
                ViewBag.standuperos = standuperos;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }
    }
}