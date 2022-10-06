using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseDeProjetos.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using BaseDeProjetos.Models;
using BaseDeProjetos.Helpers;

namespace BaseDeProjetos.Controllers.Tests
{
    [TestClass()]
    public class FunilDeVendasControllerTests
    {
        [TestMethod()]
        public void FiltraProspeccoesPorCasaDoUsuario()
        {
            //A função receberá uma lista de prospecção e um usuário e retornará somente as prospecções que tem a mesma casa que o usuário

            //Etapa de Setup
            Usuario usuario = CriarUsuarioComCasa();

            
            var listaProsp = CriarListaDeProspeccaoComBaseEmInstitutos(new List<Instituto> { Instituto.ISIQV, Instituto.ISIII, Instituto.CISHO, Instituto.ISISVP });

            //Etapa de processamento
            List<Prospeccao> prosp = FunilHelpers.VincularCasaProspeccao(usuario, listaProsp);
            
            //Etapa de testes
            Assert.IsNotNull(prosp);
            Assert.AreEqual(1, prosp.Count);
            Prospeccao prosp2 = prosp[0];
            Assert.IsNotNull(prosp2);
            Assert.AreEqual(prosp2.Casa, usuario.Casa);
        }

        [TestMethod()]
        public void SupervisoresPodemVerMaisDeUmaCasaEmProspeccoes()
        {
            Assert.Fail();
        }

        private static Usuario CriarUsuarioComCasa()
        {
            Usuario usuario = new Usuario();
            usuario.Casa = Instituto.ISIQV;
            return usuario;
        }

        //A função receberá uma lista de institutos e retornará uma lista de prospecção referentes a estes institutos
        private static List<Prospeccao> CriarListaDeProspeccaoComBaseEmInstitutos(List<Instituto> listaInstituto)
        {
            List<Prospeccao> listaProsp = new List<Prospeccao>();
            foreach(Instituto instituto in listaInstituto)
            {
                Prospeccao prospeccao = new Prospeccao();
                prospeccao.Casa = instituto;
                listaProsp.Add(prospeccao);
            }
            return listaProsp;
        }

        private static void CriarListaDeProspeccao(List<Instituto> listaInstituto, Usuario usuario, int quantidade)
        {
            List<Prospeccao> listaProsp = new List<Prospeccao>();
        }
    }
}