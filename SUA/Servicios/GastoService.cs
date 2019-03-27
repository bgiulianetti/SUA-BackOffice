using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class GastoService
    {
        public ESRepositorio Repository { get; set; }

        public GastoService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.gasto.ToString());
        }

        public List<Gasto> GetGastos()
        {
            return Repository.GetGastos();
        }

        public Gasto GetGastoById(string id)
        {
            return Repository.GetGastoById(id);
        }

        public string GetGastoInnerIdById(string id)
        {
            return Repository.GetGastoInnerIdById(id);
        }

        public void AddGasto(Gasto gasto)
        {
            Repository.AddGasto(gasto);
        }

        public void AddBulkGasto(List<Gasto> gastos)
        {
            Repository.AddBulkGasto(gastos);
        }

        public void UpdateGasto(Gasto gasto)
        {
            Repository.UpdateGasto(gasto);
        }

        public void DeleteGasto(string id)
        {
            Repository.DeleteGasto(id);
        }

        public void DeleteAllGastos()
        {
            Repository.DeleteAllGastos();
        }
    }
}