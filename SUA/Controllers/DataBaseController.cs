using Newtonsoft.Json;
using SUA.Filters;
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
        [UserValidationFilter]
        public ActionResult Backup(string id)
        {

            var entidades = new Dictionary<string, string>();
            try
            {
                var fileNameList = new List<string>();
                entidades.Add("standuperos", JsonConvert.SerializeObject(new StanduperoService().GetStanduperos()));
                entidades.Add("productores", JsonConvert.SerializeObject(new ProductorService().GetProductores()));
                entidades.Add("shows", JsonConvert.SerializeObject(new ShowService().GetShowsForBackUp()));
                entidades.Add("fechas", JsonConvert.SerializeObject(new FechaService().GetFechasForBackUp()));
                entidades.Add("usuarios", JsonConvert.SerializeObject(new UserService().GetUsers()));
                entidades.Add("salas", JsonConvert.SerializeObject(new SalaService().GetSalas()));
                try
                {
                    entidades.Add("restaurantes", JsonConvert.SerializeObject(new RestauranteService().GetRestaurantes()));
                }
                catch
                { }
               
                entidades.Add("logs", JsonConvert.SerializeObject(new LogService().GetLogs("all")));
                entidades.Add("hoteles", JsonConvert.SerializeObject(new HotelService().GetHoteles()));
                entidades.Add("votaciones", JsonConvert.SerializeObject(new VotacionService().GetVotaciones("true")));

                var directory = "";
                directory = "~/BackUp/" + DateTime.Now.ToString("yyyy-MM-dd");
                System.IO.Directory.CreateDirectory(Server.MapPath(directory));
                foreach (var entidad in entidades)
                {
                    var fileNameFullPath = Server.MapPath(directory + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + entidad.Key + ".txt");
                    System.IO.File.WriteAllText(fileNameFullPath, entidad.Value);
                    fileNameList.Add(fileNameFullPath);
                }
                SendBackupFiles(fileNameList, Server.MapPath(directory), id);
                ViewBag.mensaje = "Backup Generado con éxito";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult BackupVotacion(string id)
        {
            var entidades = new Dictionary<string, string>();
            try
            {
                var fileNameList = new List<string>();
                var votaciones = new List<Votacion>();
                var votacionService = new VotacionService();
                votaciones.AddRange(votacionService.GetVotaciones("true"));

                entidades.Add("votaciones", JsonConvert.SerializeObject(votaciones));

                var directory = "";
                directory = "~/BackUp/" + DateTime.Now.ToString("yyyy-MM-dd");
                System.IO.Directory.CreateDirectory(Server.MapPath(directory));
                foreach (var entidad in entidades)
                {
                    var fileNameFullPath = Server.MapPath(directory + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + entidad.Key + ".txt");
                    System.IO.File.WriteAllText(fileNameFullPath, entidad.Value);
                    fileNameList.Add(fileNameFullPath);
                }
                SendBackupFiles(fileNameList, Server.MapPath(directory), id);
                ViewBag.mensaje = "Backup Generado con éxito";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Restore(string id)
        {
            var date = id;
            var entidades = new List<string> { "standuperos", "productores", "shows", "fechas", "usuarios", "salas", "restaurantes", "logs", "hoteles", "votaciones" };
            try
            {
                foreach (var entidad in entidades)
                {
                    var json = System.IO.File.ReadAllText(Server.MapPath("~/BackUp/" + date + "/" + date + "_" + entidad + ".txt"));
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
                    else if (entidad == "votaciones")
                    {
                        var votaciones = JsonConvert.DeserializeObject<Votacion[]>(json);
                        if (votaciones.Length > 0)
                            new VotacionService().AddBulkVotacion(votaciones.ToList());
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

        private void SendBackupFiles(List<string> backUpFiles, string locationBackUp, string destinatario)
        {
            var userService = new UserService();
            var email = "";
            if (destinatario == "yo")
                email = "bruno.giulianetti@gmail.com";
            else
                email = userService.GetUserByNombre("sua-user").MailRecover;
            var emailService = new EmailService();

            var subject = "SUA BackOffice - BackUp " + DateTime.Now.ToString("yyyy-MM-dd");
            var emailBody = "BackUp completo de la base de datos al " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            emailService.SendEmail(email, subject, emailBody, backUpFiles, false, locationBackUp);
        }
    }
}