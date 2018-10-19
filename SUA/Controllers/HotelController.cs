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
    public class HotelController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Hotel(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Hotel";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Hotel";
                var service = new HotelService();
                var hotel = service.GetHotelById(id);
                return View(hotel);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Hotel(Hotel hotel, string accion)
        {
            ViewBag.titulo = "Crear Hotel";
            hotel.Direccion.Provincia = hotel.Direccion.Provincia.Replace("@", " ");
            var service = new HotelService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    hotel.SetId();
                    service.AddHotel(hotel);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Hotel", "Crear", JsonConvert.SerializeObject(hotel));
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateHotel(hotel);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Hotel", "Editar", JsonConvert.SerializeObject(hotel));
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
        public ActionResult Hoteles()
        {
            ViewBag.titulo = "Hoteles";
            var service = new HotelService();
            try
            {
                ViewBag.hoteles = service.GetHoteles();
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Hotel", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteHotel(string id)
        {
            var service = new HotelService();
            try
            {
                var hotel = service.GetHotelById(id);
                service.DeleteHotel(id);
                new LogService().FormatAndSaveLog("Sala", "Borrar", JsonConvert.SerializeObject(hotel));
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Hoteles", "Hotel");
        }
    }
}