using Newtonsoft.Json;
using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
        {/*
            var service = new FechaService();
            var fecha = service.GetUltimaFechaByShowId(id);
            return JsonConvert.SerializeObject(fecha);*/
            return "";
        }

        [HttpGet]
        public string GetUltimaFechaBySala(string idSala)
        {/*
            var service = new FechaService();
            var fecha = service.GetUltimaFechaBySalaId(idSala);
            return JsonConvert.SerializeObject(fecha);*/
            return "";
        }

        [HttpGet]
        public string GetUltimaFechaByShowAndSala(string idShow, string idSala)
        {/*
            var service = new FechaService();
            var fecha = service.GetUltimaFechaBySalaAndShow(idSala, idShow);
            return JsonConvert.SerializeObject(fecha);*/
            return "";
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
                ViewBag.arregloFijo = "false";
                ViewBag.accion = "Post";
                ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
                ViewBag.gastos = UtilitiesAndStuff.GetGastosCompany();
                ViewBag.entradas = UtilitiesAndStuff.GetEntradas();
            }
            else
            {
                ViewBag.arregloFijo = fecha.Borederaux.ArregloFijo.ToString().ToLower();
                ViewBag.accion = "Put";
                ViewBag.impuestos = UtilitiesAndStuff.GetImpuestos();
                ViewBag.gastos = UtilitiesAndStuff.GetGastosCompany();
                ViewBag.entradas = UtilitiesAndStuff.GetEntradas();
                ViewBag.idFecha = fecha.UniqueId;
                return View(fecha.Borederaux);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Bordereaux(Bordereaux bordereaux, string accion, string _entradas, string _impuestos, string _gastos, string id, string _arregloFijo, 
                                       string ImpuestosDeduccionesBruto, string ImpuestosDeduccionesTotalDeducir, string ImpuestosDeduccionesNeto, string ImpuestosDeduccionesCompanyPorcentaje, 
                                       string ImpuestosDeduccionesCompanyMonto, string ImpuestosDeduccionesTeatroPorcentaje, string ImpuestosDeduccionesTeatroMonto, string GastosCompanyTotal,
                                       string GastosCompanyNeto, string SUAPorcentaje, string SUAMonto, string ShowPorcentaje, string ShowMonto)
        {
            ViewBag.titulo = "Bordereaux";

            bordereaux.ImpuestosDeduccionesBruto = float.Parse(ImpuestosDeduccionesBruto, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesTotalDeducir = float.Parse(ImpuestosDeduccionesTotalDeducir, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesNeto = float.Parse(ImpuestosDeduccionesNeto, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesCompanyPorcentaje = float.Parse(ImpuestosDeduccionesCompanyPorcentaje, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesCompanyMonto = float.Parse(ImpuestosDeduccionesCompanyMonto, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesTeatroPorcentaje = float.Parse(ImpuestosDeduccionesTeatroPorcentaje, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesTeatroMonto = float.Parse(ImpuestosDeduccionesTeatroMonto, CultureInfo.InvariantCulture);
            bordereaux.GastosCompanyTotal = float.Parse(GastosCompanyTotal, CultureInfo.InvariantCulture);
            bordereaux.GastosCompanyNeto = float.Parse(GastosCompanyNeto, CultureInfo.InvariantCulture);
            bordereaux.SUAPorcentaje = float.Parse(SUAPorcentaje, CultureInfo.InvariantCulture);
            bordereaux.SUAMonto = float.Parse(SUAMonto, CultureInfo.InvariantCulture);
            bordereaux.ShowPorcentaje = float.Parse(ShowPorcentaje, CultureInfo.InvariantCulture);
            bordereaux.ShowMonto = float.Parse(ShowMonto, CultureInfo.InvariantCulture);

            bordereaux.Entradas = GetEntradas(_entradas);
            bordereaux.ImpuestosDeduccionesTeatro = GetImpuestos(_impuestos);
            bordereaux.GastosCompany = GetGastos(_gastos);

            if (_arregloFijo == "si")
                bordereaux.ArregloFijo = true;
            else
                bordereaux.ArregloFijo = false;

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

        [HttpGet]
        public ActionResult FechasCerradas()
        {
            ViewBag.titulo = "Fechas Cerradas";
            try
            {
                ViewBag.mensaje = "listar";
                var service = new FechaService();
                var fechas = service.GetFechasConBordereaux();
                ViewBag.fechas = fechas;
            }
            catch(Exception ex)
            {
                ViewBag.menasje = ex.Message;
            }
            
            return View();
        }

        public ActionResult Test()
        {
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
                    Precio = float.Parse(entradaString[2], CultureInfo.InvariantCulture),
                    Total = float.Parse(entradaString[3], CultureInfo.InvariantCulture)
                });
            }
            return entradas;
        }

        public List<ImpuestosDeduccionesTeatroBorderaux> GetImpuestos(string _impuestos)
        {
            var impuestos = new List<ImpuestosDeduccionesTeatroBorderaux>();
            if (string.IsNullOrEmpty(_impuestos))
                return impuestos;

            var lista = _impuestos.Split('*').ToList();
            foreach (var item in lista)
            {
                var impuestoString = item.Split('-');
                impuestos.Add(new ImpuestosDeduccionesTeatroBorderaux
                {
                    Nombre = impuestoString[0],
                    Porcentaje = float.Parse(impuestoString[1], CultureInfo.InvariantCulture),
                    Monto = float.Parse(impuestoString[2], CultureInfo.InvariantCulture),
                    Comentarios = impuestoString[3]
                });
            }
            return impuestos;
        }

        public List<GastosBordereaux> GetGastos(string _gastos)
        {
            var gastos = new List<GastosBordereaux>();
            if (string.IsNullOrEmpty(_gastos))
                return gastos;

            var gastosList = _gastos.Split('*').ToList();
            foreach (var item in gastosList)
            {
                var gasto = item.Split('-');
                gastos.Add(new GastosBordereaux {Gasto = gasto[0], Monto = float.Parse(gasto[1], CultureInfo.InvariantCulture), Detalle = gasto[0] });
            }
            return gastos;
        }

        public void SendEmail(Fecha fecha)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("notificacion.ypf@gmail.com", "NotificacionYpf2018");



            string body = "Se ha subido un nuevo archivo PDF.";

            MailMessage mm = new MailMessage("notificacion.ypf@gmail.com", "destinatario", "Actualizacion de PDF - [BO YPF]", body);

            //var attachment1 = new System.Net.Mail.Attachment(pdf);
            //mm.Attachments.Add(attachment1);


            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


            client.Send(mm);

        }

    }
}