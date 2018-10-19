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
    public class RestauranteController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Restaurante(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Restaurante";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Restaurante";
                var service = new RestauranteService();
                var restaurante = service.GetRestauranteById(id);
                return View(restaurante);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Restaurante(Restaurante restaurante, string accion)
        {
            ViewBag.titulo = "Crear Restaurante";
            restaurante.Direccion.Provincia = restaurante.Direccion.Provincia.Replace("@", " ");
            var service = new RestauranteService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    restaurante.SetId();
                    service.AddRestaurante(restaurante);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Restaurante", "Crear", JsonConvert.SerializeObject(restaurante));
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateRestaurante(restaurante);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Restaurante", "Editar", JsonConvert.SerializeObject(restaurante));
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
        public ActionResult Restaurantes()
        {
            ViewBag.titulo = "Restaurantes";
            var service = new RestauranteService();
            try
            {
                ViewBag.restaurantes = service.GetRestaurantes();
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Restaurante", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteRestaurante(string id)
        {
            var service = new RestauranteService();
            try
            {
                var restaurante = service.GetRestauranteById(id);
                service.DeleteRestaurante(id);
                new LogService().FormatAndSaveLog("Restaurante", "Borrar", JsonConvert.SerializeObject(restaurante));
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Hoteles", "Hotel");
        }
    }
}