using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.titulo = "Inicio";
            return View();
        }

        [HttpGet]
        public ActionResult Standupero(string dni)
        {
            ViewBag.mensaje = "";
            ViewBag.bancos = UtilitiesAndStuff.GetBancos();
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
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


        [HttpPost]
        public ActionResult Standupero(Standupero standupero, string accion)
        {
            var service = new StanduperoService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddStandupero(standupero);
                    ViewBag.mensaje = "creado";
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateStandupero(standupero);
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
        public ActionResult Standuperos()
        {
            ViewBag.titulo = "Standuperos";
            var service = new StanduperoService();
            try
            {
                var standuperos = service.GetStanduperos();
                ViewBag.standuperos = standuperos;
                ViewBag.mensaje = "ok";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //redirigir a pagina 4XX mostrando el error
            }
            return View();
        }

        public ActionResult DeleteStandupero(string dni)
        {
            var service = new StanduperoService();
            try
            {
                service.DeleteStandupero(dni);
            }
            catch(Exception ex)
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Standuperos", "Home");
        }

    }
}