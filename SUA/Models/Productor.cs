using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Productor
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public Ubicacion Direccion { get; set; }
        public string TransportePropio { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Foto { get; set; }
        public string Observaciones { get; set; }
    }
}