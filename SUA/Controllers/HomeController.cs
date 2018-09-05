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
            return View();
        }

        [HttpGet]
        public ActionResult Standupero(string dni)
        {
            ViewBag.mensaje = "";
            ViewBag.bancos = new Banco().GetBancos();
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            if (string.IsNullOrEmpty(dni))
                ViewBag.accion = "Post";
            else
            {
                ViewBag.accion = "Put";
                var service = new StanduperoService();
                var standupero = service.GetStanduperoByDni(dni);
                return View(standupero);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Standupero(Standupero standupero)
        {
            var service = new StanduperoService();
            try
            {
                service.AddStandupero(standupero);
                ViewBag.mensaje = "ok";
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
            var service = new StanduperoService();
            var standuperos = service.GetStanduperos();
            ViewBag.standuperos = standuperos;
            return View();
        }

        public ActionResult DeleteStandupero()
        {
            int a = 0;
            a++;
            return null;
        }

    }
}