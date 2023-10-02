using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
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
        private ObjectCreator? _creator;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();

            _creator = new ObjectCreator();

            // Não mude a ordem!!
            _creator.CriarProjetoIndicadoresMock();
            _creator.CriarCargoMock();
            _creator.CriarUsuarioMock();
            _creator.CriarEmpresaMock();
            _creator.CriarProjetoMock();

            _controller = new ProjetosController(_context);

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
        public async Task Test_Create_ValidModel_ReturnsRedirectToAction()
        {
            if (_context != null && _creator != null)
            {
                _context.Users.Add(_creator.usuario);
                _context.SaveChanges();
            }

            if (_controller != null && _creator != null)
            {
                var result = await _controller.Create(_creator.projeto, "meuemail@firjan.com.br");
                Assert.IsInstanceOf<RedirectToActionResult>(result);
                var redirectResult = (RedirectToActionResult)result;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }


        [Test]
        public void Test_Create_UserAuthenticated_ReturnsView()
        {
            if (_context != null && _creator != null)
            {
                var empresa = _creator.empresa;

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
            _context = null;
            _controller = null;
            _creator = null;
        }
    }
}
