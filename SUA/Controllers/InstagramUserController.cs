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
    public class InstagramUserController : Controller
    {

        public ActionResult StanduperosSUARaning()
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
                lista.Add(new ChartInfoContract { y = user.Followers, label = user.InstagramUser });
            }
            return lista.OrderByDescending(f => f.y).ToList();
        }

        [HttpGet]
        public void AddInstagramUsers()
        {
            var instagramService = new InstagramService();
            var json = System.IO.File.ReadAllText(Server.MapPath("~/assets/InstagramFollowersHistory/sergudiores.json"));
            var users = JsonConvert.DeserializeObject<InstagramUserDataContract[]>(json);

            foreach (var user in users)
            {
                var userObtenido = instagramService.GetUserBy(user.user);
                var instagramUser = new InstagramUser
                {
                    Username = user.user,
                    Following = userObtenido.Following,
                    ProfilePicture = userObtenido.Picture.AbsolutePath,
                    Posts = userObtenido.Posts,
                    Followers = CreateUserFollowersHistory(user)
                };
            }
        }

        private List<InstragramUserFollowersHistory> CreateUserFollowersHistory(InstagramUserDataContract user)
        {
            var dateArray = user.pointStart.Split(',');
            var date = new DateTime(Int32.Parse(dateArray[0]), Int32.Parse(dateArray[1]), Int32.Parse(dateArray[2]));
            var history = new List<InstragramUserFollowersHistory>();
            foreach (var item in user.data)
            {
                history.Add(new InstragramUserFollowersHistory { Count = item, Date = date });
                date = date.AddDays(1);
            }
            return history;
        }
    }
}