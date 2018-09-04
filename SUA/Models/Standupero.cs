using Nest;
using Elasticsearch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    [ElasticsearchType(Name = "standupero")]
    public class Standupero
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public Ubicacion Direccion { get; set; }
        public string Nacionalidad { get; set; }
        public string TransportePropio { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Foto { get; set; }
        public DatosBancarios DatosBancarios { get; set; }
        public string Observaciones { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string InstagramUser { get; set; }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Standupero && (obj as Standupero).GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Dni.GetHashCode();
        }
    }
}