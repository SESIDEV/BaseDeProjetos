using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Helpers.Tests
{
    [TestFixture]
    public class FunilHelpersTests
    {
        private List<Producao> producoesMock;
        private Prospeccao prospeccaoContatoInicial;
        private Prospeccao prospeccaoEmDiscussao;
        private Prospeccao prospeccaoComProposta;
        private Prospeccao prospeccaoConvertida;

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

            prospeccaoConvertida = new Prospeccao()
            {
                Id = "myProsp4",
                Status = new List<FollowUp> { new FollowUp() { Id = 4, OrigemID = "myProsp4", Status = StatusProspeccao.Convertida } },
                NomeProspeccao = "prospConvertida"
            };
        }

        [Test]
        public void Test_VerificarStatus_EmDiscussao()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.Discussao_EsbocoProjeto);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.Discussao_EsbocoProjeto);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.Discussao_EsbocoProjeto);
            var resultadoObtido4 = FunilHelpers.VerificarStatus(prospeccaoConvertida, StatusProspeccao.Discussao_EsbocoProjeto);

            Assert.AreEqual(true, resultadoObtido);
            Assert.AreEqual(false, resultadoObtido2);
            Assert.AreEqual(false, resultadoObtido3);
            Assert.AreEqual(false, resultadoObtido4);
        }

        [Test]
        public void Test_VerificarStatus_ContatoInicial()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.ContatoInicial);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.ContatoInicial);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.ContatoInicial);
            var resultadoObtido4 = FunilHelpers.VerificarStatus(prospeccaoConvertida, StatusProspeccao.ContatoInicial);

            Assert.AreEqual(false, resultadoObtido);
            Assert.AreEqual(true, resultadoObtido2);
            Assert.AreEqual(false, resultadoObtido3);
            Assert.AreEqual(false, resultadoObtido4);
        }

        [Test]
        public void Test_VerificarStatus_ComProposta()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.ComProposta);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.ComProposta);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.ComProposta);
            var resultadoObtido4 = FunilHelpers.VerificarStatus(prospeccaoConvertida, StatusProspeccao.ComProposta);

            Assert.AreEqual(false, resultadoObtido);
            Assert.AreEqual(false, resultadoObtido2);
            Assert.AreEqual(true, resultadoObtido3);
            Assert.AreEqual(false, resultadoObtido4);
        }

        [Test]
        public void Test_VerificarStatus_Convertida()
        {
            var resultadoObtido = FunilHelpers.VerificarStatus(prospeccaoEmDiscussao, StatusProspeccao.Convertida);
            var resultadoObtido2 = FunilHelpers.VerificarStatus(prospeccaoContatoInicial, StatusProspeccao.Convertida);
            var resultadoObtido3 = FunilHelpers.VerificarStatus(prospeccaoComProposta, StatusProspeccao.Convertida);
            var resultadoObtido4 = FunilHelpers.VerificarStatus(prospeccaoConvertida, StatusProspeccao.Convertida);

            Assert.AreEqual(false, resultadoObtido);
            Assert.AreEqual(false, resultadoObtido2);
            Assert.AreEqual(false, resultadoObtido3);
            Assert.AreEqual(true, resultadoObtido4);
        }
    }
}