using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Borderaux
    {
        public List<EntradasBorderaux> Entradas { get; set; }
        public int EntradasTotal { get; set; }
        public double EntradasBruno { get; set; }

        public List<ImpuestosDeduccionesTeatroBorderaux> ImpuestosDeduccionesTeatro { get; set; }
        public double ImpuestosDeduccionesBruto { get; set; }
        public double ImpuestosDeduccionesTotalDeducir { get; set; }
        public double ImpuestosDeduccionesNeto { get; set; }

        public double ImpuestosDeduccionesCompanyPorcentaje { get; set; }
        public double ImpuestosDeduccionesCompanyMonto { get; set; }
        public double ImpuestosDeduccionesTeatroPorcentaje { get; set; }
        public double ImpuestosDeduccionesTeatroMonto { get; set; }

        public Dictionary<string, double> GastosCompany { get; set; }
        public double GastosCompanyTotal { get; set; }
        public double GastosCompanyNeto { get; set; }

        public double SUAPorcentaje { get; set; }
        public double SUAMonto { get; set; }
        public double ShowPorcentaje { get; set; }
        public double ShowMonto { get; set; }

        public DateTime FechaCobro { get; set; }
        public string Forma { get; set; }
        public string Comentarios { get; set; }
    }
}