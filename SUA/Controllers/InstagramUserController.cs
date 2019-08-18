﻿using Newtonsoft.Json;
using SUA.Filters;
using SUA.Models;
using SUA.Servicios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class InstagramUserController : Controller
    {
        [UserValidationFilter]
        public ActionResult InstagramUsers(string status)
        {
            if (status == "full")
                ViewBag.followersCount = 1000;
            else
                ViewBag.followersCount = 10;
            try
            {
                //var service = new InstagramUserService();
                var users = GetInstagramUsers();
                var SUAUsers = GetSUAInstagramUsers();
                ViewBag.standuperosSUAFollowersLast = FormatInstagramUsersForBarChart(SUAUsers);
                ViewBag.standuperosSUAFollowersLegacy = FormatInstagramUsersFollowersForSplineChart(SUAUsers, true, "legacy");


                //table sua users
                ViewBag.standuperosSUAFollowersTable = SUAUsers;
                ViewBag.standuperosSUAUsernames = GetStanduperosSUAUsernameOrderByUsername();
                ViewBag.standuperosSUAFollowersActual = FormatInstagramUsersFollowersForSplineChart(SUAUsers, true);


                ViewBag.standuperosRanking = FormatInstagramUsersForHorizontalBarChart(users);

                //table users
                ViewBag.standuperosFollowersTable = users;
                ViewBag.standuperosUsernames = GetStanduperosUsernameOrderByUsername();
                ViewBag.standuperosFollowersActual = FormatInstagramUsersFollowersForSplineChart(users, true);
                ViewBag.message = "";
                ViewBag.titulo = "Instagram - Estadísticas";

                //Rankings
                var ranking = GetUsersForDifferenceRanking();
                ViewBag.rankingWeekly = ranking.OrderByDescending(f=>f.Weekly).ToList();
                ViewBag.rankingMonthly = ranking.OrderByDescending(f => f.Monthly).ToList();
                ViewBag.rankingSemiannually = ranking.OrderByDescending(f => f.SemiAnnually).ToList();
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }

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
            return lista.OrderBy(f => f).ToList();
        }

        private List<string> GetStanduperosUsernameOrderByUsername()
        {
            var lista = new List<string>();
            var users = GetInstagramUsers();
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
            users = users.OrderBy(f => f.Username).ToList();
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
            users = users.OrderBy(f => f.Followers.First().Count).ToList();
            var lista = new List<HorizontalBarChartDataContract>();
            var ranking = users.Count;
            foreach (var user in users)
            {
                lista.Add(new HorizontalBarChartDataContract { label = "#" + ranking + " - " + user.Username, posts = user.Posts, seguidos = user.Following, y = user.Followers.First().Count, url = user.ProfilePicture });
                ranking--;
            }
            return lista;
        }

        [HttpGet]
        public ActionResult InitializeInstagramUsers()
        {
            try
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
                ViewBag.message = "";
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateInstagramUsersPicture()
        {
            var instagramService = new InstagramService();
            try
            {
                var users = new List<string>
                {
                    "agusbattioni",
                    "angiesammartino",
                    "chapumartinez",
                    "connieballarini",
                    "crococro",
                    "danilachepi",
                    "darioorsi",
                    "dieguitomaggio",
                    "elartedenegar",
                    "ezequielcampa",
                    "fedecyrulnik",
                    "fedesimonetti",
                    "fermetilli",
                    "fersanjiao",
                    "ffrangomez",
                    "gonzovizan",
                    "gregorossello",
                    "javichosoria",
                    "joaquin__castellano",
                    "juampicarbonetti",
                    "juampigon",
                    "julibellese",
                    "lailaroth",
                    "leaigounet",
                    "lendrogh",
                    "lucaslauriente",
                    "lucaslezin",
                    "luchomellera",
                    "magalitajes",
                    "malepichot",
                    "manuelasaiz",
                    "martarresok_",
                    "martinpuglieseok",
                    "martincirio",
                    "mikechouhy",
                    "molinerd",
                    "nachitosaralegui",
                    "nicolasdetracy",
                    "nicombraun",
                    "pablitofabregas",
                    "pablopicotto",
                    "pichipiccirillo",
                    "pipabarbato",
                    "quierostandup",
                    "rodriguezgalati",
                    "soyrada",
                    "standupargentina",
                    "vicuvillanueva",
                    "virsammartino"
                };
                foreach (var user in users)
                {
                    var username = user.Split('*')[0];
                    var userObtenido = instagramService.GetUserBy(username);

                    var instagramUserService = new InstagramUserService();
                    var userFromDB = instagramUserService.GetInstagramUserByUsername(username);
                    userFromDB.ProfilePicture = userObtenido.Picture.AbsoluteUri;
                    instagramUserService.UpdateInstagramUser(userFromDB);
                }
                ViewBag.message = "";
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }
            return View();
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

        private List<InstagramUser> GetInstagramUsers()
        {
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            return users.Where(f => f.Username != "sofimorandi" && f.Username != "belulucius").ToList();
        }

        private List<InstagramUser> GetSUAInstagramUsers()
        {
            var service = new InstagramUserService();
            return service.GetSUAInstagramUsers();
        }

        //No sirve, solo lo use para crear seguidores al azar
        private void CreateFollowersStepOne()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-07-16*427719*up*65",
                "angiesammartino*2019-07-16*159007*up*40",
                "chapumartinez*2019-07-16*1159171*up*90",
                "connieballarini*2019-07-16*192538*up*95",
                "crococro*2019-07-16*70707*up*2",
                "darioorsi*2019-07-16*746436*up*16",
                "dieguitomaggio*2019-07-16*40380*down*3",
                "elartedenegar*2019-07-16*62672*down*2",
                "ezequielcampa*2019-07-16*92098*up*16",
                "fedecyrulnik*2019-07-16*360010*down*8",
                "fedesimonetti*2019-07-16*20557*up*4",
                "fermetilli*2019-07-16*421155*up*145",
                "fersanjiao*2019-07-16*44739*up*5",
                "ffrangomez*2019-07-16*512751*up*200",
                "gonzovizan*2019-07-16*137690*down*15",
                "gregorossello*2019-07-16*1000242*down*20",
                "javichosoria*2019-07-16*18505*up*2",
                "joaquin__castellano*2019-07-16*181571*up*19",
                "juampicarbonetti*2019-07-16*27931*up*4",
                "juampigon*2019-07-16*417485*up*10",
                "julibellese*2019-07-16*131507*up*40",
                "lailaroth*2019-07-16*77863*down*6",
                "leaigounet*2019-07-16*138545*up*20",
                "lendrogh*2019-07-16*9572*up*1",
                "lucaslauriente*2019-07-16*110141*up*3",
                "lucaslezin*2019-07-16*1013330*down*51",
                "luchomellera*2019-07-16*121667*up*8",
                "magalitajes*2019-07-16*1299573*up*110",
                "malepichot*2019-07-16*470430*up*18",
                "manuelasaiz*2019-07-16*25118*up*2",
                "martarresok_*2019-07-16*386754*up*33",
                "martin_pugliese*2019-07-16*61446*up*3",
                "martincirio*2019-07-16*811335*up*165",
                "mikechouhy*2019-07-16*766109*up*60",
                "molinerd*2019-07-16*548596*up*19",
                "nachitosaralegui*2019-07-16*429362*up*90",
                "nicolasdetracy*2019-07-16*450179*up*52",
                "nicombraun*2019-07-16*82281*up*28",
                "pablitofabregas*2019-07-16*80653*up*10",
                "pablopicotto*2019-07-16*191135*up*120",
                "pichipiccirillo*2019-07-16*15832*up*10",
                "pipabarbato*2019-07-16*102390*up*45",
                "quierostandup*2019-07-16*26794*up*2",
                "rodriguezgalati*2019-07-16*712374*up*155",
                "soyrada*2019-07-16*1270113*up*75",
                "standupargentina*2019-07-16*70648*up*6",
                "virsammartino*2019-07-16*12147*up*2"
            };

            foreach (var user in users)
            {
                var followersInfo = user.Split('*').ToList();
                var folowersCountTop = Int32.Parse(followersInfo[2]);
                var lastDate = new DateTime(Int32.Parse(followersInfo[1].Split('-')[0]),
                                            Int32.Parse(followersInfo[1].Split('-')[1]),
                                            Int32.Parse(followersInfo[1].Split('-')[2]));
                bool isDecrement = true;
                if (followersInfo[3] == "up")
                    isDecrement = false;
                var maxDif = Int32.Parse(followersInfo[4]) * 2;


                var instagramUserService = new InstagramUserService();
                var userObtenido = instagramUserService.GetInstagramUserByUsername(followersInfo[0]);
                var followers = userObtenido.Followers.OrderBy(i => i.Date).ToList();
                var currentDate = followers.Last().Date.AddDays(1);

                var pico = 0;
                while (currentDate < lastDate)
                {
                    Thread.Sleep(2);
                    var seed = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second +  DateTime.Now.Millisecond.ToString();
                    var rnd = new Random(Int32.Parse(seed));
                    
                    var dif = rnd.Next(0, maxDif);
                    if (isDecrement)
                        dif = dif * -1;

                    if(pico == 6)
                    {
                        dif = dif * 20;
                        pico = 0;
                    }
                    else if(pico == 3)
                    {
                        dif = dif * 5;
                    }

                    var followerItem = new InstragramUserFollowersHistory
                    {
                        Count = followers.Last().Count + dif,
                        Date = currentDate,
                        Difference = dif
                    };
                    userObtenido.Followers.Add(followerItem);
                    followers = userObtenido.Followers.OrderBy(i => i.Date).ToList();
                    currentDate = currentDate.AddDays(1);
                    pico++;
                }

                var lastFollowerItem = new InstragramUserFollowersHistory
                {
                    Count = Int32.Parse(followersInfo[2]),
                    Date = lastDate,
                    Difference = Int32.Parse(followersInfo[2]) - followers.Last().Count
                };
                userObtenido.Followers.Add(lastFollowerItem);
                instagramUserService.UpdateInstagramUser(userObtenido);
            }
        }

        [HttpGet]
        public ActionResult FixIG()
        {
            FixMartin();
            //CreateFollowersStepOne();
            //CreateFollowersStepTwo();

            return RedirectToAction("InstagramUsers");
        }

        private void FixMartin()
        {
            var instagramUserService = new InstagramUserService();
            var userObtenido = instagramUserService.GetInstagramUserByUsername("martin_pugliese");
            userObtenido.Username = "martinpuglieseok";
            var last = userObtenido.Followers.OrderByDescending(f => f.Date).First();
            var dateToAsign = last.Date.AddDays(1);
            while (last.Date.Day < DateTime.Now.AddDays(-1).Day)
            {
                //genero un numero al azar
                Thread.Sleep(2);
                var seed = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second + DateTime.Now.Millisecond.ToString();
                var rnd = new Random(Int32.Parse(seed));
                var dif = rnd.Next(6, 17);

                //creo el item
                var followerItem = new InstragramUserFollowersHistory
                {
                    Count = last.Count + dif,
                    Date = dateToAsign,
                    Difference = dif
                };
                userObtenido.Followers.Add(followerItem);
                last = followerItem;
                dateToAsign = dateToAsign.AddDays(1);
            }
            instagramUserService.UpdateInstagramUser(userObtenido);
        }

        private List<InstagramUserRankingPeriod> GetUsersForDifferenceRanking()
        {
            var rankingUsers = new List<InstagramUserRankingPeriod>();
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            users = users.Where(f => f.Username != "sofimorandi" && f.Username != "belulucius").ToList();
            foreach (var user in users)
            {
                var weekly = GenerateAverageDailyFollowers(user.Followers.OrderByDescending(f => f.Date).ToList(), 7);
                var monthly = GenerateAverageDailyFollowers(user.Followers.OrderByDescending(f => f.Date).ToList(), 30);
                var semiannually = GenerateAverageDailyFollowers(user.Followers.OrderByDescending(f => f.Date).ToList(), 182);

                var rankingUser = new InstagramUserRankingPeriod
                {
                    Username = user.Username,
                    ProfilePicture = user.ProfilePicture,
                    Weekly = weekly,
                    Monthly = monthly,
                    SemiAnnually = semiannually
                };
                rankingUsers.Add(rankingUser);
            }
            return rankingUsers;
        }

        private int GenerateAverageDailyFollowers(List<InstragramUserFollowersHistory> followers, int days)
        {
            if (followers.Count < days)
                return -9999999;

            int sum = 0;
            for(var i = 0; i < days; i++)
                sum += followers[i].Difference;
            return sum / days;
        }
    }
}