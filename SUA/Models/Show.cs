using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Show
    {
        public string _Show { get; set; }
        public string Nombre { get; set; }
        public List<Standupero> Integrantes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<string> Rider { get; set; }
        public string Camarin { get; set; }
        public string Observaciones { get; set; }
        public Productor ProductorDefault { get; set; }
    }
}