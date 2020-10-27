using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseDeProjetos.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Testes_BaseDeProjetos.Controllers;

namespace BaseDeProjetos.Controllers.Tests
{
    [TestClass()]
    public class Testes_FunilDeVendasController:BaseTestes
    {
        [TestMethod()]
        public void Teste_Index()
        {
            
            Assert.IsNotNull(retorno);
            Assert.IsInstanceOfType(retorno, typeof(Microsoft.AspNetCore.Mvc.ViewResult));
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Details()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Create()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Atualizar()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Create1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Edit()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Edit1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_EditarFollowUp()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_EditarFollowUp1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_Delete()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_RemoverFollowUp()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Teste_DeleteConfirmed()
        {
            Assert.Fail();
        }
    }
}