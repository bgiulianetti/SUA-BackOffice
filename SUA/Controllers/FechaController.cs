using Newtonsoft.Json;
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

        [HttpGet]
        public string GetUltimaFechaByShow(string id)
        {
            var service = new FechaService();
            var fecha = service.GetUltimaFechaByShowId(id);
            return JsonConvert.SerializeObject(fecha);
        }

        [HttpGet]
        public string GetUltimaFechaBySala(string idSala)
        {
            var service = new FechaService();
            var fecha = service.GetUltimaFechaBySalaId(idSala);
            return JsonConvert.SerializeObject(fecha);
        }

        [HttpGet]
        public string GetUltimaFechaByShowAndSala(string idShow, string idSala)
        {
            var service = new FechaService();
            var fecha = service.GetUltimaFechaBySalaAndShow(idSala, idShow);
            return JsonConvert.SerializeObject(fecha);
        }

        [HttpGet]
        public string CalcularVencimiento(string idSala)
        {
            if (idSala == "")
                return "";

            var salaService = new SalaService();
            var sala = salaService.GetSalaById(idSala);
            if (sala == null)
                return "";

            var fechaService = new FechaService();
            var fecha = fechaService.GetUltimaFechaBySalaId(idSala);
            if (fecha == null)
                return "";

            var vencimiento = UtilitiesAndStuff.CalcularVencimiento(fecha.FechaHorario, sala.RepeticionEnDias);
            return vencimiento.ToString();
        }

        [HttpGet]
        public ActionResult Bordereaux (string id)
        {
            ViewBag.titulo = "Bordereaux";
            ViewBag.mensaje = "Get";

            var fechaService = new FechaService();
            var fecha = fechaService.GetFechaById(id);
            if(fecha.Borederaux == null)
            {
                
                ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
                ViewBag.gastos = UtilitiesAndStuff.GetGastosCompany();
                ViewBag.entradas = UtilitiesAndStuff.GetEntradas();
            }
            else
            {
                ViewBag.accion = "Put";
                ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
                ViewBag.gastos = UtilitiesAndStuff.GetGastosCompany();
                ViewBag.entradas = UtilitiesAndStuff.GetEntradas();
                return View(fecha.Borederaux);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Bordereaux(Bordereaux bordereaux, string accion, string _entradas, string _impuestos, string _gastos, string id)
        {
            ViewBag.titulo = "Bordereaux";
            bordereaux.Entradas = GetEntradas(_entradas);
            bordereaux.ImpuestosDeduccionesTeatro = GetImpuestos(_impuestos);
            bordereaux.GastosCompany = GetGastos(_gastos);
            var service = new FechaService();
            var fecha = service.GetFechaById(id);
            try
            {
                fecha.Borederaux = bordereaux;
                service.UpdateFecha(fecha);
                if (fecha.Borederaux != null)
                    ViewBag.mensaje = "creado";
                else
                    ViewBag.mensaje = "actualizado";
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


        public List<EntradasBorderaux> GetEntradas(string _entradas)
        {
            var lista = _entradas.Split('*').ToList();
            var entradas = new List<EntradasBorderaux>();
            foreach (var item in lista)
            {
                var entradaString = item.Split('-');
                entradas.Add(new EntradasBorderaux {
                    Nombre = entradaString[0],
                    Cantidad = Int32.Parse(entradaString[1]),
                    Precio = double.Parse(entradaString[2]),
                    Total = double.Parse(entradaString[3])
                });
            }
            return entradas;
        }

        public List<ImpuestosDeduccionesTeatroBorderaux> GetImpuestos(string _impuestos)
        {
            var lista = _impuestos.Split('*').ToList();
            var impuestos = new List<ImpuestosDeduccionesTeatroBorderaux>();
            foreach (var item in lista)
            {
                var impuestoString = item.Split('-');
                impuestos.Add(new ImpuestosDeduccionesTeatroBorderaux
                {
                    Nombre = impuestoString[0],
                    Porcentaje = double.Parse(impuestoString[1]),
                    Monto = double.Parse(impuestoString[2]),
                    Comentarios = impuestoString[3]
                });
            }
            return impuestos;
        }

        public Dictionary<string, double> GetGastos(string _gastos)
        {
            var gastosList = _gastos.Split('*').ToList();
            var gastos = new Dictionary<string, double>();
            foreach (var item in gastosList)
            {
                var gasto = item.Split('-');
                gastos.Add(gasto[0], double.Parse(gasto[1]));
            }
            return gastos;
        }
       
    }
}