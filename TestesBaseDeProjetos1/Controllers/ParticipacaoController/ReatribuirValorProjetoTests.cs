using BaseDeProjetos.Controllers;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using TestesBaseDeProjetos1.TestHelper;

namespace BaseDeProjetos.Controllers.Tests.ParticipacaoControllerTests
{
    [TestFixture]
    internal class ReatribuirValorProjetoTests
    {
        private ParticipacaoController? _controller;
        private ApplicationDbContext? _context;
        private ObjectCreator? _objectCreator;

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
        }

        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalDentroPeriodoProjeto()
        {
            var projeto = new Projeto
            {
                DataInicio = new DateTime(2023, 1, 1),
                DataEncerramento = new DateTime(2023, 12, 31),
                ValorTotalProjeto = 12000
            };

            DateTime dataFinalFiltro = new DateTime(2023, 6, 1);
            int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio, true);
            int qtdMesesFiltrados = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, projeto.DataInicio, true);

            decimal valorProjMes = (decimal)projeto.ValorTotalProjeto / qtdMesesTotal;

            _controller?.ReatribuirValorProjeto(projeto, dataFinalFiltro);

            Assert.AreEqual(valorProjMes * qtdMesesFiltrados, projeto.ValorTotalProjeto);
        }
        
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalAntesPeriodoProjeto()
        {
            var projeto = new Projeto
            {
                DataInicio = new DateTime(2023, 1, 1),
                DataEncerramento = new DateTime(2023, 12, 31),
                ValorTotalProjeto = 12000
            };

            DateTime dataFinalFiltro = new DateTime(2022, 6, 1);

            var ex = Assert.Throws<ArgumentException>(() => _controller?.ReatribuirValorProjeto(projeto, dataFinalFiltro));

            if (ex != null)
            {
                StringAssert.Contains("dataFinalFiltro cannot be smaller than DataEncerramento", ex.Message);
            }
            else
            {
                Assert.Fail();
            }
        }
        
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalForaPeriodoProjeto()
        {
            var projeto = new Projeto
            {
                DataInicio = new DateTime(2023, 1, 1),
                DataEncerramento = new DateTime(2023, 12, 31),
                ValorTotalProjeto = 12000
            };

            DateTime dataFinalFiltro = new DateTime(2024, 1, 1);

            var ex = Assert.Throws<ArgumentException>(() => _controller?.ReatribuirValorProjeto(projeto, dataFinalFiltro));

            if (ex != null )
            {
                StringAssert.Contains("dataFinalFiltro cannot be greater than DataEncerramento", ex.Message);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
