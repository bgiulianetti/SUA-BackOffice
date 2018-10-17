using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Usuario(string id)
        {
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.estados = UtilitiesAndStuff.GetEstados();
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
        public ActionResult Usuarios()
        {
            if (Request.Cookies["session"] == null)
                return RedirectToAction("Login", "Home");

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


    }
}