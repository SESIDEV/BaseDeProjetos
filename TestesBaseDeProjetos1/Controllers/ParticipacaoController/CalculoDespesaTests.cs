namespace BaseDeProjetos.Controllers.Tests.ParticipacaoControllerTests
{
    public class CalculoDespesaTests
    {
        //private ApplicationDbContext? _context;

        //private readonly Dictionary<int, decimal> despesas = new Dictionary<int, decimal>
        //{
        //    {2021, 290000},
        //    {2022, 400000},
        //    {2023, 440000}
        //};

        //[SetUp]
        //public void Setup()
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Separar o contexto do banco para cada teste
        //        .Options;

        //    if (_context == null)
        //    {
        //        _context = new ApplicationDbContext(options);
        //        _context.Database.EnsureCreated();
        //    }
        //}

        //[Test]
        //public async Task Test_MesmoAnoMesmoMes()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 01, 2021, 01, 2021);
        //    Assert.AreEqual(despesas[2021] / 12, result);
        //}

        //[Test]
        //public async Task Test_MesmoAnoMesDiferente()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 01, 2021, 06, 2021);
        //    Assert.AreEqual(despesas[2021] / 12 * 6, result);
        //}

        //[Test]
        //public async Task Test_AnoDiferenteMesmoMes()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 01, 2021, 01, 2022);
        //    Assert.AreEqual(despesas[2021] + (despesas[2022] / 12), result);
        //}

        //[Test]
        //public async Task Test_AnoDiferenteMesDiferente()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 01, 2021, 06, 2022);
        //    Assert.AreEqual(despesas[2021] + despesas[2022] / 2, result);
        //}

        //[Test]
        //public async Task Test_MultiplosAnosDiferentesMesIgual()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 06, 2021, 06, 2023);
        //    Assert.AreEqual((despesas[2021] / 12 * 7) + despesas[2022] + despesas[2023] / 2, result);
        //}

        //[Test]
        //public async Task Test_MultiplosAnosDiferentesMesesDiferentes()
        //{
        //    var result = await ParticipacaoController.CalculoDespesa(_context, 01, 2021, 09, 2023);
        //    Assert.AreEqual(despesas[2021] + despesas[2022] + (despesas[2023] / 12 * 9), result);
        //}

        //[Test]
        //public void Test_AnoNaoExistente()
        //{
        //    Assert.Throws<KeyNotFoundException>(() => ParticipacaoController.CalculoDespesa(_context, 01, 2020, 01, 2021).GetAwaiter().GetResult());
        //}

        //public void Test_AnoInicialMaiorQueFinal()
        //{
        //    Assert.Throws<ArgumentException>(() => ParticipacaoController.CalculoDespesa(_context, 01, 2022, 01, 2023).GetAwaiter().GetResult());
        //}
    }
}