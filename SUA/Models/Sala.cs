using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Sala
    {
        public int UniqueId { get; set; }
        public string Nombre { get; set; }
        public Ubicacion Direccion { get; set; }
        public int Capacidad { get; set; }
        public string Telefono { get; set; }
        public string Administrador { get; set; }
        public string Email { get; set; }
        public string Luces { get; set; }
        public Contacto TecnicoLuces { get; set; }
        public string Sonido { get; set; }
        public Contacto TecnicoSonido { get; set; }
        public string Proyector { get; set; }
        public string Pantalla { get; set; }
        public string ArregloEconomico { get; set; }
        public string WhatsAppPersonal { get; set; }
        public string Arreglo { get; set; }
        public string Argentores { get; set; }
        public List<string> ImpuestosYGastos { get; set; }
        public string Acomodadores { get; set; }
        public string Boletero { get; set; }
        public Camarin Camarin { get; set; }
        public Carteleria Carteleria { get; set; }
        public Contacto Prensa { get; set; }
        public Boleteria Boleteria { get; set; }
        public string Horario { get; set; }
        public string TicketeraOnline { get; set; }
        public PruebaSonidoEnsayo PruebaSonidoEnsayo { get; set; }
        public string ComoPagan { get; set; }
        public string AclaracionesSala { get; set; }
    }
}