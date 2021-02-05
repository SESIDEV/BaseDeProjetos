using Xunit;
using Testes_BaseDeProjetos.Controllers;
using System.Threading.Tasks;
using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Linq;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ProjetoControllerEntregas : BaseTestes
    {
        private const string _IdValido = "proj_637477823195206322";

        public ProjetoControllerEntregas(BaseApplicationFactory<Startup> factory) : base(factory)
        {

            factory.CreateClient();
        }

        [Fact]
        public async Task IncluirEntrega_Valido_Deve_Retonar_OK()
        {
            var response = await _client.GetAsync("/Projetos/IncluirEntrega/proj_637477823195206322");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("proj_")]
        public async Task IncluirEntrega_Invalido_Deve_Retornar404(string id)
        {
            var response = await _client.GetAsync("/Projetos/IncluirEntrega/" + id);
            Assert.False(response.IsSuccessStatusCode);
        }


        [Fact]
        public async void IncluirEntrega_Deve_Retornar_View_Correta()
        {
            ProjetosController _controller = SetupController(base._context);
            var response = _controller.IncluirEntrega(_IdValido) as ViewResult;
            Assert.Equal("IncluirEntrega", response.ViewName);
        }

        [Fact]
        public void IncluirEntrega_Deve_Possuir_Dados_Projeto()
        {
            var _controller = SetupController(_context);
            var response = _controller.IncluirEntrega(_IdValido) as ViewResult;
            Projeto projeto = response.ViewData["Projeto"] as Projeto;
            Assert.Equal(_IdValido, projeto.Id);
            Assert.Equal("Projeto teste", projeto.NomeProjeto);
            Assert.Equal("TESTE", projeto.NomeLider);

        }

        [Theory]
        [InlineData("")]
        [InlineData("proj_")]
        public async Task IncluirEntrega_POST_Deve_Retornar404_Se_Id_Invalido(string id)
        {
            List<Entrega> entregas = ComEntregas(1, id);
            var entrega = new StringContent(JsonConvert.SerializeObject(entregas[0]), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Projetos/IncluirEntrega/" + id,
                                                   entrega);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task IncluirEntrega_POST_Deve_RetornarOK_Se_Id_valido()
        {
            Entrega entrega = ComEntregas(1, _IdValido)[0];
            string content = JsonConvert.SerializeObject(entrega);
            var entrega_json = new StringContent(content, Encoding.ASCII, "application/json");
            var response = await _client.PostAsync("/Projetos/IncluirEntrega/" + entrega.Id,
                                                   entrega_json);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public void IncluirEntrega_POST_Deve_Adicionar_Uma_Entrega_Ao_Banco()
        {
            //Setup do teste
            ProjetosController _controller = SetupController(_context);
            int qtd_entregas_inicio = _context.Entrega.Count();

            Entrega entrega = ComEntregas(1, _IdValido)[0];
            var response = _controller.IncluirEntrega(entrega.Id,
                                                      entrega) as ViewResult;

            //Teste
            Assert.Equal(qtd_entregas_inicio + 1, _context.Entrega.Count());

            //Teardown
            _context.Entrega.Remove(entrega);
            _context.SaveChanges();
        }


        /*
         * 
         *  Funções auxiliares de teste
         * 
         * 
         */


        private ProjetosController SetupController(ApplicationDbContext context)
        {
            var _controller = new ProjetosController(context);
            return _controller;


        }
        private List<Entrega> ComEntregas(int qtd_entrega, string proj_id)
        {

            List<Entrega> entregas = new List<Entrega>();
            for (var i = 0; i < qtd_entrega; i++)
            {
                entregas.Add(new Entrega
                {
                    Id = "Entrega_teste_" + i,
                    NomeEntrega = "Teste_" + i,
                    DataEntrega = DateTime.Today.AddDays(i),
                    DataFim = DateTime.Today.AddDays(i+1),
                    ProjetoId = proj_id,
                });
            }
            return entregas;
        }

        private List<Entrega> ComEntregasVencidas(int qtd_entrega, string proj_id)
        {
            var entregas = ComEntregas(qtd_entrega, proj_id);
            for (var i = 0; i < qtd_entrega; i++)
            {
                //Atrasando as entegas geradas
                entregas[i].DataEntrega.AddDays(-2 * (i + 1));
            }

            return entregas;
        }
    }
}