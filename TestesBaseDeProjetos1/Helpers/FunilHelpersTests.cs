using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Helpers.Tests
{
    [TestFixture]
    public class FunilHelpersTests
    {
        [Test]
        public void Test_VerificarTemperatura_Quente()
        {
            int dias = 6;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-quente'>Quente: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(6).ToString();

            Assert.AreEqual(resultadoObtido, resultadoEsperado);
        }

        [Test]
        public void Test_VerificarTemperatura_Morno()
        {
            int dias = 7;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-morno'>Morno: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(7).ToString();

            Assert.AreEqual(resultadoObtido, resultadoEsperado);
        }

        [Test]
        public void Test_VerificarTemperatura_Esfriando()
        {
            int dias = 16;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-esfriando text-dark'>Esfriando: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoObtido, resultadoEsperado);
        }

        [Test]
        public void Test_VerificarTemperatura_Frio()
        {
            int dias = 31;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-frio text-dark'>Frio: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoObtido, resultadoEsperado);
        }

        [Test]
        public void Test_VerificarTemperatura_Congelado()
        {
            int dias = 366;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-frio text-dark'>Congelado: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoObtido, resultadoEsperado);
        }
    }
}