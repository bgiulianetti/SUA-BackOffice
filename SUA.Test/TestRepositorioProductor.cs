using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SUA.Repositorios;
using SUA.Models;

namespace SUA.Test
{
    [TestClass]
    public class TestRepositorioProductor
    {
        ESRepositorio repository;
        ESSettings settings;

        [TestInitialize]
        public void Setup()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            settings = new ESSettings(node);
            repository = new ESRepositorio(settings);
            DeleteSUAIndex();
        }

        [TestCleanup]
        public void CleanUp()
        {
            DeleteSUAIndex();
            repository = null;
        }

        private void DeleteSUAIndex()
        {
            repository.DeleteIndex();
        }
      
        [TestMethod]
        public void SiAgregoUnProductorExistenteObtengoUnError()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            repository.AddProductor(productor);
            try
            {
                repository.AddProductor(productor);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, ESRepositorio.PRODUCTOR_ALREADY_EXISTS_EXCEPTION);
            }

        }
        [TestMethod]
        public void PuedoAgregarUnProductorCorrectamente()
        {
            var dni = "32576829";
            var productor = CrearProductor(dni, "Giulianetti", "Bruno", "Argentina");
            repository.AddProductor(productor);
            var productorObtenido = repository.GetProductorByDni(dni);
            Assert.AreEqual(productor, productorObtenido);
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
                Foto = "url de una foto",
                DatosBancarios = new DatosBancarios { TipoCuenta = "Caja de Ahorro", Alias = "musica.caoba.jaula", Banco = "BANCO SANTANDER RIO", Cbu = "cbu", CuilCuit = "20-32576829-1", NombreCompleto = "Bruno Nicolas giulianetti" },
                Observaciones = "Ninguna Observacion",
                Celular = "1122526344",
                Email = "bruno.giulianetti@gmail.com"
            };
        }
    }
}
