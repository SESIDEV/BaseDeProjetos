using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Helpers.Tests
{
    [TestFixture]
    public class FunilHelpersTests
    {
        private List<Producao> producoesMock = new List<Producao>();
        private static Prospeccao prospeccaoContatoInicial = new Prospeccao();
        private static Prospeccao prospeccaoEmDiscussao = new Prospeccao();
        private static Prospeccao prospeccaoComProposta = new Prospeccao();

        [SetUp]
        public void Setup_Tests_FiltrarProducoes()
        {
            producoesMock = new List<Producao> {
                new Producao() {
                    Autores = "Eu, Moq, teste"
                },
                new Producao() {
                    Casa = Instituto.ISIQV
                },
                new Producao() {
                    Descricao = "Descrição, descritiva, teste"
                },
                new Producao() {
                    Titulo = "Titulo, teste"
                }
            };
        }

        [Test]
        public void Test_VerificarTemperatura_Quente()
        {
            int dias = 6;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-quente'>Quente: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(6).ToString();

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_VerificarTemperatura_Morno()
        {
            int dias = 7;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-morno'>Morno: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(7).ToString();

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_VerificarTemperatura_Esfriando()
        {
            int dias = 16;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-esfriando text-dark'>Esfriando: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_VerificarTemperatura_Frio()
        {
            int dias = 31;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-frio text-dark'>Frio: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_VerificarTemperatura_Congelado()
        {
            int dias = 366;
            var resultadoEsperado = new HtmlString($"<span class='badge bg-frio text-dark'>Congelado: ({dias} Dias)</span>").ToString();

            var resultadoObtido = FunilHelpers.VerificarTemperatura(dias).ToString();

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_FiltrarProducoes_SearchStringVazia()
        {
            var resultadoEsperado = producoesMock;

            var resultadoObtido = FunilHelpers.FiltrarProduções("", producoesMock);

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_FiltrarProducoes_SearchTermoValido()
        {
            var resultadoEsperado = new List<Producao> { producoesMock[0] };

            var resultadoObtido = FunilHelpers.FiltrarProduções("Moq", producoesMock);

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_FiltrarProducoes_SearchTermoInvalido()
        {
            var resultadoEsperado = new List<Producao>();

            var resultadoObtido = FunilHelpers.FiltrarProduções("Esse termo não é valido de forma alguma", producoesMock);

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [Test]
        public void Test_FiltrarProducoes_SearchTermoNulo()
        {
            var resultadoEsperado = producoesMock;

            var resultadoObtido = FunilHelpers.FiltrarProduções(null, producoesMock);

            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        [SetUp]
        public void Setup_Test_VerificarStatus()
        {
            prospeccaoContatoInicial = new Prospeccao()
            {
                Id = "myProsp",
                Status = new List<FollowUp> { new FollowUp() { Id = 1, OrigemID = "myProsp", Status = StatusProspeccao.ContatoInicial } },
                NomeProspeccao = "prospStatusInicial"
            };

            prospeccaoEmDiscussao = new Prospeccao()
            {
                Id = "myProsp2",
                Status = new List<FollowUp> { new FollowUp() { Id = 2, OrigemID = "myProsp2", Status = StatusProspeccao.Discussao_EsbocoProjeto } },
                NomeProspeccao = "prospEmDiscussao"
            };

            prospeccaoComProposta = new Prospeccao
            {
                Id = "idProsp3",
                Status = new List<FollowUp> { new FollowUp { Id = 3, Status = StatusProspeccao.ComProposta, OrigemID = "idProsp3" } },
                NomeProspeccao = "prospComProposta"
            };
        }

        [TestCaseSource(nameof(VerificarStatus_EmDiscussao_Cases))]
        public void Test_VerificarStatus_EmDiscussao(Prospeccao prospeccao, bool resultadoEsperado)
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccao, StatusProspeccao.Discussao_EsbocoProjeto);
            Assert.AreEqual(resultadoEsperado, resultadoObtido);
        }

        public static IEnumerable<TestCaseData> VerificarStatus_EmDiscussao_Cases
        {
            get
            {
                yield return new TestCaseData(prospeccaoEmDiscussao, true);
                yield return new TestCaseData(prospeccaoContatoInicial, false);
                yield return new TestCaseData(prospeccaoComProposta, false);
            }
        }


        [Test]
        public void Test_VerificarStatus_ContatoInicial()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.ContatoInicial);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.ContatoInicial);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.ContatoInicial);

            Assert.AreEqual(false, resultadoObtido);
            Assert.AreEqual(true, resultadoObtido2);
            Assert.AreEqual(false, resultadoObtido3);
        }

        [Test]
        public void Test_VerificarStatus_ComProposta()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.ComProposta);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.ComProposta);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.ComProposta);

            Assert.AreEqual(false, resultadoObtido);
            Assert.AreEqual(false, resultadoObtido2);
            Assert.AreEqual(true, resultadoObtido3);
        }
    }
}