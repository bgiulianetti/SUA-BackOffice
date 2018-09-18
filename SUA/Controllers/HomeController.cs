using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.fechas = GetFechasFormateadasParaCalendarFeed();
            ViewBag.titulo = "Inicio";
            return View();
        }


        public List<CalendarFeed> GetFechasFormateadasParaCalendarFeed()
        {
            var fechasCalendar = new List<CalendarFeed>();
            var service = new FechaService();
            var fechas = service.GetFechas();
            foreach (var fecha in fechas)
            {
                var calendarFeed = new CalendarFeed
                {
                    Fecha = fecha.FechaHorario.ToString("yyyy-MM-ddThh:mm"),
                    Productor = fecha.Productor.Nombre + " " + fecha.Productor.Apellido,
                    Sala = fecha.Sala.Nombre,
                    Show = fecha.Show._Show
                };
                fechasCalendar.Add(calendarFeed);
            }
            return fechasCalendar;
        }
    }
}