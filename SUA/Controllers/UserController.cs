using Newtonsoft.Json;
using SUA.Filters;
using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Usuario(string id)
        {
            ViewBag.estados = UtilitiesAndStuff.GetEstados();
            ViewBag.permisos = UtilitiesAndStuff.GetPermisos();
            ViewBag.mensaje = "Get";

            var showService = new ShowService();
            var shows = showService.GetShows();
            ViewBag.userMaster = "no";

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.shows = shows;
                ViewBag.accion = "Post";
                ViewBag.titulo = "Crear Usuario";
            }
            else
            {
                var userService = new UserService();
                var user = userService.GetUserById(id);
                if(user.Username == "sua-user")
                {
                    var userLogueado = Request.Cookies["session"].Value;
                    if(userLogueado != "sua-user")
                    {
                        return RedirectToAction("Usuarios", "User");
                    }
                }


                if (user.UserMaster == "si")
                    ViewBag.userMaster = "si";

                foreach (var item in user.ShowsAsignados)
                {
                    if (shows.Contains(item))
                        shows.Remove(item);
                }

                ViewBag.username = user.Username;
                ViewBag.shows = shows;
                ViewBag.accion = "Put";
                ViewBag.titulo = "Editar Usuario";
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Usuario(UserModel user, string accion, string _shows)
        {
            ViewBag.titulo = "Crear Show";
            user.ShowsAsignados = GetShowsByIds(_shows);
            user.Username = user.Username.ToLower();
            var service = new UserService();
            try
            {
                if (user.Username == "sua-user")
                    user.UserMaster = "si";
                if (string.Equals(accion, "Post"))
                {
                    user.SetId();
                    service.AddUser(user);
                    ViewBag.mensaje = "creado";

                    var userCopia = user;
                    userCopia.Password = "xxxxx";
                    new LogService().FormatAndSaveLog("Usuario", "Crear", JsonConvert.SerializeObject(userCopia));
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateUser(user);
                    ViewBag.mensaje = "actualizado";

                    var userCopia = user;
                    userCopia.Password = "xxxxx";
                    new LogService().FormatAndSaveLog("Usuario", "Editar", JsonConvert.SerializeObject(userCopia));

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
        public ActionResult Usuarios()
        {
            ViewBag.titulo = "Usuarios";
            var service = new UserService();
            try
            {
                var users = service.GetUsers();
                ViewBag.users = users;
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Recover()
        {
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public ActionResult Recover(string email)
        {
            if (email.Replace(" ", "") == "")
            {
                ViewBag.mensaje = "El email no es válido";
                return View();
            }

            var service = new UserService();
            var user = service.GetUserByEmail(email);
            if(user == null)
            {
                ViewBag.mensaje = "El email no existe";
                return View();
            }
            else
            {
                user.Password = UtilitiesAndStuff.GenerateUniqueId();
                user.MustChangePasswordAtNextLogin = "si";
                service.UpdateUser(user);
                SendRecoverEmail(user);
                ViewBag.mensaje = "Se ha enviado la contraseña a su casilla de correo";
            }
            return View();
        }

        [HttpGet]
        public ActionResult SetNewPassword(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult SetNewPassword(string id, string password)
        {
            if(password.Replace(" ", "") == "")
            {
                ViewBag.id = id;
                return View();
            }
            var service = new UserService();
            var user = service.GetUserById(id);
            user.Password = password;
            user.MustChangePasswordAtNextLogin = "no";
            user.LastLogin = DateTime.Now;
            service.UpdateUser(user);
            Response.Cookies["session"].Value = user.Username;
            return RedirectToAction("Index", "Home");
        }

        private List<Show> GetShowsByIds(string _shows)
        {
            var shows = new List<Show>();
            if (_shows == "")
                return shows;

            var idList = _shows.Split('-').ToList();
            var showService = new ShowService();
            foreach (var item in idList)
            {
                shows.Add(showService.GetShowById(item));
            }
            return shows;
        }

        public void SendRecoverEmail(UserModel user)
        {
            var emailService = new EmailService();
            string emailBody = "Hola " + user.Username + "!" + Environment.NewLine + Environment.NewLine +
                          "Se ha recibido la solicitud para un cambio de contraseña de tu cuenta." + Environment.NewLine +
                          "Contraseña: " + user.Password + Environment.NewLine +
                          "Se le solicitará que la cambie al siguiente login." + Environment.NewLine +
                          "Puede acceder al BackOffice desde la siguiente url: " + 
                          System.Configuration.ConfigurationManager.AppSettings.Get("MyUrl") + "/login" + 
                          Environment.NewLine + Environment.NewLine +
                          "Saludos del equipo de SUA!";
            emailService.SendEmail(user.MailRecover, "SUA BackOffice - Recuperar Contraseña", emailBody);
        }

        public ActionResult InicializarUserMaster()
        {
            var user = new UserModel
            {
                Blocked = "no",
                Bordereaux = "Escritura",
                Fechas = "Escritura",
                Hoteles = "Escritura",
                LastLogin = DateTime.Now,
                MailRecover = "diego@standupargentina.com.ar",
                MustChangePasswordAtNextLogin = "no",
                Password = "Sua2018",
                Productores = "Escritura",
                Reportes = "Escritura",
                Restaurantes = "Escritura",
                Salas = "Escritura",
                Shows = "Escritura",
                ShowsAsignados = new List<Show>(),
                Standuperos = "Escritura",
                UniqueId = UtilitiesAndStuff.GenerateUniqueId(),
                UserMaster = "si",
                Username = "sua-user"
            };
            var service = new UserService();
            try
            {
                service.AddUser(user);
                Response.Cookies["session"].Value = user.Username;
                new LogService().FormatAndSaveLog("Login", "Login", "");
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home", new { error = "falla" });
            }

        }

    }
}