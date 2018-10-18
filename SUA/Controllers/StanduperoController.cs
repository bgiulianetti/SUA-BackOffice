using Newtonsoft.Json;
using SUA.Filters;
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
    public class StanduperoController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Standupero(string dni)
        {
            ViewBag.mensaje = "Get";
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

        [HttpPost]
        public ActionResult Standupero(Standupero standupero, string accion, string copiarFacturacion)
        {
            ViewBag.titulo = "Crear Standupero";
            var service = new StanduperoService();
            standupero.Direccion.Provincia = standupero.Direccion.Provincia.Replace("@", " ");

            if(standupero.DireccionFacturacion != null)
                standupero.DireccionFacturacion.Provincia = standupero.DireccionFacturacion.Provincia.Replace("@", " ");

            if (copiarFacturacion == "copiar")
            {
                standupero.DireccionFacturacion = standupero.Direccion;
            }

            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddStandupero(standupero);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Standupero", "Crear", JsonConvert.SerializeObject(standupero));
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateStandupero(standupero);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Standupero", "Editar", JsonConvert.SerializeObject(standupero));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        [UserValidationFilter]
        public ActionResult Standuperos()
        {
            ViewBag.titulo = "Standuperos";
            var service = new StanduperoService();
            try
            {
                var standuperos = service.GetStanduperos();
                ViewBag.standuperos = standuperos;
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Standupero", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteStandupero(string dni)
        {
            var service = new StanduperoService();
            try
            {
                var standupero = service.GetStanduperoByDni(dni);
                service.DeleteStandupero(dni);
                new LogService().FormatAndSaveLog("Standupero", "Borrar", JsonConvert.SerializeObject(standupero));
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Standuperos", "Standupero");
        }

    }
}