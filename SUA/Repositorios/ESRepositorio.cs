using System;
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
        public const string INVALID_ES_CONNECTION_EXCEPTION = "Falla al querer conectar con elasticsearch";

        public const string STANDUPERO_INVALID_EXCEPTION = "standupero_invalido";
        public const string STANDUPERO_ALREADY_EXISTS_EXCEPTION = "standupero_ya_existente";
        public const string STANDUPERO_NOT_EXISTS_EXCEPTION = "standupero_no_existente";
        public const string STANDUPERO_NOT_UPDATED_EXCEPTION = "standupero_no_actualizado";
        public const string STANDUPERO_NOT_CREATED_EXCEPTION = "standupero_no_creado";
        public const string STANDUPERO_NOT_DELETED_EXCEPTION = "standupero_no_borrado";


        public const string PRODUCTOR_INVALID_EXCEPTION = "productor inválido";
        public const string PRODUCTOR_ALREADY_EXISTS_EXCEPTION = "productor_ya_existente";
        public const string PRODUCTOR_NOT_EXISTS_EXCEPTION = "productor_no_existente";
        public const string PRODUCTOR_NOT_UPDATED_EXCEPTION = "productor_no_actualizado";
        public const string PRODUCTOR_NOT_CREATED_EXCEPTION = "productor_no_creado";
        public const string PRODUCTOR_NOT_DELETED_EXCEPTION = "productor_no_borrado";

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
                throw new Exception(INVALID_ES_CONNECTION_EXCEPTION);
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
            var response = Client.Search<Standupero>(s => s
                .Index(Index)
                .Type(Index)
                .Query(q => q
                    .Match(m => m.Field(f => f.Apellido).Query(apellido)))
                    );

            if (response == null)
                return null;
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
            var response = Client.Search<Standupero>(s => s
                   .Index(Index)
                   .Type(Index)
                   .Query(q => q.Term("dni", dni)));

            if (response == null)
                return null;
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
            var response = Client.Search<Standupero>(s => s
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
        public void AddStandupero(Standupero standupero)
        {
            if (standupero == null)
                throw new Exception(STANDUPERO_INVALID_EXCEPTION);

            var resultado = GetStanduperoByDni(standupero.Dni);
            if (resultado != null)
                throw new Exception(STANDUPERO_ALREADY_EXISTS_EXCEPTION);

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
                throw new Exception(STANDUPERO_INVALID_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(standupero.Dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_NOT_EXISTS_EXCEPTION);

            var result = Client.Index(standupero, i => i
                            .Index(Index)
                            .Type(Index)
                            .Id(innerId)
                            .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception(STANDUPERO_NOT_UPDATED_EXCEPTION);
        }
        public void DeleteStandupero(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new Exception(STANDUPERO_INVALID_EXCEPTION);

            var innerId = GetStanduperoInnerIdByDni(dni);
            if (innerId == null)
                throw new Exception(STANDUPERO_NOT_EXISTS_EXCEPTION);

            var response = Client.Delete<Standupero>(innerId, d => d
                                                        .Index(Index)
                                                        .Type(Index)
                                                        .Refresh(Refresh.True)
                                                        );
            if (!response.IsValid)
                throw new Exception(STANDUPERO_NOT_DELETED_EXCEPTION);
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