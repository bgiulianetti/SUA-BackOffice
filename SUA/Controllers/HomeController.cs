using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Standupero()
        {
            ViewBag.mensaje = "";
            return View();
        }


        [HttpPost]
        public ActionResult Standupero(Standupero standupero,
                                       string Direccion, string Localidad, string Ciudad, string CodigoPostal, string Pais, string Provincia,
                                       string NombreCompleto, string CuitCuil, string Cbu, string Alias, string Banco, string TipoCuenta)
        {
            var datosBancarios = new DatosBancarios
            {
                Alias = Alias,
                Banco = Banco,
                Cbu = Cbu,
                CuilCuit = CuitCuil,
                NombreCompleto = NombreCompleto,
                TipoCuenta = TipoCuenta
            };

            standupero.DatosBancarios = datosBancarios;

            var ubicacion = new Ubicacion
            {
                Ciudad = Ciudad,
                CodigoPostal = CodigoPostal,
                Direccion = Direccion,
                Localidad = Localidad,
                Pais = Pais,
                Provincia = Provincia
            };

            standupero.Direccion = ubicacion;

            var service = new StanduperoService();
            try
            {
                //service.AddStandupero(standupero);
                ViewBag.mensaje = "ok";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            return View();
        }


        [HttpGet]
        public ActionResult Standuperos()
        {
            var service = new StanduperoService();
            var standuperos = service.GetStanduperos();
            ViewBag.standuperos = standuperos;
            return View();
        }

    }
}