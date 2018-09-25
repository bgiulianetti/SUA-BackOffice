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
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.mensaje = "Get";
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
            ViewBag.titulo = "Crear Productor";
            var service = new ProductorService();
            productor.Direccion.Provincia = productor.Direccion.Provincia.Replace("@", " ");
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
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.titulo = "Productores";
            var service = new ProductorService();
            try
            {
                var productores = service.GetProductores();
                ViewBag.productores = productores;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
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