using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class InstagramUserService
    {
        public ESRepositorio Repository { get; set; }

        public InstagramUserService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.instagramUser.ToString());
        }

        public List<InstagramUser> GetInstagramUsers()
        {
            var users = Repository.GetInstagramUsers();
            return users.Where(f => f.Status != "deleted").ToList();
        }

        public void AddInstagramUser(InstagramUser user)
        {
            Repository.AddInstagramUser(user);
        }

    }
}