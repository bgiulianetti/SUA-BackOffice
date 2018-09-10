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

        private List<string> GetImpuestosList(string impuestos)
        {
            return impuestos.Split('-').ToList();
        }
    }
}