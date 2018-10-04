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
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.Cookies["session"] != null)
                return RedirectToAction("Index", "Home");

            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username.ToString() == "sua-user" && password == "sua2018")
            {
                Response.Cookies["session"].Value = username;
                Response.Cookies["session"].Expires = DateTime.Now.AddDays(5);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mensaje = "fail";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies["session"].Value = null;
            Response.Cookies["session"].Expires = DateTime.Now.AddDays(-1);
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            var calendarService = new GoogleCalendarService();
            ViewBag.CalendarsFullUrl = System.Configuration.ConfigurationManager.AppSettings.Get("CalendarsFullUrl");
            ViewBag.Key = calendarService.GetCalendarKey();
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
                    title = fecha.Show._Show +  " - " + fecha.Sala.Nombre,
                    start = fecha.FechaHorario.ToString("yyyy-MM-ddTHH:mm")
                };
                fechasCalendar.Add(calendarFeed);
            }
            return fechasCalendar;
        }

    }
}