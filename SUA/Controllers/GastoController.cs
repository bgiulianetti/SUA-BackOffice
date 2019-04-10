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
    public class GastoController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Gasto(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.categorias = UtilitiesAndStuff.GetCategoriasDeGastos();
            ViewBag.personas = GetPersonas();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Gasto";
                ViewBag.quien = "";
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Gasto";
                var service = new GastoService();
                var gasto = service.GetGastoById(id);
                ViewBag.quien = gasto.Quien.Dni;
                return View(gasto);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Gasto(Gasto gasto, string personaDNI, string accion)
        {
            ViewBag.titulo = "Crear Gasto";
            if (gasto.Categoria == "Tecnico" || gasto.Categoria == "Premios" || gasto.Categoria == "Afip" ||
                gasto.Categoria == "Sueldos" || gasto.Categoria == "Celular" || gasto.Categoria == "Programacion" ||
                gasto.Categoria == "Productor")
            {
                gasto.Quien = ObtenerPersona(personaDNI);
            }
            var service = new GastoService();

            try
            {
                if (string.Equals(accion, "Post"))
                {
                    gasto.UniqueId = UtilitiesAndStuff.GenerateUniqueId();
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
                if (gasto != null)
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


        private List<SelectListItem> GetPersonas()
        {
            var standuperos = new StanduperoService().GetStanduperos();
            var productores = new ProductorService().GetProductores();

            var personas = new List<Persona>();
            personas.AddRange(standuperos);
            personas.AddRange(productores);

            return UtilitiesAndStuff.ConvertPeronasListToSelectItemList(personas.Distinct().OrderBy(a => a.Apellido).ToList());
        }

        private Persona ObtenerPersona(string dni)
        {
            Persona persona = null;
            persona = new StanduperoService().GetStanduperoByDni(dni);
            if (persona == null)
            {
                persona = new ProductorService().GetProductorByDni(dni);
            }
            return persona;
        }
    }
}