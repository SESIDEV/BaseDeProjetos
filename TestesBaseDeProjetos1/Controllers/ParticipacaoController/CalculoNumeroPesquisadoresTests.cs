using BaseDeProjetos.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseDeProjetos.Controllers.Tests.ParticipacaoControllerTests
{
    internal class CalculoNumeroPesquisadoresTests
    {
        private static Dictionary<int, int> pesquisadores = new Dictionary<int, int>
        {
            {2021, 20},
            {2022, 20},
            {2023, 23}
        };

        [Test]
        public void Test_ThrowsException_AnoInicialMaiorQueAnoFinal()
        {
            Assert.Throws<ArgumentException>(() => ParticipacaoController.CalculoNumeroPesquisadores(2020, 2019));
        }

        [Test]
        public void Test_ReturnsPesquisadoresForAnoFinal_WhenAnoInicialEqualsAnoFinal()
        {
            var result = ParticipacaoController.CalculoNumeroPesquisadores(2021, 2021);
            Assert.AreEqual(pesquisadores[2021], result);
        }

        [Test]
        public void Test_ReturnsSumOfPesquisadoresInRange_QuandoAnoInicialMenorQueAnoFinal()
        {
            var result = ParticipacaoController.CalculoNumeroPesquisadores(2021, 2022);
            Assert.AreEqual((pesquisadores[2021] + pesquisadores[2022]) / 2, result);
        }
    }
}
