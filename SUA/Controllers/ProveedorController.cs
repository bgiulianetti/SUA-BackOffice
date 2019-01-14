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
    public class ProveedorController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Proveedor(string id)
        {
            ViewBag.mensaje = "Get";
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Proveedor";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Proveedor";
                var service = new ProveedorService();
                var proveedor = service.GetProveedorById(id);
                return View(proveedor);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Proveedor(Proveedor proveedor, string accion)
        {
            ViewBag.titulo = "Crear Proveedor";
            var service = new ProveedorService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    proveedor.SetId();
                    service.AddProveedor(proveedor);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Proveedor", "Crear", JsonConvert.SerializeObject(proveedor));
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateProveedor(proveedor);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Proveedor", "Editar", JsonConvert.SerializeObject(proveedor));
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
        public ActionResult Proveedores()
        {
            ViewBag.titulo = "Proveedores";
            var service = new ProveedorService();
            try
            {
                ViewBag.restaurantes = service.GetProveedores();
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Proveedor", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteProveedor(string id)
        {
            var service = new ProveedorService();
            try
            {
                var proveedor = service.GetProveedorById(id);
                service.DeleteProveedor(id);
                new LogService().FormatAndSaveLog("Proveedor", "Borrar", JsonConvert.SerializeObject(proveedor));
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Proveedores", "Proveedor");
        }
    }
}