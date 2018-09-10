using Newtonsoft.Json;
using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SUA.Servicios
{
    public class StanduperoService
    {
        public ESRepositorio Repository { get; set; }

        public StanduperoService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.standupero.ToString());
        }

        public List<Standupero> GetStanduperos()
        {
            return Repository.GetStanduperos();
        }

        public Standupero GetStanduperoByApellido(string apellido)
        {
            return Repository.GetStanduperoByApellido(apellido);
        }

        public Standupero GetStanduperoByDni(string dni)
        {
            return Repository.GetStanduperoByDni(dni);
        }

        public string GetStanduperoInnerIdByDni(string dni)
        {
            return GetStanduperoInnerIdByDni(dni);
        }

        public void AddStandupero(Standupero standupero)
        {
            Repository.AddStandupero(standupero);
        }

        public void UpdateStandupero(Standupero standupero)
        {
            Repository.UpdateStandupero(standupero);
        }

        public void DeleteStandupero(string dni)
        {
            Repository.DeleteStandupero(dni);
        }

        public void DeleteAllStanduperos()
        {
            Repository.DeleteAllStanduperos();
        }

        public InstagramUserInfo GetInstagramUserInfo(string instagramUsername)
        {
            return Repository.GeStanduperoInstagramUserInfo(instagramUsername);
        }

    }
}