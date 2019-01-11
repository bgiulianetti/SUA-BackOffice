using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class Proveedores
    {
        public string UniqueId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Luces { get; set; }
        public string Sonido { get; set; }
        public string Proyector { get; set; }
        public string Pantalla { get; set; }
        public string Ciudad { get; set; }
        public string Comentarios { get; set; }

        public void SetId()
        {
            UniqueId = UtilitiesAndStuff.GenerateUniqueId();
        }
    }
}