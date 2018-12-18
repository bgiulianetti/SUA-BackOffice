using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class VotacionService
    {
        public ESRepositorio Repository { get; set; }

        public VotacionService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.votacion.ToString());
        }

        public List<Votacion> GetVotaciones(string all = "")
        {
            var votos = Repository.GetVotaciones();
            if (all != "true")
                votos = votos.OrderByDescending(f => f.Fecha).Take(100).ToList();
            return votos;
        }

        public int GetCount()
        {
            return Repository.GetCount(ESRepositorio.ContentType.votacion.ToString());
        }

        public void AddVotacion(Votacion votacion)
        {
            Repository.AddVotacion(votacion);
        }

        public void AddBulkVotacion(List<Votacion> votaciones)
        {
            Repository.AddBulkVotacion(votaciones);
        }

        public void GetVotacionesByShow(string show)
        {
            Repository.GetVotacionesByShow(show);
        }

        public List<RankingRecord> GetRankingByShow(string show, string full)
        {
            var ranking = new List<RankingRecord>();
            var votaciones = Repository.GetVotacionesByShow(show);
            foreach (var votacion in votaciones)
            {
                var CiudadObtenida = ranking.Find(f => f.Ciudad.Nombre == votacion.Ciudad.Nombre);
                if(CiudadObtenida != null)
                {
                    var index = ranking.FindIndex(c => c.Ciudad.Nombre == votacion.Ciudad.Nombre);
                    ranking[index] = new RankingRecord { Ciudad = CiudadObtenida.Ciudad, VotesCount = CiudadObtenida.VotesCount + 1 };
                }
                else
                {
                    var record = new RankingRecord { Ciudad = votacion.Ciudad, VotesCount = 1};
                    ranking.Add(record);
                }
            }
            foreach (var registro in ranking)
            {
                registro.VotesCount = registro.VotesCount * 100 / votaciones.Count;
            }
            var rankingOrdenado = ranking.OrderByDescending(f=>f.VotesCount).ToList();

            if (full == "full")
                return rankingOrdenado;
            else
                return rankingOrdenado.Take(100).ToList();
        }
    }
}