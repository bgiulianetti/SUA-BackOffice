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
            ViewBag.mensaje = "";
            var standuperoService = new StanduperoService();
            ViewBag.standuperos = standuperoService.GetStanduperos();

            var productoresService = new ProductorService();
            var productores = productoresService.GetProductores();
            ViewBag.productores = ConvertToSelectListItem(productores);

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
        public ActionResult Show(Show show, string accion, string _standuperos)
        {
            var DNIs = _standuperos.Split('-').ToList();
            var standuperos = new List<Standupero>();
            var standuperoService = new StanduperoService();
            foreach (var item in DNIs)
            {
                standuperos.Add(standuperoService.GetStanduperoByDni(item));
            }
            show.Integrantes = standuperos;

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

        private List<SelectListItem> ConvertToSelectListItem(List<Productor> productores)
        {
            var selectList = new List<SelectListItem>();
            foreach (var item in productores)
                selectList.Add(new SelectListItem { Text = item.Dni, Value = item.Nombre + " " + item.Apellido });

            return selectList;
        }
    }
}