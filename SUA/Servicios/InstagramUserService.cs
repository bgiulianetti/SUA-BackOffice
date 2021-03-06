﻿using SUA.Models;
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
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.instagramuser.ToString());
        }

        public List<InstagramUser> GetInstagramUsers()
        {
            var users = Repository.GetInstagramUsers();
            foreach (var user in users)
            {
                user.Followers = user.Followers.OrderByDescending(f => f.Date).ToList();
            }
            return users.Where(f => f.Status != "deleted").ToList();
        }

        public List<InstagramUser> GetSUAInstagramUsers()
        {
            var standuperosSUA = new List<InstagramUser>();
            var standuperos = new StanduperoService().GetStanduperos();
            foreach (var standupero in standuperos)
            {
                var instagramUser = GetInstagramUserByUsername(standupero.InstagramUser.Replace("@", "").ToLower());
                if(instagramUser != null)
                {
                    instagramUser.Followers = instagramUser.Followers.OrderByDescending(f => f.Date).ToList();
                    standuperosSUA.Add(instagramUser);
                }
            }
            return standuperosSUA.OrderByDescending(f => f.Username).ToList();
        }

        public InstagramUser GetInstagramUserByUsername(string username)
        {
            var user = Repository.GetInstagramUserByUsername(username.Replace("@", ""));
            if (user == null)
                return null;
            if (user.Status == "deleted")
                return null;
            else
                return user;
        }

        public void AddInstagramUser(InstagramUser user)
        {
            Repository.AddInstagramUser(user);
        }

        public void AddBulkInstagramUser(List<InstagramUser> users)
        {
            Repository.AddBulkInstagramUser(users);
        }

        public void UpdateInstagramUser(InstagramUser user)
        {
            Repository.UpdateInstagramUser(user);
        }

    }
}