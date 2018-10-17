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
    public class SalaController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Sala(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.provincias = UtilitiesAndStuff.GetProvincias();
            ViewBag.paises = UtilitiesAndStuff.GetPaises();
            ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Sala";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Sala";
                var service = new SalaService();
                var sala = service.GetSalaById(id);
                return View(sala);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Sala(Sala sala, string accion, string _impuestos)
        {
            ViewBag.titulo = "Crear Sala";
            sala.ImpuestosYGastos = GetImpuestosList(_impuestos);

            sala.Direccion.Provincia = sala.Direccion.Provincia.Replace("@", " ");

            var service = new SalaService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    sala.SetIdAndFechaAlta();
                    service.AddSala(sala);
                    ViewBag.mensaje = "creado";
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateSala(sala);
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
        [UserValidationFilter]
        public ActionResult Salas()
        {
            ViewBag.titulo = "Salas";
            var service = new SalaService();
            try
            {
                var salas = service.GetSalas();
                ViewBag.salas = salas;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [UserValidationFilter]
        public ActionResult DeleteSala(string id)
        {
            var service = new SalaService();
            try
            {
                service.DeleteSala(id);
            }
            catch /*(Exception ex)*/
            {
                //loguear mensaje o mandar pagina de error
            }
            return RedirectToAction("Salas", "Sala");
        }

        private List<string> GetImpuestosList(string impuestos)
        {
            return impuestos.Split('-').ToList();
        }
    }
}