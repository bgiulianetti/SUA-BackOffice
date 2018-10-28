using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class RestauranteService
    {
        public ESRepositorio Repository { get; set; }

        public RestauranteService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.restaurante.ToString());
        }

        public List<Restaurante> GetRestaurantes()
        {
            return Repository.GetRestaurantes();
        }
        public Restaurante GetRestauranteById(string id)
        {
            return Repository.GetRestauranteById(id);
        }
        public Restaurante GetRestauranteByNombre(string nombre)
        {
            return Repository.GetRestauranteByNombre(nombre);
        }
        public void AddRestaurante(Restaurante restaurante)
        {
            Repository.AddRestaurante(restaurante);
        }
        public void AddBulkRestaurante(List<Restaurante> restaurantes)
        {
            Repository.AddBulkRestaurante(restaurantes);
        }
        public void UpdateRestaurante(Restaurante restaurante)
        {
            Repository.UpdateRestaurante(restaurante);
        }
        public string GetRestauranteInnerIdById(string id)
        {
            return Repository.GetRestauranteInnerIdById(id);
        }
        public void DeleteRestaurante(string id)
        {
            Repository.DeleteRestaurante(id);
        }
    }
}