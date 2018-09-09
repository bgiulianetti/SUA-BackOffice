﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SUA.Models;
using Nest;
using Elasticsearch.Net;

namespace SUA.Repositorios
{
    public class ESRepositorio
    {
        public const string INVALID_SETTINGS_EXCEPTION = "Configuración de ES inválida";

        //Standupero
        public const string INVALID_STANDUPERO_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch al querer obtener todos los standuperos";
        public const string STANDUPERO_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero por apellido debe pasar un apellido válido";
        public const string STANDUPERO_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero por apellido";
        public const string STANDUPERO_GET_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero por dni";
        public const string STANDUPERO_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero por dni debe pasar un dni válido";
        public const string STANDUPERO_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION = "Para obtener un standupero innerId por dni debe pasar un dni válido";
        public const string STANDUPERO_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION = "Error al querer buscar un standupero innerId por dni";
        public const string STANDUPERO_CREATE_INVALID_PARAMETER_EXCEPTION = "Para agregar un standupero debe pasar un standupero válido";
        public const string STANDUPERO_CREATE_ALREADY_EXISTS_EXCEPTION = "Para agregar un standupero debe pasar un standupero que no exista previamente";
        public const string STANDUPERO_NOT_CREATED_EXCEPTION = "Falla al querer crear un standupero nuevo";
        public const string STANDUPERO_UPDATE_INVALID_EXCEPTION = "Para agregar un standupero debe pasar un standupero válido";
        public const string STANDUPERO_UPDATE_INVALID_PARAMETER_EXCEPTION = "Para editar un standupero debe pasar un standupero válido";
        public const string STANDUPERO_UPDATE_NOT_EXISTS_EXCEPTION = "Para editar un standupero debe pasar un standupero que exista previamente";
        public const string STANDUPERO_UPDATE_NOT_UPDATED_EXCEPTION = "Falla al querer editar un standupero";
        public const string STANDUPERO_DELETE_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION = "Para borrar un standupero por apellido debe pasar un apellido válido";
        public const string STANDUPERO_DELETE_NOT_EXISTS_EXCEPTION = "Para borrar un standupero debe pasar un standupero que exista previamente";
        public const string STANDUPERO_DELETE_NOT_DELETED_EXCEPTION = "Falla al querer borrar un standupero";
        


        //Productor
        public const string PRODUCTOR_INVALID_EXCEPTION = "productor_inválido";
        public const string PRODUCTOR_ALREADY_EXISTS_EXCEPTION = "productor_ya_existente";
        public const string PRODUCTOR_NOT_EXISTS_EXCEPTION = "productor_no_existente";
        public const string PRODUCTOR_NOT_UPDATED_EXCEPTION = "productor_no_actualizado";
        public const string PRODUCTOR_NOT_CREATED_EXCEPTION = "productor_no_creado";
        public const string PRODUCTOR_NOT_DELETED_EXCEPTION = "productor_no_borrado";


        //Show
        public const string SHOW_INVALID_EXCEPTION = "show_inválido";
        public const string SHOW_ALREADY_EXISTS_EXCEPTION = "show_ya_existente";
        public const string SHOW_NOT_EXISTS_EXCEPTION = "show_no_existente";
        public const string SHOW_NOT_UPDATED_EXCEPTION = "show_no_actualizado";
        public const string SHOW_NOT_CREATED_EXCEPTION = "show_no_creado";
        public const string SHOW_NOT_DELETED_EXCEPTION = "show_no_borrado";


        protected ElasticClient Client { get; set; }
        protected string Index { get; set; }

        public ESRepositorio(ESSettings settings, string index)
        {
            if (settings == null)
                throw new Exception(INVALID_SETTINGS_EXCEPTION);

            var config = new ConnectionSettings(settings.Node.Uri);
            Client = new ElasticClient(config);

            Index = index;
        }

        /*-------------------Standupero-------------------*/

        public List<Standupero> GetStanduperos()
        {
            var response = Client.Search<Standupero>(s => s
                   .Index(Index)
                   .Type(Index)
                  );

            if (response == null)
                throw new Exception(INVALID_STANDUPERO_ES_CONNECTION_EXCEPTION);

            if (response.OriginalException != null)
                throw new Exception(response.OriginalException.Message);

            var standuperos = new List<Standupero>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standuperos.Add(item);
            }
            return standuperos;
        }
        public Standupero GetStanduperoByApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                throw new Exception(STANDUPERO_GET_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Apellido).Query(apellido)))
                    );

            if (response == null)
                return null;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_GET_BY_APELLIDO_INVALID_SEARCH_EXCEPTION);

            Standupero standupero = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standupero = item;
            }
            return standupero;
        }
        public Standupero GetStanduperoByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_GET_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("dni", dni)));

            if (response == null)
                return null;

            if(!response.IsValid)
                throw new Exception(STANDUPERO_GET_BY_DNI_INVALID_SEARCH_EXCEPTION);

            Standupero standupero = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    standupero = item;
            }
            return standupero;
        }
        public string GetStanduperoInnerIdByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_GET_INNERID_BY_DNI_INVALID_PARAMETER_EXCEPTION);

            var response = Client.Search<Standupero>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("dni", dni))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if(response.IsValid)
                throw new Exception(STANDUPERO_GET_INNERID_BY_DNI_INVALID_SEARCH_EXCEPTION);

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddStandupero(Standupero standupero)

        {
            if (standupero == null)
                throw new Exception(STANDUPERO_CREATE_INVALID_PARAMETER_EXCEPTION);

            var resultado = GetStanduperoByDni(standupero.Dni);
            if (resultado != null)
                throw new Exception(STANDUPERO_CREATE_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(standupero, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(STANDUPERO_NOT_CREATED_EXCEPTION);
        }
        public void UpdateStandupero(Standupero standupero)
        {
            if (standupero == null)
                throw new Exception(STANDUPERO_UPDATE_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(standupero.Dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_UPDATE_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(standupero, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(STANDUPERO_UPDATE_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteStandupero(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_DELETE_BY_APELLIDO_INVALID_PARAMETER_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_DELETE_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Standupero>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(STANDUPERO_DELETE_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllStanduperos()
        {
            Client.DeleteByQuery<Standupero>(q => q.Type(Index));
        }


        /*--------------------Productor-----------------------*/
        public List<Productor> GetProductores()
        {
            var response = Client.Search<Productor>(p => p
                   .Index(Index)
                   .Type(Index)
                  );

            if (response == null)
                throw new Exception(INVALID_ES_CONNECTION_EXCEPTION);
            if (response.OriginalException != null)
                throw new Exception(response.OriginalException.Message);

            var productores = new List<Productor>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productores.Add(item);
            }
            return productores;
        }
        public Productor GetProductorByApellido(string apellido)
        {
            var response = Client.Search<Productor>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Apellido).Query(apellido)))
                    );

            if (response == null)
                return null;
            Productor productor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productor = item;
            }
            return productor;
        }
        public Productor GetProductorByDni(string dni)
        {
            var response = Client.Search<Productor>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("dni", dni)));

            if (response == null)
                return null;
            Productor productor = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    productor = item;
            }
            return productor;
        }
        public string GetProductorInnerIdByDni(string dni)
        {
            var response = Client.Search<Productor>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("dni", dni))
                  );

            string innerId = null;
            if (response == null)
                return innerId;
            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void AddProductor(Productor productor)
        {
            if (productor == null)
                throw new Exception(PRODUCTOR_INVALID_EXCEPTION);

            var resultado = GetProductorByDni(productor.Dni);
            if (resultado != null)
                throw new Exception(PRODUCTOR_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(productor, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(PRODUCTOR_NOT_CREATED_EXCEPTION);
        }
        public void UpdateProductor(Productor productor)
        {
            if (productor == null)
                throw new Exception(PRODUCTOR_INVALID_EXCEPTION);

            var innerId = GetProductorInnerIdByDni(productor.Dni);
            if (innerId == null)
                throw new Exception(PRODUCTOR_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(productor, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(PRODUCTOR_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteProductor(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(PRODUCTOR_INVALID_EXCEPTION);

            var innerId = GetProductorInnerIdByDni(dni);
            if (innerId == null)
                throw new Exception(PRODUCTOR_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Productor>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(PRODUCTOR_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllProductores()
        {
            Client.DeleteByQuery<Productor>(q => q.Type(Index));
        }



        /*--------------------Show-----------------------*/
        public List<Show> GetShows()
        {
            var response = Client.Search<Show>(p => p
                   .Index(Index)
                   .Type(Index)
                  );

            if (response == null)
                throw new Exception(INVALID_ES_CONNECTION_EXCEPTION);
            if (response.OriginalException != null)
                throw new Exception(response.OriginalException.Message);

            var shows = new List<Show>();
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    shows.Add(item);
            }
            return shows;
        }
        public Show GetShowByNombre(string nombre)
        {
            var response = Client.Search<Show>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Nombre).Query(nombre)))
                    );

            if (response == null)
                return null;
            Show show = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    show = item;
            }
            return show;
        }
        public Show GetShowById(string id)
        {
            var response = Client.Search<Show>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("uniqueId", id)));

            if (response == null)
                return null;
            Show show = null;
            if (response.Total > 0)
            {
                foreach (var item in response.Documents)
                    show = item;
            }
            return show;
        }
        public void AddShow(Show show)
        {
            if (show == null)
                throw new Exception(SHOW_INVALID_EXCEPTION);

            var resultado = GetShowByNombre(show.Nombre);
            if (resultado != null)
                throw new Exception(SHOW_ALREADY_EXISTS_EXCEPTION);

            resultado = GetShowById(show.UniqueId);
            if (resultado != null)
                throw new Exception(SHOW_ALREADY_EXISTS_EXCEPTION);

            var response = Client.IndexAsync(show, i => i
              .Index(Index)
              .Type(Index)
              .Refresh(Refresh.True)
              ).Result;

            if (!response.IsValid)
                throw new Exception(SHOW_NOT_CREATED_EXCEPTION);
        }
        public void UpdateShow(Show show)
        {
            if (show == null)
                throw new Exception(SHOW_INVALID_EXCEPTION);

            var innerId = GetShowInnerIdById(show.UniqueId);
            if (innerId == null)
                throw new Exception(SHOW_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(show, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(SHOW_NOT_UPDATED_EXCEPTION);
        }
        public string GetShowInnerIdById(string id)
        {
            var response = Client.Search<Show>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("uniqueId", id))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public string GetShowInnerIdByNombre(string nombre)
        {
            var response = Client.Search<Show>(s => s
                    .Index(Index)
                    .Type(Index)
                    .Query(q => q.Term("nombre", nombre))
                  );

            string innerId = null;
            if (response == null)
                return innerId;

            if (response.Total > 0)
            {
                foreach (var item in response.Hits)
                    innerId = item.Id;
            }
            return innerId;
        }
        public void DeleteShow(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception(SHOW_INVALID_EXCEPTION);

            var innerId = GetShowInnerIdById(id);
            if (innerId == null)
                throw new Exception(SHOW_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Show>(innerId, d => d
                                                    .Index(Index)
                                                    .Type(Index)
                                                    .Refresh(Refresh.True)
                                                    );
            if (!response.IsValid)
                throw new Exception(PRODUCTOR_NOT_DELETED_EXCEPTION);
        }
        public void DeleteAllShows()
        {
            Client.DeleteByQuery<Show>(q => q.Type(Index));
        }


        /*---------Metodos genericos------------------*/

        public void DeleteIndex()
        {
            if (IndexExists())
            {
                var response = Client.DeleteIndex(Index);
                if (!response.IsValid)
                    throw new Exception("delte_index_exception");
            }
        }    
        public bool IndexExists()
        {
            var response = Client.IndexExists(Index);
            return response.Exists;
        }
        public void CreateIndex()
        {
            Client.CreateIndex(Index, c => c
                       .Settings(s => s
                         .NumberOfShards(5)
                         .NumberOfReplicas(5))
                   );
        }

        public enum ContentType
        {
            standupero,
            show,
            sala,
            productor,
            fecha
        }
    }
}