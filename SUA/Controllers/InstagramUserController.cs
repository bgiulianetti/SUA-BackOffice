﻿using Newtonsoft.Json;
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
            //Followers legacy (de todos)

            //folowers (ultimos 15 dias) de todos

            //Ranking de todos

            //ultimo mes de followers oficial (en ranknig con diferencias)

            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            ViewBag.standuperosSUAFollowersLast = FormatInstagramUsersForBarChart(GetStanduperosSUA());
            ViewBag.standuperosSUAFollowersLegacy = FormatInstagramUsersForSplineChart(service.GetSUAInstagramUsers());
            ViewBag.standuperosRanking = FormatInstagramUsersForHorizontalBarChart(service.GetInstagramUsers());
            ViewBag.standuperosFollowersLegacy = FormatInstagramUsersForSplineChart(service.GetInstagramUsers());
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


        public List<ChartInfoContract> FormatInstagramUsersForBarChart(List<InstagramUser> users)
        {
            var lista = new List<ChartInfoContract>();
            foreach (var user in users)
            {
                lista.Add(new ChartInfoContract { y = user.Followers.Last().Count, label = user.Username });
            }
            return lista.OrderByDescending(f => f.y).ToList();
        }

        public List<SplineChartDataContract> FormatInstagramUsersForSplineChart(List<InstagramUser> users)
        {
            var lista = new List<SplineChartDataContract>();
            foreach (var user in users)
            {
                var splineChartStandupero = new SplineChartDataContract
                {
                    name = user.Username,
                    showInLegend = true,
                    type = "spline",
                    yValueFormatString = "#0,### seguidores",
                };

                var dataPoints = new List<DataPointsSplineContract>();
                foreach (var datapoint in user.FollowersLegacy)
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

        private List<InstragramUserFollowersHistory> CreateUserFollowersHistory(InstagramUserDataContract user, string dataType)
        {
            var history = new List<InstragramUserFollowersHistory>();
            string[] dateArray;
            int[] data;
            if (dataType == "legacy")
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