﻿using Newtonsoft.Json;
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
    public class GastoController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Gasto(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.categorias = UtilitiesAndStuff.GetCategoriasDeGastos();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Gasto";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Gasto";
                var service = new GastoService();
                var gasto = service.GetGastoById(id);
                return View(gasto);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Gasto(Gasto gasto, string accion)
        {
            ViewBag.titulo = "Crear Gasto";
            var service = new GastoService();

            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddGasto(gasto);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Gasto", "Crear", JsonConvert.SerializeObject(gasto));
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateGasto(gasto);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Gasto", "Editar", JsonConvert.SerializeObject(gasto));
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
        public ActionResult Gastos()
        {
            ViewBag.titulo = "Gastos";
            var service = new GastoService();
            try
            {
                var gastos = service.GetGastos();
                ViewBag.gastos = gastos;
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Gasto", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteGasto(string id)
        {
            var service = new GastoService();
            try
            {
                var gasto = service.GetGastoById(id);
                if(gasto != null)
                {
                    service.DeleteGasto(id);
                    new LogService().FormatAndSaveLog("Gasto", "Borrar", JsonConvert.SerializeObject(gasto));
                }
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Gastos", "Gasto");
        }
    }
}