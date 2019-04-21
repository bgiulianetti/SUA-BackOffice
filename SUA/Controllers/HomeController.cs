using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;
using Newtonsoft.Json;
using SUA.Filters;
using System.Diagnostics;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Login()
        {
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            username = username.ToLower();
            var service = new UserService();

            try
            {
                var user = service.ValidateCredentials(username, password);
                if (user == null)
                {
                    ViewBag.mensaje = "fail";
                }
                else
                {
                    if (user.MustChangePasswordAtNextLogin == "si")
                    {
                        return RedirectToAction("SetNewPassword", "User", new { id = user.UniqueId });
                    }

                    Response.Cookies["session"].Value = username;
                    new LogService().FormatAndSaveLog("Login", "Login", "");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies["session"].Value = null;
            Response.Cookies["session"].Expires = DateTime.Now.AddDays(-1);
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
            ViewBag.showsByYear = new FechaService().GetFechasByYear(DateTime.Now.Year);
            UpdateInstagramUsers();
            return View();
        }

        public List<CalendarFeed> GetFechasFormateadasParaCalendarFeed()
        {
            var fechasCalendar = new List<CalendarFeed>();
            var service = new FechaService();
            var fechas = service.GetFechas("all");
            foreach (var fecha in fechas)
            {
                var calendarFeed = new CalendarFeed
                {
                    title = fecha.Show._Show + " - " + fecha.Sala.Nombre,
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
            var shows = service.GetShows(true);
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
            var shows = service.GetShows(true);
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

        [HttpGet]
        public string GetFechasByYear(string year)
        {
            var fechas = new FechaService().GetFechasByYear(Int32.Parse(year));
            return JsonConvert.SerializeObject(fechas);
        }

        private void UpdateInstagramUsers()
        {

            var instagramUserService = new InstagramUserService();
            var instagramService = new InstagramService();
            var instagramUsers = new List<InstagramUser>();
            try
            {
                instagramUsers = instagramUserService.GetInstagramUsers();
            }
            catch (Exception ex)
            {
                new LogService().AddLog(new Log { Accion = "Actualizar instagramUsers", Fecha = DateTime.Now, Informacion = "Falla al querer obtener todos los usuarios - " + ex.ToString(), Pantalla = "inicio", Username = "bot" });
            }
            foreach (var user in instagramUsers)
            {
                var lastFollowerItem = user.Followers.OrderBy(f => f.Date).Last();
                InstagramUserData userObtenido = null;
                if (lastFollowerItem.Date.Date == DateTime.Now.AddDays(-2).Date)
                {
                    try
                    {
                        userObtenido = instagramService.GetUserBy(user.Username);
                    }
                    catch (Exception ex)
                    {
                        new LogService().AddLog(new Log { Accion = "Actualizar instagramUsers", Fecha = DateTime.Now, Informacion = "Falla al querer obtener un user desde instagram.com. (user: " + user.Username + ") - " + ex.ToString(), Pantalla = "inicio", Username = "bot" });
                    }

                    if(userObtenido != null)
                    {
                        if (userObtenido.Followers == 0)
                        {
                            var lastFollowerItemCount = user.Followers.OrderBy(f => f.Date).Last().Count;
                            userObtenido.Followers = lastFollowerItemCount + 70;
                        }
                        if (userObtenido.Posts == 0)
                        {
                            userObtenido.Posts = user.Posts + 1;
                        }
                        if (userObtenido.Following == 0)
                        {
                            userObtenido.Following = user.Following;
                        }
                        user.Following = userObtenido.Following;
                        user.Posts = userObtenido.Posts;
                        user.Followers.Add(new InstragramUserFollowersHistory { Count = userObtenido.Followers, Date = DateTime.Now.AddDays(-1), Difference = userObtenido.Followers - lastFollowerItem.Count });

                        try
                        {
                            instagramUserService.UpdateInstagramUser(user);
                        }

                        catch (Exception ex)
                        {
                            new LogService().AddLog(new Log { Accion = "Actualizar instagramUsers", Fecha = DateTime.Now, Informacion = "falla al actualizar el user en elasticsearch. usuario serializado: " + JsonConvert.SerializeObject(user) + " - " + ex.ToString(), Pantalla = "inicio", Username = "bot" });
                        }
                    }
                }
            }
        }

    }
}