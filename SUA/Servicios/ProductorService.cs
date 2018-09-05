using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class ProductorService
    {
        public ESRepositorio Repository { get; set; }

        public ProductorService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings);
        }

        public List<Productor> GetProductores()
        {
            return Repository.GetProductores();
        }

        public Productor GetProductorByApellido(string apellido)
        {
            return Repository.GetProductorByApellido(apellido);
        }

        public Productor GetProductorByDni(string dni)
        {
            return Repository.GetProductorByDni(dni);
        }

        public string GetProductorInnerIdByDni(string dni)
        {
            return Repository.GetProductorInnerIdByDni(dni);
        }

        public void AddProductor(Productor productor)
        {
            Repository.AddProductor(productor);
        }

        public void UpdateProductor(Productor productor)
        {
            Repository.UpdateProductor(productor);
        }

        public void DeleteProductor(string dni)
        {
            Repository.DeleteProductor(dni);
        }

        public void DeleteAllProductores()
        {
            Repository.DeleteAllProductores();
        }
    }
}