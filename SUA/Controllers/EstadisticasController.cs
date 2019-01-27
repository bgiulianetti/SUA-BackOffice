using Newtonsoft.Json;
using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SUA.Utilities;

namespace SUA.Controllers
{
    public class EstadisticasController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //Ganancias netas por mes //ejemplo del clima
            //Ganancias brutas por mes //ejemplo del clima

            //ganancias netas por año //barras (como cant de shows)
            //ganancias brutas por año //barras (como cant de shows)

            //Ganancias netas por show por año //pie chart

            //Ganancia de cada show por mes //barras



            ViewBag.gananciasNetasPorMes = GetGananciasNetasPorMes();
            ViewBag.title = "Estadisticas";
            return View();
        }




        [HttpGet]
        public string GetGananciasNetasPorMes(int year = 2019)
        {
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f=>f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var fechas = new List<DateAndInfo>
            {
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  1, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  2, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  3, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  4, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  5, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  6, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  7, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  8, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year,  9, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 10, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 11, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01)).Sum(f => f.Borederaux.SUAMontoFinal) },
                new DateAndInfo { date = UtilitiesAndStuff.DateToMiliseconds(new DateTime(year, 12, 1 )), units = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01)).Sum(f => f.Borederaux.SUAMontoFinal) }
            };
            return JsonConvert.SerializeObject(fechas);
        }

    }
}