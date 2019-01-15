using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using SUA.Filters;
using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        [UserValidationFilter]
        public ActionResult Fecha(string id)
        {
            ViewBag.mensaje = "Get";
            ViewBag.salas = new SalaService().GetSalas();
            var shows = SimplifyShowsList(new ShowService().GetShows());
            ViewBag.shows = shows;
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
        public ActionResult Fecha(Fecha fecha, string accion, string showsList, string idSala, string productoresList)
        {
            ViewBag.titulo = "Crear Fecha";
            fecha.Productor = new ProductorService().GetProductorByDni(productoresList);
            fecha.Sala = new SalaService().GetSalaById(idSala);
            fecha.Show = new ShowService().GetShowById(showsList);

            var service = new FechaService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    fecha.SetIdAndFechaAlta();
                    fecha.GoogleCalendarState = "post";
                    service.AddFecha(fecha);
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Fecha", "Crear", JsonConvert.SerializeObject(fecha));
                }
                else if (string.Equals(accion, "Put"))
                {
                    var fechaObtenida = service.GetFechaById(fecha.UniqueId);
                    if (fechaObtenida.Borederaux != null)
                        fecha.Borederaux = fechaObtenida.Borederaux;
                    fecha.GoogleCalendarState = "put";

                    service.UpdateFecha(fecha);
                    ViewBag.mensaje = "actualizado";

                    new LogService().FormatAndSaveLog("Fecha", "Editar", JsonConvert.SerializeObject(fecha));
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
        public ActionResult Fechas(string all = "")
        {
            ViewBag.titulo = "Fechas";
            var service = new FechaService();
            try
            {
                var fechas = service.GetFechas(all);
                ViewBag.fechas = fechas;
                ViewBag.mensaje = "listar";
                new LogService().FormatAndSaveLog("Fecha", "Listar", "");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }


        [HttpGet]
        public string GetRepeticionPlazas(string idShow, DateTime date)
        {
            var fechaService = new FechaService();
            var repeticiones = fechaService.GetRepeticionPlazasByShowAndDate(idShow, date);
            return JsonConvert.SerializeObject(repeticiones);
        }

        [HttpGet]
        [UserValidationFilter]
        public ActionResult Bordereaux(string id)
        {
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.titulo = "Bordereaux";
            ViewBag.mensaje = "Get";

            var fechaService = new FechaService();
            var fecha = fechaService.GetFechaById(id);
            if (fecha.Borederaux == null)
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
                ViewBag.arregloProductor = fecha.Borederaux.ArregloProductor;
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
                                       string entradasBruto, string ImpuestosDeduccionesBruto, string ImpuestosDeduccionesTotalDeducir, string ImpuestosDeduccionesNeto,
                                       string ImpuestosDeduccionesCompanyPorcentaje, string ImpuestosDeduccionesCompanyMonto, string ImpuestosDeduccionesTeatroPorcentaje, string ImpuestosDeduccionesTeatroMonto,
                                       string GastosCompanyTotal, string GastosCompanyNeto, string SUAPorcentaje, string SUAMonto, string ShowPorcentaje, string ShowMonto,
                                       string ArregloProductor, string ProductorMonto, string ProductorPorcentaje, string SUAMontoFinal)
        {
            ViewBag.titulo = "Bordereaux";

            bordereaux.EntradasBruto = float.Parse(entradasBruto, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesBruto = float.Parse(ImpuestosDeduccionesBruto, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesTotalDeducir = float.Parse(ImpuestosDeduccionesTotalDeducir, CultureInfo.InvariantCulture);
            bordereaux.ImpuestosDeduccionesNeto = float.Parse(ImpuestosDeduccionesNeto, CultureInfo.InvariantCulture);
            if (ImpuestosDeduccionesCompanyPorcentaje != "")
            {
                bordereaux.ImpuestosDeduccionesCompanyPorcentaje = float.Parse(ImpuestosDeduccionesCompanyPorcentaje, CultureInfo.InvariantCulture);
            }
            else
            {
                bordereaux.ImpuestosDeduccionesCompanyPorcentaje = 0;
            }

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


            bordereaux.ArregloProductor = ArregloProductor;
            if (ProductorMonto == "")
            {
                bordereaux.ProductorMonto = 0;
                bordereaux.ProductorPorcentaje = 0;
            }
            else
            {
                bordereaux.ProductorMonto = float.Parse(ProductorMonto, CultureInfo.InvariantCulture);
                bordereaux.ProductorPorcentaje = float.Parse(ProductorPorcentaje, CultureInfo.InvariantCulture);
            }
            bordereaux.SUAMontoFinal = float.Parse(SUAMontoFinal, CultureInfo.InvariantCulture);

            var service = new FechaService();
            var fecha = service.GetFechaById(id);
            try
            {
                if (fecha.Borederaux != null)
                {
                    ViewBag.mensaje = "creado";
                    new LogService().FormatAndSaveLog("Bordereaux", "Crear", JsonConvert.SerializeObject(fecha));
                }
                else
                {
                    ViewBag.mensaje = "actualizado";
                    new LogService().FormatAndSaveLog("Bordereaux", "Editar", JsonConvert.SerializeObject(fecha));
                }
                fecha.Borederaux = bordereaux;
                service.UpdateFecha(fecha);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        [UserValidationFilter]
        public ActionResult FechasCerradas()
        {
            ViewBag.titulo = "Fechas Cerradas";
            try
            {
                ViewBag.mensaje = "listar";
                var service = new FechaService();
                var fechas = service.GetFechasConBordereaux();
                ViewBag.fechas = fechas;
                new LogService().FormatAndSaveLog("Fecha", "Fechas Cerradas", "");
            }
            catch (Exception ex)
            {
                ViewBag.menasje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public string GetAntesDespuesFechas(string idSala, DateTime fecha)
        {
            try
            {
                var salaService = new SalaService();
                var ciudad = salaService.GetSalaById(idSala).Direccion.Ciudad;

                var fechaService = new FechaService();
                var desde = fecha.AddDays(-20);
                var hasta = fecha.AddDays(20);
                var fechas = fechaService.GetFechasByCiudadAndRangoFecha(ciudad, desde, hasta);
                return JsonConvert.SerializeObject(fechas);
            }
            catch
            {
                return string.Empty;
            }

        }

        [UserValidationFilter]
        public ActionResult DeleteFecha(string id)
        {
            var service = new FechaService();
            try
            {
                var fecha = service.GetFechaById(id);
                fecha.Status = "deleted";
                fecha.GoogleCalendarState = "delete";
                service.UpdateFecha(fecha);
                new LogService().FormatAndSaveLog("Fecha", "Borrar", JsonConvert.SerializeObject(fecha));
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
            lista.Reverse();
            var entradas = new List<EntradasBorderaux>();
            foreach (var item in lista)
            {
                var entradaString = item.Split('-');
                entradas.Add(new EntradasBorderaux
                {
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
            lista.Reverse();
            foreach (var item in lista)
            {
                var impuestoString = item.Split('-');
                if (impuestoString[1] == "")
                    impuestoString[1] = "0";
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
            gastosList.Reverse();
            foreach (var item in gastosList)
            {
                var gasto = item.Split('-');
                gastos.Add(new GastosBordereaux { Gasto = gasto[0], Monto = float.Parse(gasto[1], CultureInfo.InvariantCulture), Detalle = gasto[2] });
            }
            return gastos;
        }

        [HttpGet]
        public void PrintBordereaux(string id)
        {
            var service = new FechaService();
            var fecha = service.GetFechaById(id);

            Document doc = new Document(PageSize.A4, 20, 20, 15, 10);
            var fileName = "~/assets/Pdf/" + fecha.Show.SiglaBordereaux + fecha.FechaHorario.ToString("yyyyMMdd") + ".pdf";
            string filePath = Server.MapPath(fileName);
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            PdfWriter.GetInstance(doc, file);

            doc.Open();

            AgregarCabecera(doc, fecha);
            AgregarEntradas(doc, fecha.Borederaux);
            doc.Add(new Paragraph(" "));
            AgregarImpuestos(doc, fecha.Borederaux);
            doc.Add(new Paragraph(" "));
            AgregarGastos(doc, fecha.Borederaux);
            doc.Add(new Paragraph(" "));
            AgregarTotales(doc, fecha.Borederaux);
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            AgregaFirmas(doc);

            doc.Close();
            Process.Start(filePath);

            //descargo el archivo
            var di = new DirectoryInfo(filePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(di.FullName);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            Response.Redirect(fileName);

            new LogService().FormatAndSaveLog("Bordereaux", "Imprimir", fecha.UniqueId);

        }

        [HttpGet]
        public void InfoFecha(string id)
        {
            var service = new FechaService();
            var fecha = service.GetFechaById(id);

            Document doc = new Document(PageSize.A4, 20, 20, 15, 10);
            var fileName = "~/assets/Pdf/fecha_" + fecha.UniqueId + ".pdf";
            string filePath = Server.MapPath(fileName);
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            PdfWriter.GetInstance(doc, file);

            doc.Open();

            string showLogo = Server.MapPath("~/assets/img/" + fecha.Show.UniqueId + ".png");
            var jpgLogo = Image.GetInstance(showLogo);
            jpgLogo.ScaleToFit(140f, 60f);
            jpgLogo.SetAbsolutePosition(15f, PageSize.A4.Height - 135f);
            doc.Add(jpgLogo);


            //Logo
            string bordereauxIcon = Server.MapPath("~/assets/img/fecha.png");
            var jpg = Image.GetInstance(bordereauxIcon);
            jpg.ScaleToFit(140f, 120f);
            //jpg.SpacingBefore = 10f;
            //jpg.SpacingAfter = 17f;
            //jpg.Alignment = Element.ALIGN_LEFT;
            jpg.SetAbsolutePosition(90f, PageSize.A4.Height - 135f);
            doc.Add(jpg);

            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(fecha.Show._Show + " en " + fecha.Sala.Direccion.Ciudad, FontFactory.GetFont(FontFactory.COURIER_BOLD, 12)));
            doc.Add(new Paragraph("Dirección:  " + fecha.Sala.Direccion.Direccion +  ", " + fecha.Sala.Direccion.Ciudad, FontFactory.GetFont(FontFactory.COURIER_BOLD, 12)));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(fecha.Sala.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));

            /*

            var arreglo = new PdfPTable(2);
            arreglo.DefaultCell.Padding = 3;
            arreglo.SetWidths(new int[] { 10, 10 });
            arreglo.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            arreglo.HeaderRows = 0;
            arreglo.DefaultCell.BorderWidth = 0;
            arreglo.DefaultCell.BackgroundColor = BaseColor.WHITE;
            arreglo.AddCell(new Phrase(fecha.Show._Show + " - " + fecha.Show.Nombre, FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            arreglo.CompleteRow();

            arreglo.CompleteRow();
            doc.Add(arreglo);
            doc.Add(new Paragraph(" "));
            */
            doc.Close();
            Process.Start(filePath);

            //descargo el archivo
            var di = new DirectoryInfo(filePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(di.FullName);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            Response.Redirect(fileName);

            new LogService().FormatAndSaveLog("Fecha", "InfoFecha", fecha.UniqueId);

        }

        private void AgregarCabecera(Document doc, Fecha fecha)
        {
            //Logo
            string bordereauxIcon = Server.MapPath("~/assets/img/bordereaux.png");
            var jpg = Image.GetInstance(bordereauxIcon);
            jpg.ScaleToFit(140f, 120f);
            jpg.SpacingBefore = 10f;
            jpg.SpacingAfter = 17f;
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);

            doc.Add(new Paragraph(" "));
            var arreglo = new PdfPTable(2);
            arreglo.DefaultCell.Padding = 3;
            arreglo.SetWidths(new int[] { 10, 10 });
            arreglo.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            arreglo.HeaderRows = 0;
            arreglo.DefaultCell.BorderWidth = 0;
            arreglo.DefaultCell.BackgroundColor = BaseColor.WHITE;
            arreglo.AddCell(new Phrase(fecha.Show._Show + " - " + fecha.Show.Nombre, FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            arreglo.AddCell(new Phrase(fecha.Sala.Nombre, FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            arreglo.CompleteRow();
            arreglo.AddCell(new Phrase(fecha.FechaHorario.ToString("dd/MM/yyyy HH:mm") + "hs", FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            arreglo.AddCell(new Phrase(fecha.Sala.Direccion.Ciudad, FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));

            arreglo.CompleteRow();
            doc.Add(arreglo);
            doc.Add(new Paragraph(" "));
        }
        private void AgregarEntradas(Document doc, Bordereaux bordereaux)
        {
            PdfPTable table = new PdfPTable(4);
            table.DefaultCell.Padding = 3;

            table.SetWidths(new int[] { 20, 10, 10, 10 });
            table.DefaultCell.BorderWidth = .2f;
            table.DefaultCell.BackgroundColor = new BaseColor(158, 198, 229);
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(new Phrase("Entrada", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Cantidad", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Precio", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Total", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;


            table.HeaderRows = 1;
            table.DefaultCell.BackgroundColor = BaseColor.WHITE;
            foreach (var item in bordereaux.Entradas)
            {
                table.AddCell(new Phrase(item.Nombre, FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase(item.Cantidad.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase("$" + item.Precio.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase("$" + item.Total.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));

                table.CompleteRow();
            }
            doc.Add(table);

            var totales = new PdfPTable(2);
            totales.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            totales.DefaultCell.Padding = 3;
            totales.SetWidths(new int[] { 10, 10 });
            totales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totales.HeaderRows = 0;
            totales.DefaultCell.BorderWidth = .2f;
            totales.AddCell(new Phrase("Total: " + bordereaux.EntradasTotal.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.AddCell(new Phrase("Bruto: $" + bordereaux.EntradasBruto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.CompleteRow();
            doc.Add(totales);
        }
        private void AgregarImpuestos(Document doc, Bordereaux bordereaux)
        {
            PdfPTable table = new PdfPTable(4);
            table.DefaultCell.Padding = 3;
            table.SetWidths(new int[] { 25, 6, 10, 25 });
            table.DefaultCell.BackgroundColor = new BaseColor(158, 198, 229);
            table.DefaultCell.BorderWidth = .2f;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(new Phrase("Impuestos/Deducciones", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("%", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("$", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Comentarios", FontFactory.GetFont(FontFactory.COURIER, 10)));

            table.HeaderRows = 1;
            table.DefaultCell.BackgroundColor = BaseColor.WHITE;
            foreach (var item in bordereaux.ImpuestosDeduccionesTeatro)
            {
                table.AddCell(new Phrase(item.Nombre, FontFactory.GetFont(FontFactory.COURIER, 10)));
                if (item.Porcentaje.ToString() == "" || item.Porcentaje.ToString() == "0")
                    table.AddCell(new Phrase("-", FontFactory.GetFont(FontFactory.COURIER, 10)));
                else
                    table.AddCell(new Phrase(item.Porcentaje.ToString() + "%", FontFactory.GetFont(FontFactory.COURIER, 10)));

                table.AddCell(new Phrase("$" + item.Monto.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase(item.Comentarios, FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.CompleteRow();
            }
            doc.Add(table);

            var totales = new PdfPTable(3);
            totales.DefaultCell.Padding = 3;
            totales.SetWidths(new int[] { 10, 10, 10 });
            totales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totales.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            totales.HeaderRows = 0;
            totales.DefaultCell.BorderWidth = .2f;
            totales.AddCell(new Phrase("Bruto: $" + bordereaux.ImpuestosDeduccionesBruto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.AddCell(new Phrase("Deducir: $" + bordereaux.ImpuestosDeduccionesTotalDeducir.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.AddCell(new Phrase("Neto: $" + bordereaux.ImpuestosDeduccionesNeto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.CompleteRow();
            doc.Add(totales);

            doc.Add(new Paragraph(" "));

            var arreglo = new PdfPTable(2);
            arreglo.DefaultCell.Padding = 3;
            arreglo.SetWidths(new int[] { 10, 10 });
            arreglo.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            arreglo.HeaderRows = 0;
            arreglo.DefaultCell.BorderWidth = .2f;
            arreglo.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

            if (bordereaux.ArregloFijo)
            {
                arreglo.AddCell(new Phrase("Teatro fijo: $" + bordereaux.ImpuestosDeduccionesTeatroMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.AddCell(new Phrase("SUA Neto: $" + bordereaux.ImpuestosDeduccionesCompanyMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.CompleteRow();
            }
            else
            {
                arreglo.AddCell(new Phrase("Teatro: " + bordereaux.ImpuestosDeduccionesTeatroPorcentaje.ToString() + "%", FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.AddCell(new Phrase("Neto: $" + bordereaux.ImpuestosDeduccionesTeatroMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.CompleteRow();
                arreglo.AddCell(new Phrase("SUA: " + bordereaux.ImpuestosDeduccionesCompanyPorcentaje.ToString() + "%", FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.AddCell(new Phrase("Neto: $" + bordereaux.ImpuestosDeduccionesCompanyMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
                arreglo.CompleteRow();
            }
            doc.Add(arreglo);
        }
        private void AgregarGastos(Document doc, Bordereaux bordereaux)
        {
            PdfPTable table = new PdfPTable(3);
            table.DefaultCell.Padding = 3;

            table.SetWidths(new int[] { 25, 10, 25 });
            table.DefaultCell.BackgroundColor = new BaseColor(158, 198, 229);
            table.DefaultCell.BorderWidth = .2f;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(new Phrase("Gasto", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Monto", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.AddCell(new Phrase("Detalle", FontFactory.GetFont(FontFactory.COURIER, 10)));
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;


            table.HeaderRows = 1;
            table.DefaultCell.BackgroundColor = BaseColor.WHITE;
            foreach (var item in bordereaux.GastosCompany)
            {
                table.AddCell(new Phrase(item.Gasto, FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase("$" + item.Monto.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                table.AddCell(new Phrase(item.Detalle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));

                table.CompleteRow();
            }
            doc.Add(table);

            var totales = new PdfPTable(2);
            totales.DefaultCell.Padding = 3;
            totales.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            totales.SetWidths(new int[] { 10, 10 });
            totales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totales.HeaderRows = 0;
            totales.AddCell(new Phrase("Total: $" + bordereaux.GastosCompanyTotal.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.AddCell(new Phrase("Neto: $" + bordereaux.GastosCompanyNeto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 10)));
            totales.CompleteRow();
            doc.Add(totales);
        }
        private void Separador(Document doc)
        {
            var p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.GRAY, Element.ALIGN_LEFT, 1)));
            doc.Add(p);
        }
        private void AgregarTotales(Document doc, Bordereaux bordereaux)
        {
            var totales = new PdfPTable(2);
            totales.DefaultCell.Padding = 3;
            totales.SetWidths(new int[] { 10, 10 });
            totales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totales.HeaderRows = 0;
            totales.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            totales.DefaultCell.BorderWidth = 1;
            totales.AddCell(new Phrase("Show: " + bordereaux.ShowPorcentaje.ToString() + "%", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("Neto: $" + bordereaux.ShowMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.CompleteRow();
            totales.AddCell(new Phrase("SUA: " + bordereaux.SUAPorcentaje.ToString() + "%", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("Neto: $" + bordereaux.SUAMonto.ToString(), FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.CompleteRow();
            doc.Add(totales);
        }
        private void AgregaFirmas(Document doc)
        {
            var totales = new PdfPTable(3);
            totales.DefaultCell.Padding = 3;
            totales.SetWidths(new int[] { 50, 50, 50 });
            totales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totales.HeaderRows = 0;
            totales.DefaultCell.BorderWidth = 0;
            totales.AddCell(new Phrase("----------------", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("----------------", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("----------------", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.CompleteRow();
            totales.AddCell(new Phrase("Firma y aclaración comediante", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("Firma y aclaración Diego", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.AddCell(new Phrase("Firma y aclaración SUA", FontFactory.GetFont(FontFactory.COURIER_BOLD, 11)));
            totales.CompleteRow();
            doc.Add(totales);
        }

        private List<Show> SimplifyShowsList(List<Show> shows)
        {
            var showsSimplfy = new List<Show>();
            foreach (var show in shows)
            {
                show.Integrantes = null;
                show.Productor.Direccion = null;
                show.Productor.DatosBancarios = null;
                show.Productor.DireccionFacturacion = null;
                showsSimplfy.Add(show);
            }
            return showsSimplfy;
        }

        private string GenerateBodyEmail(Fecha fecha)
        {
            return "";
        }
    }
}