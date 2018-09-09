using SUA.Models;
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
            ViewBag.mensaje = "Get";

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

        [HttpPost]
        public ActionResult Show(Show show, string accion, string _standuperos, string _productor)
        {
            ViewBag.titulo = "Crear Show";
            show.Integrantes = GetStanduperosListByDnis(_standuperos);
            show.Productor = new ProductorService().GetProductorByDni(_productor);
            show.SetIdAndFechaAlta();

            var service = new ShowService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddShow(show);
                    ViewBag.mensaje = "creado";
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateShow(show);
                    ViewBag.mensaje = "actualizado";
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Shows()
        {
            ViewBag.titulo = "Productores";
            var service = new ShowService();
            try
            {
                var shows = service.GetShows();
                ViewBag.shows = shows;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        private List<Standupero> GetStanduperosListByDnis(string dnis)
        {
            var dniList = dnis.Split('-').ToList();
            var standuperos = new List<Standupero>();
            var standuperoService = new StanduperoService();
            foreach (var item in dniList)
            {
                standuperos.Add(standuperoService.GetStanduperoByDni(item));
            }
            return standuperos;
        }
    }
}