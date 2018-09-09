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
    public class ProductorController : Controller
    {
        [HttpGet]
        public ActionResult Productor(string dni)
        {
            ViewBag.mensaje = "";
            ViewBag.bancos = UtilitiesAndStuff.GetBancos();
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            if (string.IsNullOrEmpty(dni))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Productor";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Productor";
                var service = new ProductorService();
                var productor = service.GetProductorByDni(dni);
                return View(productor);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Productor(Productor productor, string accion)
        {
            var service = new ProductorService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddProductor(productor);
                    ViewBag.mensaje = "creado";
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateProductor(productor);
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
        public ActionResult Productores()
        {
            ViewBag.titulo = "Productores";
            var service = new ProductorService();
            try
            {
                var productores = service.GetProductores();
                ViewBag.productores = productores;
                ViewBag.mensaje = "ok";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
                //redirigir a pagina 4XX mostrando el error
            }
            return View();
        }

        public ActionResult DeleteProductor(string dni)
        {
            var service = new ProductorService();
            try
            {
                service.DeleteProductor(dni);
            }
            catch/* (Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Productores", "Productor");
        }
    }
}