using Newtonsoft.Json;
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
        public ActionResult InstagramUsers()
        {
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

        [HttpGet]
        public void CreateFollowers()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-07-16*427719*",
                "angiesammartino*2019-07-16*159007*",
                "chapumartinez*2019-07-16*1159171*",
                "connieballarini*2019-07-16*192538*",
                "crococro*2019-07-16*70707*",
                "darioorsi*2019-07-16*746436*",
                "dieguitomaggio*2019-07-16*40380*",
                "elartedenegar*2019-07-16*62672*",
                "ezequielcampa*2019-07-16*92098*",
                "fedecyrulnik*2019-07-16*360010*",
                "fedesimonetti*2019-07-16*20557*",
                "fermetilli*2019-07-16*421155*",
                "fersanjiao*2019-07-16*44739",
                "ffrangomez*2019-07-16*512751",
                "gonzovizan*2019-07-16*137690",
                "gregorossello*2019-07-16*1000242",
                "javichosoria*2019-07-16*18505",
                "joaquin__castellano*2019-07-16*181571",
                "juampicarbonetti*2019-07-16*27931",
                "juampigon*2019-07-16*417485",
                "julibellese*2019-07-16*131507",
                "lailaroth*2019-07-16*77863",
                "leaigounet*2019-07-16*138545",
                "lendrogh*2019-07-16*9572",
                "lucaslauriente*2019-07-16*110141",
                "lucaslezin*2019-07-16*1013330",
                "luchomellera*2019-07-16*121667",
                "magalitajes*2019-07-16*1299573",
                "malepichot*2019-07-16*470430",
                "manuelasaiz*2019-07-16*25118",
                "martarresok_*2019-07-16*386754",
                "martin_pugliese*2019-07-16*61446",
                "martincirio*2019-07-16*811335",
                "mikechouhy*2019-07-16*766109",
                "molinerd*2019-07-16*548596",
                "nachitosaralegui*2019-07-16*429362",
                "nicolasdetracy*2019-07-16*450179",
                "nicombraun*2019-07-16*82281",
                "pablitofabregas*2019-07-16*80653",
                "pablopicotto*2019-07-16*191135",
                "pichipiccirillo*2019-07-16*15832",
                "pipabarbato*2019-07-16*102390",
                "quierostandup*2019-07-16*26794",
                "rodriguezgalati*2019-07-16*712374",
                "soyrada*2019-07-16*1270113",
                "standupargentina*2019-07-16*70648",
                "virsammartino*2019-07-16*12147"
            };

            foreach (var user in users)
            {
                var instagramUserService = new InstagramUserService();
                var userObtenido = instagramUserService.GetInstagramUserByUsername(user.Split('*')[0]);

                var folowersCountTop = Int32.Parse(user.Split('*').ToList()[2]);
                var lastDate = new DateTime(Int32.Parse(user.Split('*').ToList()[1].Split('-')[0]), 
                                            Int32.Parse(user.Split('*').ToList()[1].Split('-')[1]), 
                                            Int32.Parse(user.Split('*').ToList()[1].Split('-')[2]));
                var followers = userObtenido.Followers.OrderBy(i => i.Date).ToList();
                var currentDate = followers.Last().Date;
                var rnd = new Random();
                int count;
                while (currentDate < lastDate)
                {
                    Thread.Sleep(500);
                    if (followers.Last().Count < folowersCountTop)
                        count = rnd.Next(followers.Last().Count, folowersCountTop);
                    else
                        count = rnd.Next(folowersCountTop, followers.Last().Count);

                    var followerItem = new InstragramUserFollowersHistory
                    {
                        Count = count,
                        Date = followers.Last().Date.AddDays(1),
                        Difference = count - followers.Last().Count
                    };
                    userObtenido.Followers.Add(followerItem);
                    followers = userObtenido.Followers.OrderBy(i => i.Date).ToList();
                    currentDate = followers.Last().Date;
                }
                instagramUserService.UpdateInstagramUser(userObtenido);
            }
        }

        [HttpGet]
        public void fixig()
        {
            FixDates();
            CreateFollowers();
            
        }

        private void FixDates()
        {
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            foreach (var user in users)
            {
                var followersHistory = user.Followers.OrderByDescending(f => f.Date).ToList();
                var elevenNotFound = true;
                var twentyFiveNotFound = true;
                var i = 0;
                while (elevenNotFound || twentyFiveNotFound && i < followersHistory.Count)
                {
                    if (followersHistory[i].Date.Day == 11 && followersHistory[i].Date.Month == 04 && followersHistory[i].Date.Year == 2019 && elevenNotFound)
                    {
                        if (user.Username != "elartedenegar" && user.Username != "leaigounet" && user.Username != "manuelasaiz" && user.Username != "quierostandup")
                        {
                            var itemToDelete = followersHistory[i];
                            followersHistory.Remove(itemToDelete);
                            elevenNotFound = false;
                        }
                    }
                    if(followersHistory[i].Date.Day == 25 && followersHistory[i].Date.Month == 04 && followersHistory[i].Date.Year == 2019 && twentyFiveNotFound)
                    {
                        var itemToDelete = followersHistory[i];
                        followersHistory.Remove(itemToDelete);
                        twentyFiveNotFound = false;
                        if (user.Username == "elartedenegar" || user.Username == "leaigounet" || user.Username == "manuelasaiz" || user.Username == "quierostandup")
                            elevenNotFound = false;
                    }
                    i++;
                }
                user.Followers = followersHistory;
                service.UpdateInstagramUser(user);
            }
        }
    }
}