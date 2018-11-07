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

        public List<Votacion> GetVotaciones()
        {
            return Repository.GetVotaciones();
        }

        public void AddVotacion(Votacion votacion)
        {
            Repository.AddVotacion(votacion);
        }

        public void GetVotacionesByShow(string show)
        {
            Repository.GetVotacionesByShow(show);
        }

        public List<RankingRecord> GetRankingByShow(string show)
        {
            var ranking = new List<RankingRecord>();
            var votaciones = Repository.GetVotacionesByShow(show);
            var votacionesOrdenadas = votaciones.OrderBy(f => f.Ciudad.Nombre);
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
            var rankingOrdenado = ranking.OrderByDescending(f=>f.VotesCount).ToList();
            return rankingOrdenado.Take(100).ToList();
        }
    }
}