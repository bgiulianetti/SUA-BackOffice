using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class PrensaService
    {
        public ESRepositorio Repository { get; set; }

        public PrensaService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.prensa.ToString());
        }

        public List<Prensa> GetPrensa()
        {
            return Repository.GetPrensa();
        }
        public Prensa GetPrensaById(string id)
        {
            return Repository.GetPrensaById(id);
        }
        public Prensa GetPrensaByNombre(string nombre)
        {
            return Repository.GetPrensaByNombre(nombre);
        }
        public void AddPrensa(Prensa prensa)
        {
            Repository.AddPrensa(prensa);
        }
        public void AddBulkPrensa(List<Prensa> prensa)
        {
            Repository.AddBulkPrensa(prensa);
        }
        public void UpdatePrensa(Prensa prensa)
        {
            Repository.UpdatePrensa(prensa);
        }
        public string GetPrensaInnerIdById(string id)
        {
            return Repository.GetPrensaInnerIdById(id);
        }
        public void DeletePrensa(string id)
        {
            Repository.DeletePrensa(id);
        }
    }
}