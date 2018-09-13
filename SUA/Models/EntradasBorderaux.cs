using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class EntradasBorderaux
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total
        {
            get; set;
            /*get { return Cantidad * Precio; }
            set { }*/
        }
    }
}