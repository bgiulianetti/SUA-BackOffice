using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class InfoSalasVistaAndShow
    {
        //show especifico
        public string Show { get; set; }
        public string IdSala { get; set; }
        public int RepeticionEnDias { get; set; }
        public DateTime UltimaFechaPorShow { get; set; }
        public double DiasDesdeUltimaFechaPorShow { get; set; }

        //Cualquier show
        public DateTime UltimaFechaGeneral { get; set; }
        public string ShowPresentadoPorultimaVezEnSala { get; set; }
        public double DiasDesdeUltimaFechaGeneral { get; set; }
        public int DiferenciaEnDiasVencimiento { get; set; }

    }
}