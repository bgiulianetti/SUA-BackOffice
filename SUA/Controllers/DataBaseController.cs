using Newtonsoft.Json;
using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class DataBaseController : Controller
    {
        [HttpGet]
        public ActionResult Backup()
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
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Restore(string date)
        {
            var entidades = new List<string> { "standuperos", "productores", "shows", "fechas", "usuarios", "salas", "restaurantes", "logs", "hoteles" };
            try
            {
                foreach (var entidad in entidades)
                {
                    var json = System.IO.File.ReadAllText(Server.MapPath("~/BackUp/" + date + "_" + entidad + ".txt"));
                    if (entidad == "standuperos")
                    {
                        var standuperos = JsonConvert.DeserializeObject<Standupero[]>(json);
                        if (standuperos.Length > 0)
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