using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class EstadisticasController : Controller
    {
        // GET: Estadisticas
        public ActionResult Index()
        {
            //Ganancias netas por mes //ejemplo del clima
            //Ganancias brutas por mes //ejemplo del clima

            //ganancias netas por año //barras (como cant de shows)
            //ganancias brutas por año //barras (como cant de shows)

            //Ganancias netas por show por año //pie chart

            //Ganancia de cada show por mes //barras
            ViewBag.gananciasNetasPorMes = GetGananciasNetasPorMes();
            return View();
        }


        [HttpGet]
        public List<DateAndInfo> GetGananciasNetasPorMes(int year = 2019)
        {
            var service = new FechaService();
            var fechasInYear = service.GetFechas("true").Where(f=>f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31) && f.Borederaux != null).ToList();
            var fechas = new List<DateAndInfo>();
            fechas.Add(new DateAndInfo { Year = year, Month = 00, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 01, 01)).Sum(f=>f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 01, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 02, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 02, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 03, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 03, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 04, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 04, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 05, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 05, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 06, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 06, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 07, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 07, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 08, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 08, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 09, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 09, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 10, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 10, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 11, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            fechas.Add(new DateAndInfo { Year = year, Month = 11, Day = 01, Info = fechasInYear.Where(f => f.FechaHorario >= new DateTime(year, 12, 01)).Sum(f => f.Borederaux.SUAMontoFinal).ToString() });
            return fechas;
        }
    }
}