using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TestesBaseDeProjetos1.TestHelper;

namespace BaseDeProjetos.Controllers.Tests
{
    [TestFixture]
    public class ProjetoControllerTests
    {
        private ProjetosController? _controller;
        private ApplicationDbContext? _context;
        private ObjectCreator? _objectCreator;
        private DbCache? _cache;

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

            _controller = new ProjetosController(_context, _cache);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "username"),
                //new Claim(type: "Cargo", value: _objectCreator.usuario.Cargo?.ToString()),
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
        public async Task Test_Edit_ValidModel_ReturnsRedirectToAction()
        {
            await _context.Users.AddAsync(_objectCreator?.usuario);
            await _context.Projeto.AddAsync(_objectCreator?.projeto);
            await _context.SaveChangesAsync();

            var existingProjeto = await _context.Projeto.FindAsync(_objectCreator?.projeto.Id);

            existingProjeto.NomeProjeto = "Nome Alterado";
            existingProjeto.AreaPesquisa = LinhaPesquisa.QuimicaESustentabilidade;

            if (existingProjeto != null)
            {
                if (_context != null && _controller != null && _objectCreator != null)
                {
                    _context.Entry(existingProjeto).State = EntityState.Detached;

                    var result = await _controller.Edit(existingProjeto.Id, existingProjeto, "meuemail@firjan.com.br");
                    Assert.IsInstanceOf<RedirectToActionResult>(result);
                    var redirectResult = (RedirectToActionResult)result;
                    Assert.AreEqual("Index", redirectResult?.ActionName);
                }
            }
        }

        [Test]
        public async Task Test_Create_ValidModel_ReturnsRedirectToAction()
        {
            if (_context != null && _objectCreator != null)
            {
                await _context.Users.AddAsync(_objectCreator.usuario);
                await _context.SaveChangesAsync();
            }

            if (_controller != null && _objectCreator != null)
            {
                var result = await _controller.Create(_objectCreator.projeto, "meuemail@firjan.com.br");

                Assert.IsInstanceOf<RedirectToActionResult>(result);
                var redirectResult = (RedirectToActionResult)result;
                Assert.AreEqual("Index", redirectResult?.ActionName);
            }
        }

        [Test]
        public void Test_Create_UserAuthenticated_ReturnsView()
        {
            if (_context != null && _objectCreator != null)
            {
                var empresa = _objectCreator.empresa;

                _context.Empresa.Add(empresa);

                _context.SaveChanges();
            }

            if (_controller != null)
            {
                var result = _controller.Create();
                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [TearDown]
        public void Teardown()
        {
            _context?.Database.EnsureDeleted();
            _context = null;
            _controller = null;
            _objectCreator = null;
        }
    }
}