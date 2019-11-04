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
        public ActionResult InstagramUsers(string status, string deltaFollowersDateFrom, string deltaFollowersDateTo)
        {
            if (status == "full")
                ViewBag.followersCount = 1000;
            else
                ViewBag.followersCount = 7;
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


                //Followers Delta
                DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                if (deltaFollowersDateFrom != null && deltaFollowersDateFrom != string.Empty)
                {
                    from = new DateTime(Int32.Parse(deltaFollowersDateFrom.Split('-')[0]), Int32.Parse(deltaFollowersDateFrom.Split('-')[1]), 1);
                    to = new DateTime(Int32.Parse(deltaFollowersDateTo.Split('-')[0]), Int32.Parse(deltaFollowersDateTo.Split('-')[1]), DateTime.DaysInMonth(Int32.Parse(deltaFollowersDateTo.Split('-')[0]), Int32.Parse(deltaFollowersDateTo.Split('-')[1])));
                    ViewBag.yearFrom = from.Year;
                    ViewBag.monthFrom = from.Month;
                    ViewBag.yearTo = to.Year;
                    ViewBag.monthTo = to.Month;
                }
                else
                {
                    ViewBag.yearFrom = DateTime.Now.Year;
                    ViewBag.monthFrom = DateTime.Now.Month;
                    ViewBag.yearTo = DateTime.Now.Year;
                    ViewBag.monthTo = DateTime.Now.Month;
                }
                var deltaFollowers = GetInstagramUserFollowersDeltaQuantity(from, to);

                ViewBag.rankingDeltaUsersQuantity = deltaFollowers.OrderByDescending(f=>f.Quantity).ToList();
                ViewBag.rankingDeltaUsersPercentage = deltaFollowers.OrderByDescending(f => f.Percentage).ToList();
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
        public List<FollowersDeltaQuantity> GetInstagramUserFollowersDeltaQuantity(DateTime dateFrom, DateTime dateTo)
        {
            var usersList = new List<FollowersDeltaQuantity>();

            if ((dateTo - dateFrom).TotalDays <=1)
            {
                return usersList;
            }
            if (dateTo >= DateTime.Now)
                dateTo = DateTime.Now.AddDays(-1);

            var users = GetInstagramUsers();
            foreach (var user in users)
            {
                var dateFromSaved = dateFrom;
                try
                {
                    var firstFollowerDate = user.Followers.OrderBy(f => f.Date).First().Date.Date;
                    if (firstFollowerDate > dateFrom)
                        dateFrom = firstFollowerDate;

                    //followers from
                    var countFrom = user.Followers.Where(f => f.Date.Date == dateFrom.Date).FirstOrDefault().Count;
                    //followers to
                    var countTo = user.Followers.Where(f => f.Date.Date == dateTo.Date).FirstOrDefault().Count;

                    //difference percentage
                    double percentage = (double)((countTo - countFrom) * 100) / countFrom;

                    //follower difference quantity
                    var followersDeltaQuantity = user.Followers.Where(f => f.Date >= dateFrom && f.Date <= dateTo).Sum(f => f.Difference);

                    usersList.Add(new FollowersDeltaQuantity { Username = user.Username, ProfilePicture = user.ProfilePicture, Percentage = percentage, Quantity = followersDeltaQuantity });
                }
                catch(Exception ex)
                {
                    throw new Exception("Falla al querer obtener delta de usuario: " + user.Username + ". Exception: " + ex.Message);
                }
                dateFrom = dateFromSaved;
            }

            return usersList;
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
        private void updateUsers()
        {
            var users = new List<string>
            {
                "agusbattioni*2019-09-27*451180,451056",
                "angiesammartino*2019-09-2*159625,159594",
                "chapumartinez*2019-09-27*1192848,1192752",
                "connieballarini*2019-09-27*207565,207522",
                "crococro*2019-09-27*75200,75203",
                "danilachepi*2019-09-27*1743599,1744660",
                "darioorsi*2019-09-27*749631,749559",
                "dieguitomaggio*2019-09-27*39002,39002",
                "elartedenegar*2019-09-27*62720,62713",
                "ezequielcampa*2019-09-27*96794,96806",
                "fedecyrulnik*2019-09-27*477754,479230",
                "fedesimonetti*2019-09-27*22527,22583",
                "fermetilli*2019-09-27*462539,462616",
                "fersanjiao*2019-09-27*45467,45472",
                "ffrangomez*2019-09-27*561466,561730",
                "gonzovizan*2019-09-27*132745,132692",
                "gregorossello*2019-09-27*1004318,1004174",
                "javichosoria*2019-09-27*19858,19858",//19445,19462,19478,19489,19505,19518,19538,19571,19597",
                "joaquin__castellano*2019-09-27*187484,187488",//185913,186073,186061,186036,186006,185998,186051,186058,186069",
                "juampicarbonetti*2019-09-27*27606,27601",//27661,27664,27664,27650,27648,27649,27625,27624,27618",
                "juampigon*2019-09-27*421632,421559",//422864,422927,422944,422860,422868,422839,422845,422855,422841",
                "julibellese*2019-09-27*141080,141140",//137601,137617,137671,137767,138034,138212,140829,141046,141085",
                "lailaroth*2019-09-27*76101,76088",//76711,76708,76670,76633,76608,76585,76569,76563,76533",
                "leaigounet*2019-09-27*140203,140150",//140639,140571,140501,140441,140223,140154,140060,140019,139956",
                "lendrogh*2019-09-27*9405,9402",//9455,9453,9451,9451,9456,9459,9462,9470,9468",
                "lucaslauriente*2019-09-27*111108,111127",//110838,110854,110870,110897,110898,110889,110914,110943,110950",
                "lucaslezin*2019-09-27*1039622,1039934",//1017804,1018862,1020950,1021982,1022583,1023294,1023783,1024304,1024788",
                "luchomellera*2019-09-27*125334,125348",//124285,124284,124312,124331,124349,124357,124367,124392,124599",
                "magalitajes*2019-09-27*1337809,1337856",//1329663,1329902,1330289,1330788,1331134,1331486,1331928,1332029,1332041",
                "malepichot*2019-09-27*476147,476171",//474505,474533,474621,474688,474821,474887,475021,475128,475049",
                "manuelasaiz*2019-09-27*26183,26178",//26205,26189,26186,26195,26220,26219,26190,26204,26186",
                "martarresok_*2019-09-27*403021,403077",//398428,398924,399348,399671,399881,400099,400282,400373,400618",
                "martinpuglieseok*2019-09-27*62428,62421",//62015,62034,62051,62092,62062,62087,62102,62139,62152",
                "martincirio*2019-09-27*944635,953061",//844898,845629,846090,847118,848410,849453,863665,890823,895905",
                "mikechouhy*2019-09-27*783654,783686",//777179,777180,777191,777202,779022,780064,780459,780714,780986",
                "molinerd*2019-09-27*557904,557984",//554690,554946,555155,555280,555347,555387,555598,555786,556085",
                "nachitosaralegui*2019-09-27*450914,451075",//443521,443956,444321,444416,444477,445113,445550,446111,446293",
                "nicolasdetracy*2019-09-27*478183,478241",//473545,473921,474179,474348,474516,474940,475211,475360,475425",
                "nicombraun*2019-09-27*84006,83998",//83564,83619,83726,83775,83823,83969,84003,84013,84024",
                "pablitofabregas*2019-09-27*84692,84714",//83427,83460,83475,83528,83618,83773,83825,83900,83950",
                "pablopicotto*2019-09-27*208812,208921",//203493,203995,204432,204715,204968,205260,205350,205382,205833",
                "pichipiccirillo*2019-09-27*16768,16767",//16680,16683,16682,16679,16676,16679,16677,16679,16676",
                "pipabarbato*2019-09-27*118436,118594",//109308,109446,109544,109509,109675,110090,110555,110833,111008",
                "quierostandup*2019-09-27*26799,26811",//26788,26801,26806,26803,26799,26791,26802,26804,26811",
                "rodriguezgalati*2019-09-27*760655,761091",//744123,744575,745270,745753,746175,746498,747322,747700,747851",
                "soyrada*2019-09-27*1313249,1313909",//1298977,1299236,1299467,1299691,1299896,1300121,1300486,1301006,1301751",
                "standupargentina*2019-09-27*74101,74120",//72057,72132,72179,72229,72259,72301,72340,72410,72478",
                "vicuvillanueva*2019-09-27*22957,22962",//22661,22645,22643,22619,22619,22622,22621,23025,23114",
                "virsammartino*2019-09-27*17710,17775",//15125,15169,15257,15314,15341,15397,15443,15485,15503"
            };

            foreach (var user in users)
            {
                var username = user.Split('*').ToList()[0];
                var followersCount = user.Split('*').ToList()[2].Split(',').ToList();
                var date = new DateTime(2019,09,27);


                var instagramUserService = new InstagramUserService();
                var userObtenido = instagramUserService.GetInstagramUserByUsername(username);
                var followers = userObtenido.Followers.OrderBy(i => i.Date).ToList();
                //var lastCount = followers.Last().Count;

                foreach (var item in followersCount)
                {
                    var item_int = Int32.Parse(item);
                    var followerItem = new InstragramUserFollowersHistory
                    {
                        Count = item_int,
                        Date = date,
                        Difference = item_int - followers.Last().Count
                    };

                    followers.Add(followerItem);
                    //followers = followers.OrderBy(i => i.Date).ToList();
                    date = date.AddDays(1);
                }

                userObtenido.Followers = followers;
                instagramUserService.UpdateInstagramUser(userObtenido);
            }
        }

        [HttpGet]
        public ActionResult FixIG()
        {
            //updateUsers();
            return RedirectToAction("InstagramUsers");
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