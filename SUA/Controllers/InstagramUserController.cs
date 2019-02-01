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

        public ActionResult InstagramUsers()
        {
            //Followers legacy (de todos)

            //Ranking de todos

            //ultimo mes de followers oficial (en ranknig con diferencias)

            
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            ViewBag.standuperosSUA = FormatInstagramUsersForChart(GetStanduperosSUA());
            return View();
        }

        private List<InstagramUser> GetStanduperosSUA()
        {
            var lista = new List<InstagramUser>();
            var standuperos = new StanduperoService().GetStanduperos();
            var instagramUserService = new InstagramUserService();
            foreach (var standupero in standuperos)
            {
                lista.Add(instagramUserService.GetInstagramUserByUsername(standupero.InstagramUser.ToLower()));
            }
            return lista;
        }


        public List<ChartInfoContract> FormatInstagramUsersForChart(List<InstagramUser> users)
        {
            var lista = new List<ChartInfoContract>();
            foreach (var user in users)
            {
                lista.Add(new ChartInfoContract { y = user.Followers.Last().Count, label = user.Username });
            }
            return lista.OrderByDescending(f => f.y).ToList();
        }

        [HttpGet]
        public void InitializeInstagramUsers()
        {
            var instagramUsers = new List<InstagramUser>();
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
                    ProfilePicture = userObtenido.Picture.AbsoluteUri,
                    Posts = userObtenido.Posts,
                    FollowersLegacy = CreateUserFollowersHistory(user)
                };
                instagramUsers.Add(instagramUser);
            }

            var instagramUserService = new InstagramUserService();
            instagramUserService.AddBulkInstagramUser(instagramUsers);
        }

        private List<InstragramUserFollowersHistory> CreateUserFollowersHistory(InstagramUserDataContract user)
        {
            var dateArray = user.pointStart.Split(',');
            var date = new DateTime(Int32.Parse(dateArray[0]), Int32.Parse(dateArray[1]), Int32.Parse(dateArray[2]));
            var history = new List<InstragramUserFollowersHistory>();
            foreach (var item in user.dataLegacy)
            {
                var difference = 0;
                if (history.Count > 0)
                    difference = item - history.Last().Count;
                history.Add(new InstragramUserFollowersHistory { Count = item, Date = date, Difference = difference });
                date = date.AddDays(1);
            }
            return history;
        }
    }
}