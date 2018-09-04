using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Fecha
    {
        public Show Show { get; set; }
        public Sala Sala { get; set; }
        public DateTime FechaHorario { get; set; }
        public List<Productor> Productores { get; set; }
        public Borderaux Borederaux { get; set; }
        public Dictionary<string, double> PrecioEntrada { get; set; }
    }
}