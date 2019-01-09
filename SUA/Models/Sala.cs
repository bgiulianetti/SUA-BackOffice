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
            salaString += Administrador == null ? "" : "Administrador: " + this.Administrador + "\n";
            salaString += Email == null ? "" : "Email: " + this.Email + "\n";

            //Luces
            salaString += Luces == null ? "" : "Luces: " + this.Luces + "\n";
            if (this.TecnicoLuces != null)
            {
                salaString += TecnicoLuces.Nombre == null ? "" : "Técnico Luces - Nombre: " + this.TecnicoLuces.Nombre + "\n";
                salaString += TecnicoLuces.Email == null ? "" : "Técnico Luces - Email: " + this.TecnicoLuces.Email + "\n";
                salaString += TecnicoLuces.Descripcion == null ? "" : "Técnico Luces - Descripcion: " + this.TecnicoLuces.Descripcion + "\n";
            }

            //Sonido
            salaString += Sonido == null ? "" : "Sonido: " + this.Sonido + "\n";
            if (this.TecnicoSonido != null)
            {
                salaString += TecnicoSonido.Nombre == null ? "" : "Técnico Sonido - Nombre: " + this.TecnicoSonido.Nombre + "\n";
                salaString += TecnicoSonido.Email == null ? "" : "Técnico Sonido - Email: " + this.TecnicoSonido.Email + "\n";
                salaString += TecnicoSonido.Descripcion == null ? "" : "Técnico Sonido - Descripcion: " + this.TecnicoSonido.Descripcion + "\n";
            }

            salaString += Proyector == null ? "" : "Proyector: " + this.Proyector + "\n";
            salaString += Pantalla == null ? "" : "Pantalla: " + this.Pantalla + "\n";
            salaString += ArregloEconomico == null ? "" : "ArregloEconomico: " + this.ArregloEconomico + "\n";
            salaString += WhatsAppPersonal == null ? "" : "WhatsAppPersonal: " + this.WhatsAppPersonal + "\n";
            salaString += Acomodadores == null ? "" : "Acomodadores: " + this.Acomodadores + "\n";
            salaString += Boletero == null ? "" : "Boletero: " + this.Boletero + "\n";

            //Camarin
            if (this.Camarin != null)
            {
                salaString += Camarin.Descripcion != null ? "" : "Camarin - Descripción: " + this.Camarin.Descripcion + "\n";
                salaString += Camarin.Toilet != null ? "" : "Camarin - Baño: " + this.Camarin.Toilet + "\n";
            }

            //Carteleria
            if (this.Carteleria != null)
            {
                salaString += Carteleria.Descripcion != null ? "" : "Carteleria - Descripción: " + this.Carteleria.Descripcion + "\n";
                salaString += "Carteleria - Cantidad: " + this.Carteleria.Cantidad + "\n";
                salaString += "Carteleria - Precio: " + this.Carteleria.Precio + "\n";
            }

            //Prensa
            if (this.Prensa != null)
            {
                salaString += Prensa.Nombre != null ? "" : "Prensa - Nombre: " + this.Prensa.Nombre + "\n";
                salaString += Prensa.Descripcion != null ? "" : "Prensa - Descripción: " + this.Prensa.Descripcion + "\n";
                salaString += Prensa.Email != null ? "" : "Prensa - Email: " + this.Prensa.Email + "\n";
                salaString += Prensa.Telefono != null ? "" : "Prensa - Teléfono: " + this.Prensa.Telefono + "\n";
            }

            //Prensa
            if (this.Boleteria != null)
            {
                salaString += Boleteria.Descripcion != null ? "" : "Boleteria - Descripción: " + this.Boleteria.Descripcion + "\n";
                salaString += Boleteria.Horario != null ? "" : "Boleteria - Horario: " + this.Boleteria.Horario + "\n";
                salaString += Boleteria.Telefono != null ? "" : "Boleteria - Telefono: " + this.Boleteria.Telefono + "\n";
            }

            salaString += Horario == null ? "" : "Horario: " + this.Horario + "\n";
            salaString += TicketeraOnline == null ? "" : "TicketeraOnline: " + this.TicketeraOnline + "\n";

            //Prueba de sonido
            if (this.PruebaSonidoEnsayo != null)
            {
                salaString += PruebaSonidoEnsayo.Descripcion != null ? "" : "Prueba de Sonido/Ensayo - Descripción: " + this.PruebaSonidoEnsayo.Descripcion + "\n";
                salaString += PruebaSonidoEnsayo.Horario != null ? "" : "Prueba de Sonido/Ensayo - Horario: " + this.PruebaSonidoEnsayo.Horario + "\n";
            }

            salaString += ComoPagan == null ? "" : "ComoPagan: " + this.ComoPagan + "\n";
            salaString += AclaracionesSala == null ? "" : "AclaracionesSala: " + this.AclaracionesSala + "\n";

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