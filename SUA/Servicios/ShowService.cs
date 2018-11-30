using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class ShowService
    {
        public ESRepositorio Repository { get; set; }

        public ShowService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.show.ToString());
        }

        public List<Show> GetShows(bool isIndexRequesting = false)
        {
            var shows = Repository.GetShows();
            var session = HttpContext.Current.Request.Cookies.Get("session");
            var service = new UserService();
            var user = service.GetUserByNombre(session.Value);
            if(user.UserMaster == "no")
            {
                if (isIndexRequesting)
                    return shows;

                if (user.ShowsAsignados.Count > 0)
                {
                    var showFiltrados = new List<Show>();
                    foreach (var item in shows)
                    {
                        if (user.ShowsAsignados.Contains(item))
                            showFiltrados.Add(item);
                    }
                    return showFiltrados;
                }
                else
                {
                    return new List<Show>();
                }
            }
            else
            {
                return shows;
            }
        }

        public List<Show> GetShowsForBackUp()
        {
            return Repository.GetShows();
        }

        public Show GetShowByNombre(string nombre)
        {
            return Repository.GetShowByNombre(nombre);
        }

        public Show GetShowById(string id)
        {
            return Repository.GetShowById(id);
        }

        public string GetShowInnerIdById(string id)
        {
            return Repository.GetShowInnerIdById(id);
        }

        public string GetShowInnerIdByNombre(string nombre)
        {
            return Repository.GetShowInnerIdByNombre(nombre);
        }

        public void AddShow(Show show)
        {
            Repository.AddShow(show);
        }

        public void AddBulkShow(List<Show> shows)
        {
            Repository.AddBulkShow(shows);
        }

        public void UpdateShow(Show show)
        {
            Repository.UpdateShow(show);
        }

        public void DeleteShow(string id)
        {
            Repository.DeleteShow(id);
        }

        public void DeleteAllShows()
        {
            Repository.DeleteAllShows();
        }

    }
}