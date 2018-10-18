﻿using SUA.Filters;
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

                if (user.UserMaster == "si")
                    ViewBag.userMaster = "si";

                foreach (var item in user.ShowsAsignados)
                {
                    if (shows.Contains(item))
                        shows.Remove(item);
                }

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
            var service = new UserService();
            try
            {
                if (string.Equals(accion, "Post"))
                {
                    user.SetId();
                    service.AddUser(user);
                    ViewBag.mensaje = "creado";
                }
                else if (string.Equals(accion, "Put"))
                {
                    service.UpdateUser(user);
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
            System.Web.HttpContext.Current.Session["user"] = user;
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
            var userService = new UserService();
            var credenciales = userService.GetEmailCredentials();

            var client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(credenciales.Email, credenciales.Password);
            string body = "Hola " + user.Username + "!" + Environment.NewLine + Environment.NewLine +
                          "Se ha recibido la solicitud para un cambio de contraseña de tu cuenta." + Environment.NewLine +
                          "Contraseña: " + user.Password + Environment.NewLine +
                          "Se le solicitará que la cambie al siguiente login." + Environment.NewLine +
                          "Puede acceder al BackOffice desde la siguiente url: " + System.Configuration.ConfigurationManager.AppSettings.Get("MyUrl") + "/login" + Environment.NewLine  +Environment.NewLine +
                          "Saludos del equipo de SUA!";

            var mm = new MailMessage(credenciales.Email, user.MailRecover, "SUA BackOffice - Recuperar Contraseña", body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
        }

    }
}