using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TestesBaseDeProjetos1.TestHelper;

namespace BaseDeProjetos.Controllers.Tests.ParticipacaoControllerTests
{
    [TestFixture]
    internal class AcertarPrecificacaoProjetosTests
    {
        private ApplicationDbContext _context;
        private ObjectCreator _objectCreator;
        private ParticipacaoController _controller;
        private List<Projeto> projetos;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Separar o contexto do banco para cada teste
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _objectCreator = new ObjectCreator();

            // Não mude a ordem!!
            _objectCreator.CriarProjetoIndicadoresMock();
            _objectCreator.CriarCargoMock();
            _objectCreator.CriarUsuarioMock();
            _objectCreator.CriarEmpresaMock();
            _objectCreator.CriarProjetoMock();

            _controller = new ParticipacaoController(_context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "username"),
            }));

            var sessionMock = new Mock<ISession>();

            var httpContextMock = new Mock<HttpContext>();

            httpContextMock.Setup(x => x.User).Returns(user);
            httpContextMock.Setup(y => y.Session).Returns(sessionMock.Object);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object,
            };

            // Encapsulamos a lista
            projetos = new List<Projeto>
            {
                _objectCreator.projeto
            };
        }

        /// <summary>
        /// Testa um filtro com data final anterior ao começo do projeto e ao fim do projeto
        /// </summary>
        [Test]
        public void Test_AcertarPrecificacaoProjetos_FiltroFinalAnteriorComecoAnteriorFim()
        {
            var result = _controller.AcertarPrecificacaoProjetos("1", "2022", projetos);

            TestContext.Out.WriteLine($"Expected: {0}");
            TestContext.Out.WriteLine($"Actual: {result[0].ValorTotalProjeto}");

            Assert.AreEqual(0, result[0].ValorTotalProjeto);
        }

        [Test]
        public void Test_AcertarPrecificacaoProjetos_FiltroFinalPosteriorComecoAnteriorFim()
        {
            var result = _controller.AcertarPrecificacaoProjetos("6", "2023", projetos);
            DateTime dataFinalFiltro = Helpers.Helpers.ObterUltimoDiaMes(2023, 6);

            int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, _objectCreator.projeto.DataInicio, true);
            int qtdMesesFiltrados = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, _objectCreator.projeto.DataInicio, true);

            decimal valorProjMes = (decimal)_objectCreator.projeto.ValorTotalProjeto / qtdMesesTotal;

            _controller?.ReatribuirValorProjeto(_objectCreator.projeto, dataFinalFiltro);

            // Cast para float pois os valores tem um desvio pequeno "não significante"
            // e.g:
            // Expected: 27272.727272727298d
            // But was:  27272.727272727276d
            TestContext.Out.WriteLine($"Expected: {(float)valorProjMes * qtdMesesFiltrados}");
            TestContext.Out.WriteLine($"Actual: {(float)result[0].ValorTotalProjeto}");

            Assert.AreEqual((float)valorProjMes * qtdMesesFiltrados, (float)result[0].ValorTotalProjeto);
        }


    }
}
