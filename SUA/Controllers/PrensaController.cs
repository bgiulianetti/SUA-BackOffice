using Newtonsoft.Json;
using SUA.Filters;
using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class PrensaController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Prensa(string id)
        {
            ViewBag.mensaje = "Get";
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Prensa";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Prensa";
                var service = new PrensaService();
                var prensa = service.GetPrensaById(id);
                return View(prensa);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Prensa(Prensa prensa, string accion)
        {
            ViewBag.titulo = "Crear Prensa";
            var service = new PrensaService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    prensa.SetId();
                    service.AddPrensa(prensa);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Prensa", "Crear", JsonConvert.SerializeObject(prensa));
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdatePrensa(prensa);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Prensa", "Editar", JsonConvert.SerializeObject(prensa));
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
        public ActionResult Prensas()
        {
            ViewBag.titulo = "Prensas";
            var service = new PrensaService();
            try
            {
                ViewBag.prensas = service.GetPrensa();
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Prensa", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeletePrensa(string id)
        {
            var service = new PrensaService();
            try
            {
                var prensa = service.GetPrensaById(id);
                service.DeletePrensa(id);
                new LogService().FormatAndSaveLog("Prensa", "Borrar", JsonConvert.SerializeObject(prensa));
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Prensas", "Prensa");
        }
    }
}