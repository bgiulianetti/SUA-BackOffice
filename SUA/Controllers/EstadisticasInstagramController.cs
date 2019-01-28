using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class EstadisticasInstagramController : Controller
    {
        // GET: EstadisticasInstagram
        public ActionResult Index()
        {
            var service = new InstagramService();
            var standuperoService = new StanduperoService();
            var standuperos = standuperoService.GetStanduperos();
            var lista = new List<InstagramUserData>();
            foreach (var standupero in standuperos)
            {
                lista.Add(service.GetUser(standupero.InstagramUser.Replace("@", "").Trim()));
            }

            ViewBag.standuperosInstagram = lista;
            return View();
        }
    }
}