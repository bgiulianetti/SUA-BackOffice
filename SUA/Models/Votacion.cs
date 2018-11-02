using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Votacion
    {
        public string Ip { get; set; }
        public string NombrePersona { get; set; }
        public string Show { get; set; }
        public DateTime Fecha { get; set; }
        public string Telefono { get; set; }
        public string Mensaje { get; set; }
        public Ciudad Ciudad { get; set; }
    }
}