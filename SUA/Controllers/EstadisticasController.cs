using Newtonsoft.Json;
using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;
using SUA.Filters;

namespace SUA.Controllers
{
    public class EstadisticasController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Index()
        {
            ViewBag.title = "Estadisticas";
            return View();
        }

        [HttpGet]
        public string GetGananciasNetasPorMes(int year)
        {
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var fechas = new List<DateAndInfo>
            {
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  1, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 01, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  2, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01) && f.FechaHorario <= new DateTime(year, 02, 28)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  3, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01) && f.FechaHorario <= new DateTime(year, 03, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  4, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01) && f.FechaHorario <= new DateTime(year, 04, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  5, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01) && f.FechaHorario <= new DateTime(year, 05, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  6, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01) && f.FechaHorario <= new DateTime(year, 06, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  7, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01) && f.FechaHorario <= new DateTime(year, 07, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  8, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01) && f.FechaHorario <= new DateTime(year, 08, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  9, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01) && f.FechaHorario <= new DateTime(year, 09, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 10, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01) && f.FechaHorario <= new DateTime(year, 10, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 11, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01) && f.FechaHorario <= new DateTime(year, 11, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 12, 1 )), units = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01) && f.FechaHorario <= new DateTime(year, 12, 31)).ToList()) }
            };
            return JsonConvert.SerializeObject(fechas);
        }

        [HttpGet]
        public string GetGananciasBrutasPorMes(int year)
        {
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var fechas = new List<DateAndInfo>
            {
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  1, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 01, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  2, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01) && f.FechaHorario <= new DateTime(year, 02, 28)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  3, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01) && f.FechaHorario <= new DateTime(year, 03, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  4, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01) && f.FechaHorario <= new DateTime(year, 04, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  5, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01) && f.FechaHorario <= new DateTime(year, 05, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  6, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01) && f.FechaHorario <= new DateTime(year, 06, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  7, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01) && f.FechaHorario <= new DateTime(year, 07, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  8, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01) && f.FechaHorario <= new DateTime(year, 08, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  9, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01) && f.FechaHorario <= new DateTime(year, 09, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 10, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01) && f.FechaHorario <= new DateTime(year, 10, 31)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 11, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01) && f.FechaHorario <= new DateTime(year, 11, 30)).ToList()) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 12, 1 )), units = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01) && f.FechaHorario <= new DateTime(year, 12, 31)).ToList()) }
            };
            return JsonConvert.SerializeObject(fechas);
        }

        [HttpGet]
        public string GetGananciasNetasPorMesVersus(int yearA, int yearB)
        {
            var service = new FechaService();
            var fechasInYearA = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(yearA, 01, 01) && f.FechaHorario <= new DateTime(yearA, 12, 31) && f.Borederaux != null).ToList();
            var fechasA = new List<LabelAndInfo>
            {
                new LabelAndInfo { label = "Jan", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 01, 01) && f.FechaHorario <= new DateTime(yearA, 01, 31)).ToList()) },
                new LabelAndInfo { label = "Feb", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 02, 01) && f.FechaHorario <= new DateTime(yearA, 02, 28)).ToList()) },
                new LabelAndInfo { label = "Mar", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 03, 01) && f.FechaHorario <= new DateTime(yearA, 03, 31)).ToList()) },
                new LabelAndInfo { label = "Apr", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 04, 01) && f.FechaHorario <= new DateTime(yearA, 04, 30)).ToList()) },
                new LabelAndInfo { label = "May", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 05, 01) && f.FechaHorario <= new DateTime(yearA, 05, 31)).ToList()) },
                new LabelAndInfo { label = "Jun", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 06, 01) && f.FechaHorario <= new DateTime(yearA, 06, 30)).ToList()) },
                new LabelAndInfo { label = "Jul", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 07, 01) && f.FechaHorario <= new DateTime(yearA, 07, 31)).ToList()) },
                new LabelAndInfo { label = "Aug", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 08, 01) && f.FechaHorario <= new DateTime(yearA, 08, 31)).ToList()) },
                new LabelAndInfo { label = "Sep", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 09, 01) && f.FechaHorario <= new DateTime(yearA, 09, 30)).ToList()) },
                new LabelAndInfo { label = "Oct", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 10, 01) && f.FechaHorario <= new DateTime(yearA, 10, 31)).ToList()) },
                new LabelAndInfo { label = "Nov", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 11, 01) && f.FechaHorario <= new DateTime(yearA, 11, 30)).ToList()) },
                new LabelAndInfo { label = "Dec", y = GetMonthNetoSum(fechasInYearA.Where(f => f.FechaHorario >= new DateTime(yearA, 12, 01) && f.FechaHorario <= new DateTime(yearA, 12, 31)).ToList()) }
            };


            var fechasInYearB = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(yearB, 01, 01) && f.FechaHorario <= new DateTime(yearB, 12, 31) && f.Borederaux != null).ToList();
            var fechasB = new List<LabelAndInfo>
            {
                new LabelAndInfo { label = "Jan", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 01, 01) && f.FechaHorario <= new DateTime(yearB, 01, 31)).ToList()) },
                new LabelAndInfo { label = "Feb", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 02, 01) && f.FechaHorario <= new DateTime(yearB, 02, 28)).ToList()) },
                new LabelAndInfo { label = "Mar", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 03, 01) && f.FechaHorario <= new DateTime(yearB, 03, 31)).ToList()) },
                new LabelAndInfo { label = "Apr", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 04, 01) && f.FechaHorario <= new DateTime(yearB, 04, 30)).ToList()) },
                new LabelAndInfo { label = "May", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 05, 01) && f.FechaHorario <= new DateTime(yearB, 05, 31)).ToList()) },
                new LabelAndInfo { label = "Jun", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 06, 01) && f.FechaHorario <= new DateTime(yearB, 06, 30)).ToList()) },
                new LabelAndInfo { label = "Jul", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 07, 01) && f.FechaHorario <= new DateTime(yearB, 07, 31)).ToList()) },
                new LabelAndInfo { label = "Aug", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 08, 01) && f.FechaHorario <= new DateTime(yearB, 08, 31)).ToList()) },
                new LabelAndInfo { label = "Sep", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 09, 01) && f.FechaHorario <= new DateTime(yearB, 09, 30)).ToList()) },
                new LabelAndInfo { label = "Oct", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 10, 01) && f.FechaHorario <= new DateTime(yearB, 10, 31)).ToList()) },
                new LabelAndInfo { label = "Nov", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 11, 01) && f.FechaHorario <= new DateTime(yearB, 11, 30)).ToList()) },
                new LabelAndInfo { label = "Dec", y = GetMonthNetoSum(fechasInYearB.Where(f => f.FechaHorario >= new DateTime(yearB, 12, 01) && f.FechaHorario <= new DateTime(yearB, 12, 31)).ToList()) }
            };

            var lista = new List<List<LabelAndInfo>> {
                fechasA,
                fechasB
            };

            return JsonConvert.SerializeObject(lista);
        }

        [HttpGet]
        public string GetGananciasNetasPorShow(int year)
        {
            var info = new List<PieChartContract>();
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var showService = new ShowService();
            var shows = showService.GetShows();
            foreach (var show in shows)
            {
                var montoByShow = fechasInYear.Where(f => f.Show.UniqueId == show.UniqueId).Sum(f => f.Borederaux.SUAMontoFinal);
                info.Add(new PieChartContract { label = show._Show, y = montoByShow });
            }

            return JsonConvert.SerializeObject(info);
        }

        private string GetMonthNetoSum(List<Fecha> fechas)
        {
            float sum = fechas.Sum(f => f.Borederaux.SUAMontoFinal);
            var sum_string = sum.ToString("0.0").Replace(",", ".");
            return sum_string;
        }

        private string GetMonthBrutoSum(List<Fecha> fechas)
        {
            float sum = fechas.Sum(f => f.Borederaux.EntradasBruto);
            var sum_string = sum.ToString("0.0").Replace(",", ".");
            return sum_string;
        }
    }
}