using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class FechaService
    {
        public ESRepositorio Repository { get; set; }

        public FechaService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.fecha.ToString());
        }


        public List<Fecha> GetFechas()
        {
            return Repository.GetFechas();
        }
        public Fecha GetFechaById(string id)
        {
            return Repository.GetFechaById(id);
        }
        public List<Fecha> GetFechasByShow(string nombreShow)
        {
            return Repository.GetFechasByShow(nombreShow);
        }
        public List<Fecha> GetFechasByProvincia(string nombreProvincia)
        {
            return Repository.GetFechasByProvincia(nombreProvincia);
        }
        public List<Fecha> GetFechasBySala(string nombreSala)
        {
            return Repository.GetFechasBySala(nombreSala);
        }
        public Fecha GetFechaBySalaAndFechaAndHorario(string idSala, DateTime fechaYHorario)
        {
            return Repository.GetFechaBySalaAndFechaAndHorario(idSala, fechaYHorario);
        }
        public void AddFecha(Fecha fecha)
        {
            Repository.AddFecha(fecha);
        }
        public void UpdateFecha(Fecha fecha)
        {
            Repository.UpdateFecha(fecha);
        }
        public string GetFechaInnerIdById(string id)
        {
            return Repository.GetFechaInnerIdById(id);
        }
        public void DeleteFecha(string id)
        {
            Repository.DeleteFecha(id);
        }

        public Fecha GetUltimaPorDeSala(string idSala)
        {
            var fechas = Repository.GetFechasByIdSala(idSala);
            fechas.OrderBy(f => f.FechaHorario);
            return fechas.First();
        }

    }
}