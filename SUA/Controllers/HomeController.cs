﻿using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;
using Newtonsoft.Json;
using SUA.Filters;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            var user = Session["user"] as UserModel;
            if (user != null)
                return RedirectToAction("Index", "Home");

            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var service = new UserService();
            var user = service.ValidateCredentials(username, password);
            if(user == null)
            {
                ViewBag.mensaje = "fail";
            }
            else
            {
                if(user.MustChangePasswordAtNextLogin == "si")
                {
                    return RedirectToAction("SetNewPassword", "User", new { id = user.UniqueId});
                }
                System.Web.HttpContext.Current.Session["user"] = user;
                new LogService().FormatAndSaveLog("Login", "Login", "");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        [UserValidationFilter]
        public ActionResult Index()
        {
            var calendarService = new GoogleCalendarService();
            ViewBag.CalendarsFullUrl = "home/GetShowCalendars";
            ViewBag.Key = calendarService.GetCalendarKey();
            ViewBag.showCalendars = GetShowCalendarsList();
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

        [HttpGet]
        public string GetShowCalendars()
        {
            var service = new ShowService();
            var shows = service.GetShows();
            var lista = new List<GoogleCalendarProperties>();
            var listaJson = "";
            foreach (var show in shows)
            {
                lista.Add(
                    new GoogleCalendarProperties
                    {
                        uniqueId = show.UniqueId,
                        show = show.Nombre,
                        googleCalendarId = show.GoogleCalendarId,
                        color = show.BackgroundColorCalendar,
                        textColor = show.TextColorCalendar
                    }
               );
            }
            listaJson = JsonConvert.SerializeObject(lista);
            return listaJson;
        }

        public List<GoogleCalendarProperties> GetShowCalendarsList()
        {
            var service = new ShowService();
            var shows = service.GetShows();
            var lista = new List<GoogleCalendarProperties>();
            foreach (var show in shows)
            {
                lista.Add(
                    new GoogleCalendarProperties
                    {
                        uniqueId = show.UniqueId,
                        show = show.Nombre,
                        googleCalendarId = show.GoogleCalendarId,
                        color = show.BackgroundColorCalendar,
                        textColor = show.TextColorCalendar
                    }
               );
            }
            return lista;
        }

        public List<string> GetNombresDeShow()
        {
            var showService = new ShowService();
            var shows = showService.GetShows();
            var lista = new List<string>();
            foreach (var show in shows)
            {
                lista.Add(show.Nombre);
            }
            return lista;
        }

    }
}