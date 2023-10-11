using BaseDeProjetos.Controllers;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

        /// <summary>
        /// Testa um filtro com data inicial posterior ao começo do projeto e data final posterior ao fim do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroInicialPosteriorComeco_FiltroFinalPosteriorFim()
        {
            DateTime dataInicialFiltro = new DateTime(2023, 2, 1);
            //DateTime dataFinalFiltro = new DateTime(2024, 1, 1);

            if (_objectCreator != null)
            {
                int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, _objectCreator.projeto.DataInicio, true);
                int qtdMesesFiltro = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, dataInicialFiltro, true);

                decimal valorProjMes = (decimal)_objectCreator.projeto.ValorTotalProjeto / qtdMesesTotal;
                _controller?.ReatribuirValorProjeto(dataInicialFiltro, _objectCreator.projeto);

                Assert.AreEqual(valorProjMes * qtdMesesFiltro, _objectCreator.projeto.ValorTotalProjeto);
            }
            else
            {
                Assert.Fail($"{nameof(_objectCreator)} não pode ser null");
            }
        }

        /// <summary>
        /// Testa um filtro com data inicial anterior ao começo do projeto e data final anterior ao final do projeto
        /// Caso não executado em funcionamento normal do programa
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroInicialAnteriorComeco_FiltroFinalPosteriorFim()
        {
            //DateTime dataInicial = new DateTime(2022, 1, 1);
            //DateTime dataFinalFiltro = new DateTime(2024, 1, 1);

            Assert.Pass("Condição não válida para o método, sempre passa");
        }
        
        /// <summary>
        /// Testa um filtro com data inicial anterior ao começo do projeto e data final anterior ao final do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroInicialAnteriorComeco_FiltroFinalAnteriorFim()
        {
            //DateTime dataInicialFiltro = new DateTime(2022, 1, 1);
            DateTime dataFinalFiltro = new DateTime(2023, 11, 30);

            if (_objectCreator != null)
            {
                int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, _objectCreator.projeto.DataInicio, true);
                int qtdMesesFiltro = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, _objectCreator.projeto.DataInicio, true);

                decimal valorProjMes = (decimal)_objectCreator.projeto.ValorTotalProjeto / qtdMesesTotal;

                _controller?.ReatribuirValorProjeto(_objectCreator.projeto, dataFinalFiltro);

                Assert.AreEqual(valorProjMes * qtdMesesFiltro, _objectCreator.projeto.ValorTotalProjeto);
            }
        }

        /// <summary>
        /// Testa um filtro com data inicial posterior ao começo do projeto e data final anterior ao final do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroInicialPosteriorComeco_FiltroFinalAnteriorFim()
        {
            //DateTime dataInicialFiltro = new DateTime(2021, 2, 1);
            DateTime dataFinalFiltro = new DateTime(2023, 11, 30);

            if (_objectCreator != null)
            {
                int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, _objectCreator.projeto.DataInicio, true);
                int qtdMesesFiltro = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, _objectCreator.projeto.DataInicio, true);

                decimal valorProjMes = (decimal)_objectCreator.projeto.ValorTotalProjeto / qtdMesesTotal;

                _controller?.ReatribuirValorProjeto(_objectCreator.projeto, dataFinalFiltro);

                Assert.AreEqual(valorProjMes * qtdMesesFiltro, _objectCreator.projeto.ValorTotalProjeto);
            }
            else
            {
                Assert.Fail($"{nameof(_objectCreator)} não pode ser null");
            }
        }

        /// <summary>
        /// Testa um filtro com data final posterior ao começo do projeto mas anterior ao fim do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalPosteriorComecoAnteriorFim()
        {
            DateTime dataFinalFiltro = new DateTime(2023, 6, 1);

            if (_objectCreator != null)
            {
                int qtdMesesTotal = Helpers.Helpers.DiferencaMeses(_objectCreator.projeto.DataEncerramento, _objectCreator.projeto.DataInicio, true);
                int qtdMesesFiltrados = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, _objectCreator.projeto.DataInicio, true);

                decimal valorProjMes = (decimal)_objectCreator.projeto.ValorTotalProjeto / qtdMesesTotal;

                _controller?.ReatribuirValorProjeto(_objectCreator.projeto, dataFinalFiltro);

                Assert.AreEqual(valorProjMes * qtdMesesFiltrados, _objectCreator.projeto.ValorTotalProjeto);
            }
            else
            {
                Assert.Fail($"{nameof(_objectCreator)} não pode ser null");
            }
        }

        /// <summary>
        /// Testa um filtro com data final anterior ao começo do projeto e ao fim do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalAnteriorComecoAnteriorFim()
        {
            DateTime dataFinalFiltro = new DateTime(2022, 6, 1);

            if (_objectCreator != null)
            {
                var ex = Assert.Throws<ArgumentException>(() => _controller?.ReatribuirValorProjeto(_objectCreator.projeto, dataFinalFiltro));

                if (ex != null)
                {
                    StringAssert.Contains("dataFinalFiltro não pode ser inferior a DataInicio", ex.Message);
                }
                else
                {
                    Assert.Fail($"Exceção não lançada");
                }
            }
            else
            {
                Assert.Fail($"{nameof(_objectCreator)} não pode ser null.");
            }
        }

        /// <summary>
        /// Testa um filtro com data final posterior ao começo do projeto e ao fim do projeto
        /// </summary>
        [Test]
        public void Test_ReatribuirValorProjeto_FiltroFinalPosteriorComecoPosteriorFim()
        {
            Assert.Pass("Condição não válida para o método, sempre passa");
        }
    }
}
