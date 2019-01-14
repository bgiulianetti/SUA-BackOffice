using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class ProveedorService
    {
        public ESRepositorio Repository { get; set; }

        public ProveedorService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.proveedor.ToString());
        }

        public List<Proveedor> GetProveedores()
        {
            return Repository.GetProveedores();
        }
        public Proveedor GetProveedorById(string id)
        {
            return Repository.GetProveedorById(id);
        }
        public Proveedor GetProveedorByNombre(string nombre)
        {
            return Repository.GetProveedorByNombre(nombre);
        }
        public void AddProveedor(Proveedor proveedor)
        {
            Repository.AddProveedor(proveedor);
        }
        public void AddBulkProveedor(List<Proveedor> proveedores)
        {
            Repository.AddBulkProveedor(proveedores);
        }
        public void UpdateProveedor(Proveedor proveedor)
        {
            Repository.UpdateProveedor(proveedor);
        }
        public string GetProveedorInnerIdById(string id)
        {
            return Repository.GetProveedorInnerIdById(id);
        }
        public void DeleteProveedor(string id)
        {
            Repository.DeleteProveedor(id);
        }
    }
}