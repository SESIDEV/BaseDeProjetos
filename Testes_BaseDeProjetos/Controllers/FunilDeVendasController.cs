using Xunit;
using Testes_BaseDeProjetos.Controllers;
using System.Threading.Tasks;
using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Linq;
using System.Net;
using SmartTesting.Controllers;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BaseDeProjetos.Controllers.Tests
{
    public class FunilDeVendasControllerTests : BaseTestes
    {
        private const string _IdValidoProjeto = "proj_637477823195206322";

        public FunilDeVendasControllerTests(BaseApplicationFactory<Startup> factory) : base(factory)
        {
            factory.CreateClient();
        }
        
        [Fact]
        public void DeveSerVeradeiro()
        {
            Assert.True(true);
        }


         /*
         * 
         *  Funções auxiliares de teste
         * 
         * 
         */

        private FunilDeVendasController SetupController(ApplicationDbContext context)
        {
            var _controller = new FunilDeVendasController(context, new Mailer(new List<Usuario>()));
            CriarSeedDB();
            return _controller;
        }

        private void CriarSeedDB()
        {
            return;
        }

        private void RemoverEntradaDeTeste(Prospeccao prosp)
        {
            //Teardown
            _context.Prospeccao.Remove(prosp);
            _context.SaveChanges();
        }

    }

}