using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SUA.Repositorios;

namespace SUA.TestProject
{
    [TestClass]
    public class TestRepositorioFecha
    {
        ESRepositorio repository;
        ESSettings settings;
        string index;

        [TestInitialize]
        public void Setup()
        {
            var node = new UriBuilder("localhost")
            { Port = 9200 };
            settings = new ESSettings(node);
            index = "test_" + ESRepositorio.ContentType.fecha.ToString();
            repository = new ESRepositorio(settings, index);
            repository.CreateIndex();
        }

        [TestCleanup]
        public void CleanUp()
        {
            DeleteIndex();
            repository = null;
        }
    }
}
