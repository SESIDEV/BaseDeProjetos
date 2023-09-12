using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Controllers.Tests
{
    [TestFixture]
    public class FunilDeVendasControllerTests
    {
        [Test]
        public void Test_VerificarCondicoesRemocao_UsuarioValidoDoisStatus()
        {
            var prospeccao = new Prospeccao { Status = new List<FollowUp> { new FollowUp(), new FollowUp() } };
            var usuarioAtivoNaSessao = new Usuario();
            var donoDaProspeccao = usuarioAtivoNaSessao;

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_ApenasContatoInicial()
        {
            var prospeccao = new Prospeccao { Status = new List<FollowUp> { new FollowUp() } };
            var usuarioAtivoNaSessao = new Usuario();
            var donoDaProspeccao = usuarioAtivoNaSessao;

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_UsuarioDifereDono()
        {
            var prospeccao = new Prospeccao { Status = new List<FollowUp> { new FollowUp(), new FollowUp() } };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1" };
            var donoDaProspeccao = new Usuario { Id = "meuId2" };

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsFalse(result);
        }

        
    }
}