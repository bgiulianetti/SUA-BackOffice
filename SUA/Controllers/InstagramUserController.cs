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
        public void Fix()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-04-26*399950,400250,400433,401136,401585,401902,402213,402400,402500",
                "angiesammartino*2019-04-26*142997,143103,143219,143289,143489,143835,144178,144570,144591",
                "chapumartinez*2019-04-26*1127400,1127890,1128293,1128002,1128222,1128321,1128467,1128500",
                "connieballarini*2019-04-26*147900,148231,149132,149884,151311,152678,153222,154320,155103",
                "crococro*2019-04-26*70020,70031,70040,70004,70010,69920,69941,69972,69960",
                "darioorsi*2019-04-26*740410,740521,740200,739923,740121,740243,740343,740554,740668",
                "dieguitomaggio*2019-04-26*41500,41492,41479,41468,41460,41456,41449,41438,41422",
                "elartedenegar*2019-04-26*60410,60730,60885,61283,61329,61730,62421,62909,63050",
                "ezequielcampa*2019-04-26*86200,86243,86218,86234,86301,86279,86543,86322,86260",
                "fedecyrulnik*2019-04-26*363897,363637,363235,363235,363009,362869,362723,362605,362400",
                "fedesimonetti*2019-04-26*19272,19292,19285,19299,19315,19269,19285,19273,19300",
                "fermetilli*2019-04-26*367380,367932,368645,368795,369234,369411,369819,370236,370596",
                "fersanjiao*2019-04-26*43015,43121,43007,43019,43163,43087,43015,43101,43150",
                "ffrangomez*2019-04-26*431601,434954,436102,438953,440452,441765,442361,443242,443900",
                "gonzovizan*2019-04-26*,,,,,,,,",
                "gregorossello*2019-04-26*,,,,,,,,",
                "javichosoria*2019-04-26*,,,,,,,,",
                "joaquin__castellano*,,,,,,,,",
                "juampicarbonetti*,,,,,,,,",
                "juampigon*2019-04-26*,,,,,,,,",
                "julibellese*2019-04-26*,,,,,,,,",
                "lailaroth*2019-04-26*,,,,,,,,",
                "lendrogh*2019-04-26*,,,,,,,,",
                "lucaslauriente*2019-04-26*,,,,,,,,",
                "lucaslezin*2019-04-26*,,,,,,,,",
                "luchomellera*2019-04-26*,,,,,,,,",
                "magalitajes*2019-04-26*,,,,,,,,",
                "malepichot*2019-04-26*,,,,,,,,",
                "martarresok_*2019-04-26*,,,,,,,,",
                "martin_pugliese*2019-04-26*,,,,,,,,",
                "martincirio*2019-04-26*,,,,,,,,",
                "mikechouhy*2019-04-26*,,,,,,,,",
                "molinerd*2019-04-26*,,,,,,,,",
                "nachitosaralegui*2019-04-26*,,,,,,,,",
                "nicolasdetracy*2019-04-26*,,,,,,,,",
                "nicombraun*2019-04-26*,,,,,,,,",
                "pablitofabregas*2019-04-26*,,,,,,,,",
                "pablopicotto*2019-04-26*,,,,,,,,",
                "pichipiccirillo*2019-04-26*,,,,,,,,",
                "pipabarbato*2019-04-26*,,,,,,,,",
                "rodriguezgalati*2019-04-26*,,,,,,,,",
                "soyrada*2019-04-26*,,,,,,,,",
                "standupargentina*2019-04-26*,,,,,,,,",
                "virsammartino*2019-04-26*,,,,,,,,"
            };

            foreach (var user in users)
            {
                var instagramUserService = new InstagramUserService();
                var userObtenido = instagramUserService.GetInstagramUserByUsername(user.Split('*')[0]);

                if (userObtenido.Username == "chapumartinez")
                {
                    userObtenido.ProfilePicture = "https://instagram.faep3-1.fna.fbcdn.net/vp/eff1b96f1644ee8d099d9bd041911a97/5D7144AA/t51.2885-19/s320x320/57111693_1236063976568847_6500151145267200000_n.jpg?_nc_ht=instagram.faep3-1.fna.fbcdn.net";
                }
                else if (userObtenido.Username == "connieballarini")
                {
                    userObtenido.ProfilePicture = "https://instagram.faep3-1.fna.fbcdn.net/vp/e57b29d8c6a284d7bd253428c38c4dad/5D759F3B/t51.2885-19/s320x320/54425512_2370290399876042_2705795174180585472_n.jpg?_nc_ht=instagram.faep3-1.fna.fbcdn.net";
                }
                else if(userObtenido.Username == "ezequielcampa")
                {
                    userObtenido.ProfilePicture = "https://instagram.faep3-1.fna.fbcdn.net/vp/ee142471be7ded1c93b630dc55e8e409/5D77CD74/t51.2885-19/s320x320/55798327_466205993918758_3553304998932643840_n.jpg?_nc_ht=instagram.faep3-1.fna.fbcdn.net";
                }
                else if (userObtenido.Username == "fermetilli")
                {
                    userObtenido.ProfilePicture = "https://instagram.faep3-1.fna.fbcdn.net/vp/418c9e438da68bc296ffcfbdea642397/5D56CD34/t51.2885-19/s320x320/53320136_630636060717891_7949599210624516096_n.jpg?_nc_ht=instagram.faep3-1.fna.fbcdn.net";
                }
                var lastDay = userObtenido.Followers.First();
                var newDays = user.Split('*').ToList()[2].Split(',').ToList();
                foreach (var day in newDays)
                {
                    var intDay = Int32.Parse(day);
                    var followersHistoryItem = new InstragramUserFollowersHistory { Count = intDay, Date = lastDay.Date.AddDays(1), Difference = intDay - lastDay.Count };
                    userObtenido.Followers.Add(followersHistoryItem);
                    lastDay = followersHistoryItem;
                }
                instagramUserService.UpdateInstagramUser(userObtenido);
            }
        }



    }
}