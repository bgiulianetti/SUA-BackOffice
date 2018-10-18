using SUA.Filters;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class HistorialController : Controller
    {
        [HttpGet]
        [UserValidationFilter]
        public ActionResult Historial()
        {
            ViewBag.titulo = "Logs";
            try
            {
                var service = new LogService();
                ViewBag.logs = service.GetLogs();
                ViewBag.mensaje = "listar";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }
    }
}