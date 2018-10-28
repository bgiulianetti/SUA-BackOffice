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
                    if(user.MustChangePasswordAtNextLogin == "si")
                    {
                        return RedirectToAction("SetNewPassword", "User", new { id = user.UniqueId});
                    }

                    Response.Cookies["session"].Value = username;
                    new LogService().FormatAndSaveLog("Login", "Login", "");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception ex)
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

        [HttpGet]
        public ActionResult GenerateBackup()
        {

            var entidades = new Dictionary<string, string>();
            try
            {
                entidades.Add("standuperos", JsonConvert.SerializeObject(new StanduperoService().GetStanduperos()));
                entidades.Add("productores", JsonConvert.SerializeObject(new ProductorService().GetProductores()));
                entidades.Add("shows", JsonConvert.SerializeObject(new ShowService().GetShowsForBackUp()));
                entidades.Add("fechas", JsonConvert.SerializeObject(new FechaService().GetFechasForBackUp()));
                entidades.Add("usuarios", JsonConvert.SerializeObject(new UserService().GetUsers()));
                entidades.Add("salas", JsonConvert.SerializeObject(new SalaService().GetSalas()));
                entidades.Add("restaurantes", JsonConvert.SerializeObject(new RestauranteService().GetRestaurantes()));
                entidades.Add("logs", JsonConvert.SerializeObject(new LogService().GetLogs()));
                entidades.Add("hoteles", JsonConvert.SerializeObject(new HotelService().GetHoteles()));

                foreach (var entidad in entidades)
                {
                    System.IO.File.WriteAllText(Server.MapPath("~/BackUp/" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + entidad.Key + ".txt"), entidad.Value);
                }
                ViewBag.mensaje = "Backup Generado con éxito";
            }
            catch(Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult GenerateRestore(string date)
        {
            var entidades = new List<string> { "standuperos", "productores", "shows" , "fechas", "usuarios", "salas", "restaurantes", "logs", "hoteles"};
            try
            {
                foreach (var entidad in entidades)
                {
                    var json = System.IO.File.ReadAllText(Server.MapPath("~/BackUp/" + date + "_" + entidad + ".txt"));
                    if(entidad == "standuperos")
                    {
                        var standuperos = JsonConvert.DeserializeObject<Standupero[]>(json);
                        if(standuperos.Length > 0)
                            new StanduperoService().AddBulkStandupero(standuperos.ToList());
                    }
                    else if (entidad == "productores")
                    {
                        var productores = JsonConvert.DeserializeObject<Productor[]>(json);
                        if (productores.Length > 0)
                            new ProductorService().AddBulkProductor(productores.ToList());
                    }
                    else if (entidad == "shows")
                    {
                        var shows = JsonConvert.DeserializeObject<Show[]>(json);
                        if (shows.Length > 0)
                            new ShowService().AddBulkShow(shows.ToList());
                    }
                    else if (entidad == "fechas")
                    {
                        var fechas = JsonConvert.DeserializeObject<Fecha[]>(json);
                        if (fechas.Length > 0)
                            new FechaService().AddBulkFecha(fechas.ToList());
                    }
                    else if (entidad == "usuarios")
                    {
                        var usuarios = JsonConvert.DeserializeObject<UserModel[]>(json);
                        if (usuarios.Length > 0)
                            new UserService().AddBulkUser(usuarios.ToList());
                    }
                    else if (entidad == "salas")
                    {
                        var salas = JsonConvert.DeserializeObject<Sala[]>(json);
                        if (salas.Length > 0)
                            new SalaService().AddBulkSala(salas.ToList());
                    }
                    else if (entidad == "restaurantes")
                    {
                        var restaurantes = JsonConvert.DeserializeObject<Restaurante[]>(json);
                        if (restaurantes.Length > 0)
                            new RestauranteService().AddBulkRestaurante(restaurantes.ToList());
                    }
                    else if (entidad == "logs")
                    {
                        var logs = JsonConvert.DeserializeObject<Log[]>(json);
                        if (logs.Length > 0)
                            new LogService().AddBulkLog(logs.ToList());
                    }
                    else if (entidad == "hoteles")
                    {
                        var hoteles = JsonConvert.DeserializeObject<Hotel[]>(json);
                        if (hoteles.Length > 0)
                            new HotelService().AddBulkHotel(hoteles.ToList());
                    }
                }
                ViewBag.mensaje = "Backup Generado con éxito";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

    }
}