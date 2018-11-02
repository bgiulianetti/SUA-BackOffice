using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUA.Controllers
{
    public class VotarCiudadController : Controller
    {
        [HttpGet]
        public ActionResult Votar(string show)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Votar()
        {
            return View();
        }

    }
}