using Newtonsoft.Json;
using SUA.Filters;
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
        public void Fix()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-04-26*399950,400250,400433,401136,401585,401902,402213,402400,402500",
                "angiesammartino*2019-04-26*142997,143103,143219,143289,143489,143835,144178,144570,144591",
                "chapumartinez*2019-04-26*1127400,1127890,1128293,1128002,1128222,1128321,1128467,1128500,1128102",
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
                "gonzovizan*2019-04-26*143165,143142,143007,142988,142870,142710,142615,142382,142305",
                "gregorossello*2019-04-26*1005400,1005465,1005499,1005520,1005802,1005793,1005701,1005721,1005762",
                "javichosoria*2019-04-26*17730,17756,17763,17777,17785,17824,17839,17909,17927",
                "joaquin__castellano*2019-04-26*175300,175295,175291,175302,175289,175280,175288,175300,175295",
                "juampicarbonetti*2019-04-26*26650,26661,26673,26685,26689,26693,26688,26695,26700",
                "juampigon*2019-04-26*413160,413261,413293,414315,414725,414799,415010,415015,415022",
                "julibellese*2019-04-26*115070,115097,115108,115123,115161,115183,115202,115232,115250",
                "lailaroth*2019-04-26*79400,79424,79489,79503,79524,79577,79615,79679,79700",
                "leaigounet*2019-04-26*129715,129803,129850,129920,130101,130459,130901,131523,131899",
                "lendrogh*2019-04-26*9370,9389,9399,9410,9419,9428,9433,9447,9449",
                "lucaslauriente*2019-04-26*109003,109040,109053,109069,109088,109092,109103,109153,109165",
                "lucaslezin*2019-04-26*1037005,1036815,1036112,1035700,1034333,1034002,1033832,1033502,1033270",
                "luchomellera*2019-04-26*119006,119043,119168,119222,119265,119296,119301,119320,119332",
                "magalitajes*2019-04-26*1254300,1254932,1255236,1255500,1255906,1256443,1257221,1257971,1258801",
                "malepichot*2019-04-26*463670,463721,463815,463922,464010,464036,464002,464042,464009",
                "manuelasaiz*2019-04-26*24670,24679,24688,24655,24632,24601,24622,24657,24660",
                "martarresok_*2019-04-26*375210,375222,375345,375410,375721,375999,376110,376387,376570",
                "martin_pugliese*2019-04-26*60222,60228,60215,60290,60284,60255,60278,60309,60315",
                "martincirio*2019-04-26*752020,753120,754944,756031,757446,758701,759002,759991,760510",
                "mikechouhy*2019-04-26*742350,742410,742602,743733,743997,744001,744202,744287,744399",
                "molinerd*2019-04-26*541602,541637,541732,541808,541879,541896,541920,542008,542387",
                "nachitosaralegui*2019-04-26*397002,398334,399111,401922,402331,404771,405891,405920,406000",
                "nicolasdetracy*2019-04-26*423033,423456,425999,428859,429234,430115,430594,430996,431020",
                "nicombraun*2019-04-26*69992,70109,70652,70992,71555,71845,72601,72977,73543",
                "pablitofabregas*2019-04-26*77520,77545,77598,77610,77699,77831,77992,78032,78002",
                "pablopicotto*2019-04-26*144702,144923,145231,146880,147833,148100,149343,150456,150801",
                "pichipiccirillo*2019-04-26*12802,12815,12826,12822,12816,12801,12834,12810,12800",
                "pipabarbato*2019-04-26*85010,85234,85345,85599,86111,86374,86596,86891,87101",
                "quierostandup*2019-04-26*26189,26197,26201,26216,26257,26287,26295,26301,26305",
                "rodriguezgalati*2019-04-26*651222,651831,652345,652876,653444,654782,655115,655189,656213",
                "soyrada*2019-04-26*1240300,1240330,1240349,1240366,1240372,1240389,1240402,1240429,1240398",
                "standupargentina*2019-04-26*68200,68233,68289,68315,68389,68410,68434,68497,68514",
                "virsammartino*2019-04-26*11655,11663,11674,11680,11683,11685,11692,11698,11690"
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

        [HttpGet]
        public void fixig()
        {
            FixDates();
            //
            Random rnd = new Random();
            int month = rnd.Next(1, 13); // creates a number between 1 and 12
        }

        private void FixDates()
        {
            var service = new InstagramUserService();
            var users = service.GetInstagramUsers();
            foreach (var user in users)
            {
                var followersHistory = user.Followers.OrderByDescending(f=>f.Date).ToList();
                var originalDate = new DateTime(2019,05,05);
                var lastCorrectDate = new DateTime(2019, 04, 11);
                var i = 0;
                while(originalDate >= lastCorrectDate)
                {
                    followersHistory[i].Date = originalDate;
                    originalDate = originalDate.AddDays(-1);
                    i++;
                }
                user.Followers = followersHistory;
                service.UpdateInstagramUser(user);
            }
        }

    }
}