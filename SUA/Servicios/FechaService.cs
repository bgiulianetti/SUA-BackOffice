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
            var node = new UriBuilder("localhost")
            {
                Port = 9200
            };
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
        public List<Fecha> GetFechasByShowId(string id)
        {
            return Repository.GetFechasByShowId(id);
        }
        public Fecha GetUltimaFechaByShowId(string id)
        {
            var fechas = Repository.GetFechasByShowId(id);
            var fechasOrdenadas = fechas.OrderBy(f => f.FechaHorario);
            return fechasOrdenadas.Last();
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

        public Fecha GetUltimaFechaBySalaId(string idSala)
        {
            var fechas = Repository.GetFechasByIdSala(idSala);
            if (fechas.Count > 0)
            {
                fechas.OrderBy(f => f.FechaHorario);
                return fechas.Last();
            }
            return null;

        }
        public Fecha GetUltimaFechaBySalaAndShow(string idSala, string idShow)
        {
            var fechas = Repository.GetFechasByIdSala(idSala);
            var fechasPorShowEspecifico = fechas.Find(f => f.Show.UniqueId == idShow);
            if (fechasPorShowEspecifico != null)
            {
                fechas.OrderBy(f => f.FechaHorario);
                return fechas.First();
            }
            return null;
        }

        public List<Fecha> GetFechasConBordereaux()
        {
            var fechasConBordereaux = new List<Fecha>();
            var fechas = Repository.GetFechas();
            foreach (var item in fechas)
            {
                if (item.Borederaux != null)
                    fechasConBordereaux.Add(item);
            }
            return fechasConBordereaux;
        }

    }
}