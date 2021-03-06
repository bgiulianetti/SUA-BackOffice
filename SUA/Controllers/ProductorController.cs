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
    public class ProductorController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Productor(string dni)
        {
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
        public ActionResult Productor(Productor productor, string accion, string copiarFacturacion)
        {
            ViewBag.titulo = "Crear Productor";
            var service = new ProductorService();
            productor.Direccion.Provincia = productor.Direccion.Provincia.Replace("@", " ");

            if(productor.DireccionFacturacion != null)
                productor.DireccionFacturacion.Provincia = productor.DireccionFacturacion.Provincia.Replace("@", " ");

            if (copiarFacturacion == "copiar")
            {
                productor.DireccionFacturacion = productor.Direccion;
            }
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    service.AddProductor(productor);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Productor", "Crear", JsonConvert.SerializeObject(productor));
                }

                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateProductor(productor);
                    ActualizarDependencias(productor);
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Productor", "Editar", JsonConvert.SerializeObject(productor));
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
        public ActionResult Productores()
        {
            ViewBag.titulo = "Productores";
            var service = new ProductorService();
            try
            {
                var productores = service.GetProductores();
                ViewBag.productores = productores;
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Productor", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteProductor(string dni)
        {
            var service = new ProductorService();
            try
            {
                var productor = service.GetProductorByDni(dni);
                service.DeleteProductor(dni);
                new LogService().FormatAndSaveLog("Productor", "Borrar", JsonConvert.SerializeObject(productor));
            }
            catch/* (Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Productores", "Productor");
        }

        private void ActualizarDependencias(Productor productor)
        {
            ////////////////Shows/////////////////////
            var showService = new ShowService();
            var shows = showService.GetShows();
            foreach (var show in shows)
            {
               if(show.Productor.Dni == productor.Dni)
                {
                    show.Productor = productor;
                    showService.UpdateShow(show);
                }
            }

            ////////////////Fechas////////////////
            var fechaService = new FechaService();
            var fechas = fechaService.GetFechas("all");
            foreach (var fecha in fechas)
            {
                if(fecha.Show.Productor.Dni == productor.Dni)
                {
                    fecha.Show.Productor = productor;
                }
                if(fecha.Productor.Dni == productor.Dni)
                {
                    fecha.Productor = productor;
                }
                fechaService.UpdateFecha(fecha);
            }
        }

    }
}