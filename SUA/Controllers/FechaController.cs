﻿using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class FechaController : Controller
    {
        [HttpGet]
        public ActionResult Fecha(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.salas = new SalaService().GetSalas();
            ViewBag.shows = new ShowService().GetShows();
            ViewBag.productores = new ProductorService().GetProductores();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Fecha";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Fecha";
                var service = new FechaService();
                var fecha = service.GetFechaById(id);
                ViewBag.productor = fecha.Productor.Dni;
                ViewBag.sala = fecha.Sala.UniqueId;
                ViewBag.show = fecha.Show.UniqueId;
                return View(fecha);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Fecha(Fecha fecha, string accion, string showsList, string salasList, string productoresList)
        {
            ViewBag.titulo = "Crear Fecha";
            fecha.Productor = new ProductorService().GetProductorByDni(productoresList);
            fecha.Sala = new SalaService().GetSalaById(salasList);
            fecha.Show = new ShowService().GetShowById(showsList);

            var service = new FechaService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    fecha.SetIdAndFechaAlta();
                    service.AddFecha(fecha);
                    ViewBag.mensaje = "creado";
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateFecha(fecha);
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
        public ActionResult Fechas()
        {
            ViewBag.titulo = "Fechas";
            var service = new FechaService();
            try
            {
                var fechas = service.GetFechas();
                ViewBag.fechas = fechas;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        public ActionResult DeleteFecha(string id)
        {
            var service = new FechaService();
            try
            {
                service.DeleteFecha(id);
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Fechas", "Fecha");
        }

    }
}