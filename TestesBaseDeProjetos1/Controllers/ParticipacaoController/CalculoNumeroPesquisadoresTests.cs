using NUnit.Framework;

namespace BaseDeProjetos.Controllers.Tests.ParticipacaoControllerTests
{
    [TestFixture]
    internal class CalculoNumeroPesquisadoresTests
    {
        //private ApplicationDbContext _context;
        //private ObjectCreator _objectCreator;
        //private ParticipacaoController _controller;
        //private readonly ILogger<ParticipacaoController> _logger;
        //private readonly DbCache _cache;

        //private static readonly Dictionary<int, int> pesquisadores = new Dictionary<int, int>
        //{
        //    {2021, 20},
        //    {2022, 20},
        //    {2023, 23}
        //};

        //[SetUp]
        //public void Setup()
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Separar o contexto do banco para cada teste
        //        .Options;

        //    _context = new ApplicationDbContext(options);
        //    _context.Database.EnsureCreated();

        //    _objectCreator = new ObjectCreator();

        //    // Não mude a ordem!!
        //    _objectCreator.CriarProjetoIndicadoresMock();
        //    _objectCreator.CriarCargoMock();
        //    _objectCreator.CriarUsuarioMock();
        //    _objectCreator.CriarEmpresaMock();
        //    _objectCreator.CriarProjetoMock();

        //    _controller = new ParticipacaoController(_context, _cache, _logger);

        //    var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        //        new Claim(ClaimTypes.Name, "username"),
        //    }));

        //    var sessionMock = new Mock<ISession>();

        //    var httpContextMock = new Mock<HttpContext>();

        //    httpContextMock.Setup(x => x.User).Returns(user);
        //    httpContextMock.Setup(y => y.Session).Returns(sessionMock.Object);

        //    _controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = httpContextMock.Object,
        //    };
        //}

        //[Test]
        //public void Test_ThrowsException_AnoInicialMaiorQueAnoFinal()
        //{
        //    Assert.Throws<ArgumentException>(() => _controller.CalculoNumeroPesquisadores(2020, 2019));
        //}

        //[Test]
        //public void Test_ReturnsPesquisadoresForAnoFinal_WhenAnoInicialEqualsAnoFinal()
        //{
        //    var result = _controller.CalculoNumeroPesquisadores(2021, 2021);
        //    Assert.AreEqual(pesquisadores[2021], result);
        //}

        //[Test]
        //public void Test_ReturnsSumOfPesquisadoresInRange_QuandoAnoInicialMenorQueAnoFinal()
        //{
        //    var result = _controller.CalculoNumeroPesquisadores(2021, 2022);
        //    Assert.AreEqual((pesquisadores[2021] + pesquisadores[2022]) / 2, result);
        //}
    }
}