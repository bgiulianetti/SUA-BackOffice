using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Sala
    {
        public string UniqueId { get; set; }
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
        public string DatosBancarios { get; set; }
        public DateTime FechaAlta { get; set; }
        public int RepeticionEnDias { get; set; }

        public override string ToString()
        {
            var salaString = "";
            salaString += "Sala: " + this.Nombre + "\n";
            salaString += Direccion == null ? "" : "Dirección: " + this.Direccion.Direccion + ", " + this.Direccion.Ciudad + "\n";
            salaString += "Capacidad: " +  this.Capacidad + "\n";
            salaString += Telefono == null ? "" : "Telefono: " + this.Telefono + "\n";
            salaString += "\n";
            salaString += Administrador == null ? "" : "Administrador: " + this.Administrador + "\n";
            salaString += Email == null ? "" : "Email: " + this.Email + "\n\n";

            //Luces
            salaString += Luces == null ? "" : "Luces: " + this.Luces + "\n";
            if (this.TecnicoLuces != null)
            {
                salaString += string.IsNullOrEmpty(TecnicoLuces.Nombre) ? "" : "Técnico Luces - Nombre: " + this.TecnicoLuces.Nombre + "\n";
                salaString += string.IsNullOrEmpty(TecnicoLuces.Email) ? "" : "Técnico Luces - Email: " + this.TecnicoLuces.Email + "\n";
                salaString += string.IsNullOrEmpty(TecnicoLuces.Descripcion) ? "" : "Técnico Luces - Descripcion: " + this.TecnicoLuces.Descripcion + "\n\n";
            }

            //Sonido
            salaString += string.IsNullOrEmpty(Sonido) ? "" : "Sonido: " + this.Sonido + "\n";
            if (this.TecnicoSonido != null)
            {
                salaString += string.IsNullOrEmpty(TecnicoSonido.Nombre) ? "" : "Técnico Sonido - Nombre: " + this.TecnicoSonido.Nombre + "\n";
                salaString += string.IsNullOrEmpty(TecnicoSonido.Email) ? "" : "Técnico Sonido - Email: " + this.TecnicoSonido.Email + "\n";
                salaString += string.IsNullOrEmpty(TecnicoSonido.Descripcion) ? "" : "Técnico Sonido - Descripcion: " + this.TecnicoSonido.Descripcion + "\n\n";
            }

            salaString += string.IsNullOrEmpty(Proyector) ? "" : "Proyector: " + this.Proyector + "\n";
            salaString += string.IsNullOrEmpty(Pantalla) ? "" : "Pantalla: " + this.Pantalla + "\n";
            salaString += string.IsNullOrEmpty(ArregloEconomico) ? "" : "Arreglo: " + this.Arreglo + "\n";
            salaString += string.IsNullOrEmpty(WhatsAppPersonal) ? "" : "WhatsAppPersonal: " + this.WhatsAppPersonal + "\n";
            salaString += string.IsNullOrEmpty(Acomodadores) ? "" : "Acomodadores: " + this.Acomodadores + "\n";
            salaString += string.IsNullOrEmpty(Boletero) ? "" : "Boletero: " + this.Boletero + "\n\n";
            //Camarin
            if (this.Camarin != null)
            {
                salaString += string.IsNullOrEmpty(Camarin.Descripcion) ? "" : "Camarin - Descripción: " + this.Camarin.Descripcion + "\n";
                salaString += string.IsNullOrEmpty(Camarin.Toilet) ? "" : "Camarin - Baño: " + this.Camarin.Toilet + "\n\n";
            }

            //Carteleria
            if (this.Carteleria != null)
            {
                salaString += string.IsNullOrEmpty(Carteleria.Descripcion) ? "" : "Carteleria - Descripción: " + this.Carteleria.Descripcion + "\n";
                salaString += this.Carteleria.Cantidad == 0 ? "" : "Carteleria - Cantidad: " + this.Carteleria.Cantidad + "\n";
                salaString += this.Carteleria.Precio == 0 ? "" : "Carteleria - Precio: " + this.Carteleria.Precio + "\n\n";
            }

            //Prensa
            if (this.Prensa != null)
            {
                salaString += string.IsNullOrEmpty(Prensa.Nombre) ? "" : "Prensa - Nombre: " + this.Prensa.Nombre + "\n";
                salaString += string.IsNullOrEmpty(Prensa.Descripcion) ? "" : "Prensa - Descripción: " + this.Prensa.Descripcion + "\n";
                salaString += string.IsNullOrEmpty(Prensa.Email) ? "" : "Prensa - Email: " + this.Prensa.Email + "\n";
                salaString += string.IsNullOrEmpty(Prensa.Telefono) ? "" : "Prensa - Teléfono: " + this.Prensa.Telefono + "\n\n";
            }

            //Prensa
            if (this.Boleteria != null)
            {
                salaString += string.IsNullOrEmpty(Boleteria.Descripcion) ? "" : "Boleteria - Descripción: " + this.Boleteria.Descripcion + "\n";
                salaString += string.IsNullOrEmpty(Boleteria.Horario) ? "" : "Boleteria - Horario: " + this.Boleteria.Horario + "\n";
                salaString += string.IsNullOrEmpty(Boleteria.Telefono) ? "" : "Boleteria - Telefono: " + this.Boleteria.Telefono + "\n\n";
            }

            salaString += string.IsNullOrEmpty(Horario) ? "" : "Horario: " + this.Horario + "\n";
            salaString += string.IsNullOrEmpty(TicketeraOnline) ? "" : "TicketeraOnline: " + this.TicketeraOnline + "\n\n";

            //Prueba de sonido
            if (this.PruebaSonidoEnsayo != null)
            {
                salaString += string.IsNullOrEmpty(PruebaSonidoEnsayo.Descripcion) ? "" : "Prueba de Sonido/Ensayo - Descripción: " + this.PruebaSonidoEnsayo.Descripcion + "\n";
                salaString += string.IsNullOrEmpty(PruebaSonidoEnsayo.Horario) ? "" : "Prueba de Sonido/Ensayo - Horario: " + this.PruebaSonidoEnsayo.Horario + "\n\n";
            }

            salaString += string.IsNullOrEmpty(ComoPagan) ? "" : "ComoPagan: " + this.ComoPagan + "\n";
            salaString += string.IsNullOrEmpty(AclaracionesSala) ? "" : "AclaracionesSala: " + this.AclaracionesSala + "\n\n";

            return salaString;
        }

        public void SetIdAndFechaAlta()
        {
            UniqueId = UtilitiesAndStuff.GenerateUniqueId();
            FechaAlta = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Sala && (obj as Sala).GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }
    }
}