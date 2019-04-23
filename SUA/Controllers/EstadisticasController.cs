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
            var fechas = new List<LabelAndInfo>
            {
                new LabelAndInfo { label = "Jan", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 01, 31)).ToList()) },
                new LabelAndInfo { label = "Feb", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01) && f.FechaHorario <= new DateTime(year, 02, 28)).ToList()) },
                new LabelAndInfo { label = "Mar", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01) && f.FechaHorario <= new DateTime(year, 03, 31)).ToList()) },
                new LabelAndInfo { label = "Apr", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01) && f.FechaHorario <= new DateTime(year, 04, 30)).ToList()) },
                new LabelAndInfo { label = "May", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01) && f.FechaHorario <= new DateTime(year, 05, 31)).ToList()) },
                new LabelAndInfo { label = "Jun", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01) && f.FechaHorario <= new DateTime(year, 06, 30)).ToList()) },
                new LabelAndInfo { label = "Jul", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01) && f.FechaHorario <= new DateTime(year, 07, 31)).ToList()) },
                new LabelAndInfo { label = "Aug", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01) && f.FechaHorario <= new DateTime(year, 08, 31)).ToList()) },
                new LabelAndInfo { label = "Sep", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01) && f.FechaHorario <= new DateTime(year, 09, 30)).ToList()) },
                new LabelAndInfo { label = "Oct", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01) && f.FechaHorario <= new DateTime(year, 10, 31)).ToList()) },
                new LabelAndInfo { label = "Nov", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01) && f.FechaHorario <= new DateTime(year, 11, 30)).ToList()) },
                new LabelAndInfo { label = "Dec", y = GetMonthNetoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01) && f.FechaHorario <= new DateTime(year, 12, 31)).ToList()) }
            };
            return JsonConvert.SerializeObject(fechas);
        }

        [HttpGet]
        public string GetGananciasBrutasPorMes(int year)
        {
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var fechas = new List<LabelAndInfo>
            {
                new LabelAndInfo { label = "Jan", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 01, 31)).ToList()) },
                new LabelAndInfo { label = "Feb", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01) && f.FechaHorario <= new DateTime(year, 02, 28)).ToList()) },
                new LabelAndInfo { label = "Mar", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01) && f.FechaHorario <= new DateTime(year, 03, 31)).ToList()) },
                new LabelAndInfo { label = "Apr", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01) && f.FechaHorario <= new DateTime(year, 04, 30)).ToList()) },
                new LabelAndInfo { label = "May", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01) && f.FechaHorario <= new DateTime(year, 05, 31)).ToList()) },
                new LabelAndInfo { label = "Jun", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01) && f.FechaHorario <= new DateTime(year, 06, 30)).ToList()) },
                new LabelAndInfo { label = "Jul", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01) && f.FechaHorario <= new DateTime(year, 07, 31)).ToList()) },
                new LabelAndInfo { label = "Aug", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01) && f.FechaHorario <= new DateTime(year, 08, 31)).ToList()) },
                new LabelAndInfo { label = "Sep", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01) && f.FechaHorario <= new DateTime(year, 09, 30)).ToList()) },
                new LabelAndInfo { label = "Oct", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01) && f.FechaHorario <= new DateTime(year, 10, 31)).ToList()) },
                new LabelAndInfo { label = "Nov", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01) && f.FechaHorario <= new DateTime(year, 11, 30)).ToList()) },
                new LabelAndInfo { label = "Dec", y = GetMonthBrutoSum(fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01) && f.FechaHorario <= new DateTime(year, 12, 31)).ToList()) }
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
        public string GetGananciasNetasPorShow(string from, string to)
        {
            var fromDate = new DateTime(Int32.Parse(from.Split('-')[0]), Int32.Parse(from.Split('-')[1]), Int32.Parse(from.Split('-')[2]));
            var toDate = new DateTime(Int32.Parse(to.Split('-')[0]), Int32.Parse(to.Split('-')[1]), Int32.Parse(to.Split('-')[2]));
            var info = new List<PieChartContract>();
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= fromDate && f.FechaHorario <= toDate && f.Borederaux != null).ToList();
            var showService = new ShowService();
            var shows = showService.GetShows();
            float sumatoria = 0;
            foreach (var show in shows)
            {
                var montoByShow = fechasInYear.Where(f => f.Show.UniqueId == show.UniqueId).Sum(f => f.Borederaux.SUAMontoFinal);
                if(montoByShow > 0)
                {
                    float gasto = 0;
                    foreach (var fecha in fechasInYear)
                    {
                        if (fecha.Show.UniqueId == show.UniqueId && fecha.Gastos != null)
                        {
                            gasto += (float)fecha.Gastos.Sum(g => g.Importe);
                        }
                    }
                    montoByShow = montoByShow - gasto;
                    info.Add(new PieChartContract { label = show._Show, y = montoByShow });
                    sumatoria += montoByShow;
                }
            }
            var contador = 0;
            foreach (var showInfo in info)
            {
                var porcentaje = showInfo.y * 100 / sumatoria;
                info[contador].label = info[contador].label + "-" + Math.Round(porcentaje, 2).ToString() + "%";
                contador++;
            }
            info.Add(new PieChartContract { label = "sumatoria", y = sumatoria});
            return JsonConvert.SerializeObject(info);
        }

        [HttpGet]
        public string GetGananciasNetasPorShowConPorcentaje(string from, string to)
        {
            var fromDate = new DateTime(Int32.Parse(from.Split('-')[2]), Int32.Parse(from.Split('-')[1]), Int32.Parse(from.Split('-')[0]));
            var toDate = new DateTime(Int32.Parse(to.Split('-')[2]), Int32.Parse(to.Split('-')[1]), Int32.Parse(to.Split('-')[0]));
            var info = new List<PieChartContract>();
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f => f.FechaHorario >= fromDate && f.FechaHorario <= toDate && f.Borederaux != null).ToList();
            var showService = new ShowService();
            var shows = showService.GetShows();
            float sumatoria = 0;
            foreach (var show in shows)
            {
                var montoByShow = fechasInYear.Where(f => f.Show.UniqueId == show.UniqueId).Sum(f => f.Borederaux.SUAMontoFinal);
                if (montoByShow > 0)
                {
                    float gasto = 0;
                    foreach (var fecha in fechasInYear)
                    {
                        if (fecha.Show.UniqueId == show.UniqueId && fecha.Gastos != null)
                        {
                            gasto += (float)fecha.Gastos.Sum(g => g.Importe);
                        }
                    }
                    montoByShow = montoByShow - gasto;
                    info.Add(new PieChartContract { label = show._Show, y = montoByShow });
                    sumatoria += montoByShow;
                }
            }
            var listShows = new List<ShowListInfo>();
            foreach (var showInfo in info)
            {
                var porcentaje = showInfo.y * 100 / sumatoria;
                listShows.Add(new ShowListInfo { Show = showInfo.label, Porcentaje = porcentaje, Monto = showInfo.y });
            }
            return JsonConvert.SerializeObject(listShows);
        }

        [HttpGet]
        public string GetGastosFueraBxPorMes(int year)
        {
            var gastos = new GastoService().GetGastos();
            var gastosFormateados = new List<LabelAndInfo>
            {
                new LabelAndInfo { label = "Jan", y = gastos.Where(g => g.Fecha >= new DateTime(year, 01, 01) && g.Fecha <= new DateTime(year, 01, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Feb", y = gastos.Where(g => g.Fecha >= new DateTime(year, 02, 01) && g.Fecha <= new DateTime(year, 02, 28)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Mar", y = gastos.Where(g => g.Fecha >= new DateTime(year, 03, 01) && g.Fecha <= new DateTime(year, 03, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Apr", y = gastos.Where(g => g.Fecha >= new DateTime(year, 04, 01) && g.Fecha <= new DateTime(year, 04, 30)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "May", y = gastos.Where(g => g.Fecha >= new DateTime(year, 05, 01) && g.Fecha <= new DateTime(year, 05, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Jun", y = gastos.Where(g => g.Fecha >= new DateTime(year, 06, 01) && g.Fecha <= new DateTime(year, 06, 30)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Jul", y = gastos.Where(g => g.Fecha >= new DateTime(year, 07, 01) && g.Fecha <= new DateTime(year, 07, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Aug", y = gastos.Where(g => g.Fecha >= new DateTime(year, 08, 01) && g.Fecha <= new DateTime(year, 08, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Sep", y = gastos.Where(g => g.Fecha >= new DateTime(year, 09, 01) && g.Fecha <= new DateTime(year, 09, 30)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Oct", y = gastos.Where(g => g.Fecha >= new DateTime(year, 10, 01) && g.Fecha <= new DateTime(year, 10, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Nov", y = gastos.Where(g => g.Fecha >= new DateTime(year, 11, 01) && g.Fecha <= new DateTime(year, 11, 30)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") },
                new LabelAndInfo { label = "Dec", y = gastos.Where(g => g.Fecha >= new DateTime(year, 12, 01) && g.Fecha <= new DateTime(year, 12, 31)).Sum(g=>g.Importe).ToString("0.0").Replace(",", ".") }
            };
            return JsonConvert.SerializeObject(gastosFormateados);
        }

        private string GetMonthNetoSum(List<Fecha> fechas)
        {
            float sum = fechas.Sum(f => f.Borederaux.SUAMontoFinal);
            float gasto = 0;
            foreach (var fecha in fechas)
            {
                if (fecha.Gastos != null)
                    gasto += (float)fecha.Gastos.Sum(g => g.Importe);
            }
            sum = sum - gasto;
            var sum_string = sum.ToString("0.0").Replace(",", ".");
            return sum_string;
        }

        private string GetMonthBrutoSum(List<Fecha> fechas)
        {
            float sum = fechas.Sum(f => f.Borederaux.EntradasBruto);
            var sum_string = sum.ToString("0.0").Replace(",", ".");
            return sum_string;
        }

        private class ShowListInfo
        {
            public string Show { get; set; }
            public double Porcentaje { get; set; }
            public double Monto { get; set; }
        }
    }
}