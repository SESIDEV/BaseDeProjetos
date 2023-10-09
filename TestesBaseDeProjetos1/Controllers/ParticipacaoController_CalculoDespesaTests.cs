using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ParticipacaoController_CalculoDespesaTests
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
            Assert.AreEqual(despesas[2021] / 12, result);
        }

        [Test]
        public void Test_MesmoAnoMesDiferente()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 06, 2021);
            Assert.AreEqual(despesas[2021] / 12 * 6, result);
        }

        [Test]
        public void Test_AnoDiferenteMesmoMes()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 01, 2022);
            Assert.AreEqual(despesas[2021] + (despesas[2022] / 12), result);
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
            Assert.AreEqual((despesas[2021] / 12 * 7) + despesas[2022] + despesas[2023] / 2, result);
        }

        [Test]
        public void Test_MultiplosAnosDiferentesMesesDiferentes()
        {
            var result = ParticipacaoController.CalculoDespesa(01, 2021, 09, 2023);
            Assert.AreEqual(despesas[2021] + despesas[2022] + (despesas[2023] / 12 * 9), result);
        }

        [Test]
        public void Test_AnoNaoExistente()
        {
            Assert.Throws<KeyNotFoundException>(() => ParticipacaoController.CalculoDespesa(01, 2020, 01, 2021));
        }

        public void Test_AnoInicialMaiorQueFinal()
        {
            Assert.Throws<ArgumentException>(() => ParticipacaoController.CalculoDespesa(01, 2022, 01, 2023));
        }
    }
}
