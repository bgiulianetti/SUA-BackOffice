using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class SalaService
    {
        public ESRepositorio Repository { get; set; }

        public SalaService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.sala.ToString());
        }

        public List<Sala> GetSalas()
        {
            return Repository.GetSalas();
        }
        public Sala GetSalaById(string id)
        {
            return Repository.GetSalaById(id);
        }
        public Sala GetSalaByNombre(string nombre)
        {
            return Repository.GetSalaByNombre(nombre);
        }
        public Sala GetSalaByProvincia(string provincia)
        {
            return Repository.GetSalaByProvincia(provincia);
        }
        public void AddSala(Sala sala)
        {
            Repository.AddSala(sala);
        }
        public void UpdateSala(Sala sala)
        {
            Repository.UpdateSala(sala);
        }
        public string GetSalaInnerIdById(string id)
        {
            return Repository.GetSalaInnerIdById(id);
        }
        public void DeleteSala(string id)
        {
            Repository.DeleteSala(id);
        }
    }
}