using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ParticipacaoControllerTests
    {
        // Assuming this dictionary represents the annual expenses for certain years
        private Dictionary<int, decimal> despesas = new Dictionary<int, decimal>
        {
            {2021, 290000},
            {2022 , 400000},
            {2023, 440000}
        };

        [Test]
        public void Test_MesmoAnoMesmoMes()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 01, 2021);
            Assert.AreEqual(290000M / 12, result);
        }

        [Test]
        public void Test_MesmoAnoMesDiferente() // TODO: Verificar
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 06, 2021);
            Assert.AreEqual(290000M / 2, result); // Assuming the monthly expense for 2021 is 100 (1200/12)
        }

        [Test]
        public void Test_AnoDiferenteMesmoMes()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 01, 2022);
            Assert.AreEqual(290000M + (400000M / 12), result);
        }

        [Test]
        public void Test_AnoDiferenteMesDiferente()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 06, 2022);
            Assert.AreEqual(despesas[2021] + despesas[2022] / 2, result);
        }

        [Test]
        public void Test_MultiplosAnosDiferentesMesIgual()
        {
            var result = ParticipacaoController.CalculoDespesa(06, 2021, 06, 2023);
            Assert.AreEqual(despesas[2021] / 12 * 7 + despesas[2022] + despesas[2023] / 2, result);
        }

        [Test]
        public void Test_MultiplosAnosDiferentesMesesDiferentes()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 09, 2023);
            Assert.AreEqual(despesas[2021] + despesas[2022] + despesas[2023] / 12 * 9, result);
        }

        [Test]
        public void Test_AnoNaoExistente()
        {
            Assert.Throws<KeyNotFoundException>(() => ParticipacaoController.CalculoDespesa(01, 2020, 01, 2021)); // 2020 is not in the dictionary
        }
    }
}
