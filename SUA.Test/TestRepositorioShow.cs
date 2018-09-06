using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SUA.Repositorios;
using SUA.Models;
using System.Collections.Generic;

namespace SUA.Test
{
    [TestClass]
    public class TestRepositorioShow
    {
        ESRepositorio repository;
        ESSettings settings;
        string index;


        [TestInitialize]
        public void Setup()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            settings = new ESSettings(node);
            index = "test_" + ESRepositorio.ContentType.show.ToString();
            repository = new ESRepositorio(settings, index);
            DeleteIndex();
        }

        [TestCleanup]
        public void CleanUp()
        {
            DeleteIndex();
            repository = null;
        }

        private void DeleteIndex()
        {
            repository.DeleteIndex();
        }

        [TestMethod]
        public void SiAgregoUnShowExistenteObtengoUnError()
        {
            var show = CrearShow(1);
            repository.AddShow(show);
            try
            {
                repository.AddShow(show);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, ESRepositorio.SHOW_ALREADY_EXISTS_EXCEPTION);
            }

        }

        [TestMethod]
        public void PuedoAgregarUnShowCorrectamente()
        {
            var id = 1;
            var show = CrearShow(id);
            repository.AddShow(show);
            var showObtenido = repository.GetShowById(id.ToString());
            Assert.AreEqual(show, showObtenido);
        }

        [TestMethod]
        public void PuedoObtenerTodosLosShowsCorrectamente()
        {
            var show1 = CrearShow(1);
            var show2 = CrearShow(2);
            repository.AddShow(show1);
            repository.AddShow(show2);

            var shows = repository.GetShows();
            foreach (var item in shows)
            {
                if (item.UniqueId == 1)
                    Assert.AreEqual(show1, item);
                else if (item.UniqueId == 2)
                    Assert.AreEqual(show2, item);
            }
        }
        [TestMethod]
        public void PuedoObtenerUnStanduperoPorApellidoCorrectamente()
        {
            var productor = CrearProductor("32576829", "Giulianetti", "Bruno", "Argentina");
            repository.AddProductor(productor);
            var productorObtenido = repository.GetProductorByApellido(productor.Apellido);
            Assert.AreEqual(productor, productorObtenido);
        }
        [TestMethod]
        public void SiModificoUnProductorInexistenteObtengoUnError()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            try
            {
                repository.UpdateProductor(productor);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, ESRepositorio.PRODUCTOR_NOT_EXISTS_EXCEPTION);
            }
        }
        [TestMethod]
        public void PuedoModificarUnProductorCorrectamente()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            repository.AddProductor(productor);
            productor.Nombre = "Nombre cambiado";
            repository.UpdateProductor(productor);

            var productorObtenido = repository.GetProductorByDni(productor.Dni);
            Assert.AreEqual(productorObtenido, productor);
        }
        [TestMethod]
        public void SiEliminoUnProductorInexistenteObtengoUnError()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            repository.CreateIndex();
            try
            {
                repository.GetProductorByDni(productor.Dni);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, ESRepositorio.PRODUCTOR_NOT_EXISTS_EXCEPTION);
            }
        }
        [TestMethod]
        public void PuedoEliminarUnProductorCorrectamente()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            repository.AddProductor(productor);

            repository.DeleteProductor(productor.Dni);

            var producorObtenido = repository.GetProductorByDni(productor.Dni);
            Assert.AreEqual(producorObtenido, null);
        }


        private Productor CrearProductor(string dni, string apellido, string nombre, string pais)
        {
            return new Productor
            {
                Nombre = nombre,
                Apellido = apellido,
                Direccion = new Ubicacion { Direccion = "Migueletes 680", Localidad = "CABA", Ciudad = "CABA", CodigoPostal = "1426", Provincia = "Buenos Aires", Pais = pais },
                Dni = dni,
                FechaAlta = DateTime.Now,
                FechaNacimiento = new DateTime(1986, 10, 10),
                TransportePropio = "bicicleta",
                DatosBancarios = new DatosBancarios { TipoCuenta = "Caja de Ahorro", Alias = "musica.caoba.jaula", Banco = "BANCO SANTANDER RIO", Cbu = "cbu", CuilCuit = "20-32576829-1", NombreCompleto = "Bruno Nicolas giulianetti" },
                Observaciones = "Ninguna Observacion",
                Celular = "1122526344",
                Email = "bruno.giulianetti@gmail.com"
            };
        }
        private Standupero CrearStandupero(string dni, string apellido, string nombre, string pais)
        {
            return new Standupero
            {
                Nombre = nombre,
                Apellido = apellido,
                Direccion = new Ubicacion { Direccion = "Migueletes 680", Localidad = "CABA", Ciudad = "CABA", CodigoPostal = "1426", Provincia = "Buenos Aires", Pais = pais },
                Dni = dni,
                FechaAlta = DateTime.Now,
                FechaNacimiento = new DateTime(1986, 10, 10),
                TransportePropio = "bicicleta",
                Foto = "url de una foto",
                DatosBancarios = new DatosBancarios { TipoCuenta = "Caja de Ahorro", Alias = "musica.caoba.jaula", Banco = "BANCO SANTANDER RIO", Cbu = "cbu", CuilCuit = "20-32576829-1", NombreCompleto = "Bruno Nicolas giulianetti" },
                Observaciones = "Ninguna Observacion",
                Celular = "1122526344",
                Email = "bruno.giulianetti@gmail.com",
                InstagramUser = "@bgiulianetti"
            };
        }
        private Show CrearShow(int id)
        {
            return new Show
            {
                Camarin = "Uno muy grande",
                UniqueId = id,
                Integrantes = new List<Standupero> { CrearStandupero("12345989", "Chouy", "Mike", "Argentina"),
                                                     CrearStandupero("36621192", "Orsi", "Dario", "Argentina"), },
                Nombre = "Sanata",
                _Show = "Desubicado",
                Rider = new List<string> { "5 microfonos" },
                ProductorDefault = CrearProductor("32576829", "Giulianeetti", "Federico", "Argentina"),
                FechaAlta = DateTime.Now,
                FechaCreacion = DateTime.Now,
                Observaciones = "Son graciosos"
            };
        }

    }
}
