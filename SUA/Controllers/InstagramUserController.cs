using Newtonsoft.Json;
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
    public class InstagramUserController : Controller
    {
        public ActionResult InstagramUsers()
        {
            //ver legacy de alguien en particular (spline)
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            ViewBag.standuperosSUAFollowersLast = FormatInstagramUsersForBarChart(service.GetSUAInstagramUsers());
            ViewBag.standuperosSUAFollowersLegacy = FormatInstagramUsersFollowersForSplineChart(service.GetSUAInstagramUsers(), true, "legacy");
            
            
            
            //table sua users
            ViewBag.standuperosSUAFollowersTable = service.GetSUAInstagramUsers();
            ViewBag.standuperosSUAUsernames = GetStanduperosSUAUsernameOrderByUsername();
            ViewBag.standuperosSUAFollowersActual = FormatInstagramUsersFollowersForSplineChart(service.GetSUAInstagramUsers(), true);


            ViewBag.standuperosRanking = FormatInstagramUsersForHorizontalBarChart(service.GetInstagramUsers());
            
            //table users
            ViewBag.standuperosFollowersTable = service.GetInstagramUsers();
            ViewBag.standuperosUsernames = GetStanduperosUsernameOrderByUsername();
            ViewBag.standuperosFollowersActual = FormatInstagramUsersFollowersForSplineChart(service.GetInstagramUsers(), false);

            return View();
        }

        private List<string> GetStanduperosSUAUsernameOrderByUsername()
        {
            var lista = new List<string>();
            var standuperos = new StanduperoService().GetStanduperos();
            foreach (var standupero in standuperos)
            {
                lista.Add(standupero.InstagramUser.Replace("@", "").ToLower());
            }
            return lista.OrderBy(f=>f).ToList();
        }

        private List<string> GetStanduperosUsernameOrderByUsername()
        {
            var lista = new List<string>();
            var users = new InstagramUserService().GetInstagramUsers();
            foreach (var user in users)
            {
                lista.Add(user.Username.ToLower());
            }
            return lista.OrderBy(f => f).ToList();
        }

        public List<ChartInfoContract> FormatInstagramUsersForBarChart(List<InstagramUser> users)
        {
            var lista = new List<ChartInfoContract>();
            foreach (var user in users)
            {
                lista.Add(new ChartInfoContract { y = user.Followers.First().Count, label = user.Username });
            }
            return lista.OrderByDescending(f => f.y).ToList();
        }

        public List<SplineChartDataContract> FormatInstagramUsersFollowersForSplineChart(List<InstagramUser> users, bool ShowLegend, string type = "actual")
        {
            var lista = new List<SplineChartDataContract>();
            foreach (var user in users)
            {
                var splineChartStandupero = new SplineChartDataContract
                {
                    name = user.Username,
                    showInLegend = ShowLegend,
                    type = "spline",
                    yValueFormatString = "#0,### seguidores",
                    visible = true
                };

                var dataPoints = new List<DataPointsSplineContract>();

                var folowers = new List<InstragramUserFollowersHistory>();
                if (type == "legacy")
                    folowers = user.FollowersLegacy;
                else
                    folowers = user.Followers;

                foreach (var datapoint in folowers)
                {
                    dataPoints.Add(new DataPointsSplineContract { x = UtilitiesAndStuff.DateToMiliseconds(datapoint.Date), y = datapoint.Count });
                }
                splineChartStandupero.dataPoints = dataPoints;
                lista.Add(splineChartStandupero);
            }
            return lista;
        }

        public List<HorizontalBarChartDataContract> FormatInstagramUsersForHorizontalBarChart(List<InstagramUser> users)
        {
            users = users.OrderBy(f => f.Followers.Last().Count).ToList();
            var lista = new List<HorizontalBarChartDataContract>();
            var ranking = users.Count;
            foreach (var user in users)
            {
                lista.Add(new HorizontalBarChartDataContract { label = "#" + ranking + " - " + user.Username, posts = user.Posts, seguidos = user.Following, y = user.Followers.Last().Count, url = user.ProfilePicture });
                ranking--;
            }
            return lista;
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
                    FollowersLegacy = CreateUserFollowersHistory(user, "legacy"),
                    Followers = CreateUserFollowersHistory(user, "actual")
                };
                instagramUsers.Add(instagramUser);
            }

            var instagramUserService = new InstagramUserService();
            instagramUserService.AddBulkInstagramUser(instagramUsers);
        }

        private List<InstragramUserFollowersHistory> CreateUserFollowersHistory(InstagramUserDataContract user, string type)
        {
            var history = new List<InstragramUserFollowersHistory>();
            string[] dateArray;
            int[] data;
            if (type == "legacy")
            {
                dateArray = user.pointStartLegacy.Split(',');
                data = user.dataLegacy;
            }
            else
            {
                dateArray = user.pointStart.Split(',');
                data = user.data;
            }

            var date = new DateTime(Int32.Parse(dateArray[0]), Int32.Parse(dateArray[1]), Int32.Parse(dateArray[2]));

            foreach (var item in data)
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