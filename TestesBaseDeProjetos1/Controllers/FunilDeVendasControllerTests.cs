using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
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
            var prospeccao = new Prospeccao
            {
                Status = new List<FollowUp>
                {
                    new FollowUp { Status = StatusProspeccao.ContatoInicial },
                    new FollowUp { Status = StatusProspeccao.NDAAssinado }
                }
            };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1" };
            var donoDaProspeccao = usuarioAtivoNaSessao;

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_ApenasContatoInicial()
        {
            var prospeccao = new Prospeccao
            {
                Status = new List<FollowUp>
                {
                    new FollowUp { Status = StatusProspeccao.ContatoInicial }
                }
            };
            var usuarioAtivoNaSessao = new Usuario();
            var donoDaProspeccao = usuarioAtivoNaSessao;

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_UsuarioDifereDono()
        {
            var prospeccao = new Prospeccao
            {
                Status = new List<FollowUp>
                {
                    new FollowUp { Status = StatusProspeccao.ContatoInicial },
                    new FollowUp { Status = StatusProspeccao.NDAAssinado }
                }
            };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1" };
            var donoDaProspeccao = new Usuario { Id = "meuId2" };

            var result = FunilDeVendasController.VerificarCondicoesRemocao(prospeccao, usuarioAtivoNaSessao, donoDaProspeccao);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_NaoRemoveContatoInicial()
        {
            var contatoInicial = new FollowUp { Status = StatusProspeccao.ContatoInicial };
            var prospeccao = new Prospeccao
            {
                Status = new List<FollowUp>
                {
                    contatoInicial,
                    new FollowUp { Status = StatusProspeccao.NDAAssinado }
                }
            };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1" };
            var donoDaProspeccao = new Usuario { Id = "meuId1" };

            var result = FunilDeVendasController.VerificarCondicoesRemocao(
                prospeccao,
                usuarioAtivoNaSessao,
                donoDaProspeccao,
                contatoInicial);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_RemoveStatusNaoInicial()
        {
            var statusPosterior = new FollowUp { Status = StatusProspeccao.NDAAssinado };
            var prospeccao = new Prospeccao
            {
                Status = new List<FollowUp>
                {
                    new FollowUp { Status = StatusProspeccao.ContatoInicial },
                    statusPosterior
                }
            };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1" };
            var donoDaProspeccao = new Usuario { Id = "meuId1" };

            var result = FunilDeVendasController.VerificarCondicoesRemocao(
                prospeccao,
                usuarioAtivoNaSessao,
                donoDaProspeccao,
                statusPosterior);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_VerificarCondicoesRemocao_RemoveQuandoUsuarioEhLiderNome()
        {
            var statusPosterior = new FollowUp { Status = StatusProspeccao.NDAAssinado };
            var prospeccao = new Prospeccao
            {
                LiderNome = "Usuario Teste",
                Status = new List<FollowUp>
                {
                    new FollowUp { Status = StatusProspeccao.ContatoInicial },
                    statusPosterior
                }
            };
            var usuarioAtivoNaSessao = new Usuario { Id = "meuId1", UserName = "Usuario Teste" };

            var result = FunilDeVendasController.VerificarCondicoesRemocao(
                prospeccao,
                usuarioAtivoNaSessao,
                null,
                statusPosterior);

            Assert.IsTrue(result);
        }
    }
}
