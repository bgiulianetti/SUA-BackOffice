using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Gasto
    {
        public string UniqueId { get; set; }
        public DateTime Fecha { get; set; }
        public double Importe { get; set; }
        public string Categoria { get; set; }
        public string Quien { get; set; }
        public string Detalle { get; set; }
    }
}