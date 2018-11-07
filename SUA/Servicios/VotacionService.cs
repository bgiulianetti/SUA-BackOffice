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
    }
}