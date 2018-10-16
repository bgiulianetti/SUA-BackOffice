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
    public class ShowController : Controller
    {
        [HttpGet]
        public ActionResult Show(string id)
        {
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.mensaje = "Get";
            ViewBag.colores = UtilitiesAndStuff.GetColores();
            var productoresService = new ProductorService();
            ViewBag.productores = productoresService.GetProductores();


            var standuperoService = new StanduperoService();
            var standuperos = standuperoService.GetStanduperos();

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.standuperos = standuperos;
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Show";
            }
            else
            {
                var showService = new ShowService();
                var show = showService.GetShowById(id);

                foreach (var item in show.Integrantes)
                {
                    if(standuperos.Contains(item))
                        standuperos.Remove(item);
                }

                ViewBag.productor = show.Productor.Dni;
                ViewBag.standuperos = standuperos;
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Show";
                return View(show);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Show(Show show, string accion, string _standuperos, string _productor, string _plazasRepetir)
        {
            ViewBag.titulo = "Crear Show";
            show.Integrantes = GetStanduperosListByDnis(_standuperos);
            show.Productor = new ProductorService().GetProductorByDni(_productor);
            show.SiglaBordereaux = show.SiglaBordereaux.ToUpper();
            show.Repeticion = GenerateRepeticionPlazas(_plazasRepetir);
            ViewBag.colores = UtilitiesAndStuff.GetColores();
            var service = new ShowService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    show.SetIdAndFechaAlta();
                    service.AddShow(show);
                    ViewBag.mensaje = "creado";
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateShow(show);
                    ViewBag.mensaje = "actualizado";
                    ViewBag.productor = show.Productor.Dni;
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
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.titulo = "Shows";
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

        public ActionResult DeleteShow(string id)
        {
            var service = new ShowService();
            try
            {
                service.DeleteShow(id);
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Shows", "Show");
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

        private List<RepeticionPlazas> GenerateRepeticionPlazas(string repeticion)
        {
            var lista = new List<RepeticionPlazas>();
            var repeticiones = repeticion.Split('-');
            foreach (var item in repeticiones)
            {
                var plaza = item.Split('(');
                var ciudad = plaza[0];
                var dias = Int32.Parse(plaza[1].Replace(" días", ""));
                lista.Add(new RepeticionPlazas {
                    Ciudad = ciudad,
                    Dias = dias
                });
            }
            return lista;
        }
    }
}