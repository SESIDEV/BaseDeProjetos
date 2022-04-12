using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SmartTesting.Controllers;
using Testes_BaseDeProjetos.Controllers;
using Xunit;

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
            FunilDeVendasController _controller = new FunilDeVendasController(context, (IEmailSender)new Mailer());
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