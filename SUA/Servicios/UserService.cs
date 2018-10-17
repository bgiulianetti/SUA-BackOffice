using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class UserService
    {
        public ESRepositorio Repository { get; set; }

        public UserService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.user.ToString());
        }

        public List<UserModel> GetUsers()
        {
            return Repository.GetUsuarios();
        }
        public UserModel GetUserById(string id)
        {
            return Repository.GetUserById(id);
        }
        public UserModel GetUserByNombre(string nombre)
        {
            return Repository.GetUserByNombre(nombre);
        }
        public UserModel GetUserByEmail(string email)
        {
            return Repository.GetUserByEmail(email);
        }
        public void AddUser(UserModel user)
        {
            Repository.AddUser(user);
        }
        public void UpdateUser(UserModel user)
        {
            Repository.UpdateUser(user);
        }
        public string GetUserInnerIdById(string id)
        {
            return Repository.GetUserInnerIdById(id);
        }
        public void DeleteUser(string id)
        {
            Repository.DeleteUser(id);
        }
        public UserModel ValidateCredentials(string username, string password)
        {
            if (username == "" && password == "")
                return null;

            var service = new UserService();
            var user = service.GetUserByNombre(username);
            if (user == null)
                return null;

            if (user.Password != password)
                return null;
            user.LastLogin = DateTime.Now;
            Repository.UpdateUser(user);
            return user;

        }
    }
}