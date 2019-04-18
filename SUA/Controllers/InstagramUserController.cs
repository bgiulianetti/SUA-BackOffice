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


        private void FixInstagramUsers()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-04-12*397020,397022,397041,397285,397533,397629",
                "angiesammartino*2019-04-12*142878,142947,143016,143097,143123,143132",
                "chapumartinez*2019-04-12*1129200,1129155,1128819,1129020,1128763,1128168",
                "connieballarini*2019-04-12*139277,141211,141447,141945,142360,142726",
                "crococro*2019-04-12*69953,69994,69989,69985,69999,70007",
                "darioorsi*2019-04-12*740175,739969,739665,739403,739299,739298",
                "dieguitomaggio*2019-04-12*41691,41750,41760,41743,41726,41707",
                "ezequielcampa*2019-04-12*86089,86086,86070,86046,86025,86043",
                "fedecyrulnik*2019-04-12*365378,365173,364983,364817,364708,364575",
                "fedesimonetti*2019-04-12*19203,19215,19216,19212,19212,19207",
                "fermetilli*2019-04-12*363947,365017,365232,365417,365651,365799",
                "fersanjiao*2019-04-12*42393,42446,42543,42625,42677,42716",
                "ffrangomez*2019-04-12*422617,422835,423011,423358,423866,424182",
                "gonzovizan*2019-04-12*144749,144690,144622,144532,144443,144371",
                "gregorossello*2019-04-12*1010218,1009750,1009198,1008745,1008244,1007971",
                "javichosoria*2019-04-12*17536,17530,17534,17550,17570,17581",
                "joaquin__castellano*2019-04-12*174992,175109,175134,175364,175374,175351",
                "juampicarbonetti*2019-04-12*26469,26476,26464,26480,26480,26504",
                "juampigon*2019-04-12*412961,413321,413283,413223,413054,413023",
                "julibellese*2019-04-12*115218,115335,115375,115394,115360,115314",
                "lailaroth*2019-04-12*79759,79746,79678,79593,79576,79606",
                "lendrogh*2019-04-12*9353,9348,9428,9417,9414,9396",
                "lucaslauriente*2019-04-12*108954,108974,108959,108955,108928,108932",
                "lucaslezin*2019-04-12*1034701,1035664,1036328,1036544,1037143,1037637",
                "luchomellera*2019-04-12*118849,118885,118885,118877,118875,118887",
                "magalitajes*2019-04-12*1244386,1244502,1244644,1245670,1248087,1248580",
                "malepichot*2019-04-12*463432,463517,463521,463524,463514,463585",
                "martarresok_*2019-04-12*372697,372885,373005,373084,373287,373426",
                "martin_pugliese*2019-04-12*60124,60170,60164,60169,60169,60174",
                "martincirio*2019-04-12*736942,739290,740917,742407,743946,745796",
                "mikechouhy*2019-04-12*737809,737952,738090,738340,739340,739948",
                "molinerd*2019-04-12*538802,538949,539051,539080,539172,539408",
                "nachitosaralegui*2019-04-12*383263,383486,383919,384182,385700,386286",
                "nicolasdetracy*2019-04-12*416907,416833,416790,416935,417301,417400",
                "nicombraun*2019-04-12*64193,64369,64727,64969,65150,65359",
                "pablitofabregas*2019-04-12*76737,76835,76878,76992,77050,77074",
                "pablopicotto*2019-04-12*137931,138844,139172,139373,139589,139807",
                "pichipiccirillo*2019-04-12*12786,12782,12783,12786,12784,12778",
                "pipabarbato*2019-04-12*83239,83229,83237,83283,83312,83321",
                "rodriguezgalati*2019-04-12*644341,644785,645097,645771,646216,646724",
                "soyrada*2019-04-12*1238652,1238936,1239069,1239121,1239353,1239853",
                "standupargentina*2019-04-12*67324,67363,67372,67382,67466,67551",
                "virsammartino*2019-04-12*11629,11627,11633,11636,11641,11640"
            };

        }

    }
}