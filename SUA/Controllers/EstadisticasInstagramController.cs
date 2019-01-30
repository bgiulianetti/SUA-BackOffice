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
        public ActionResult InstagramStats()
        {
            var service = new InstagramService();
            var instagramUsers = service.GetUsers();
            ViewBag.standuperosSUA = GetInstagramUsersForChart(instagramUsers);
            return View();
        }


        public List<ChartInfoContract> GetInstagramUsersForChart(List<InstagramUserData> users)
        {
            var lista = new List<ChartInfoContract>();
            foreach (var user in users)
            {
                lista.Add(new ChartInfoContract { y = user.Followers, label = user.InstagramUser/* + "\nSeguidos: " + user.Following + "\nPosts: " + user.Posts*/});
            }
            return lista.OrderByDescending(f => f.y).ToList();
        }
    }
}