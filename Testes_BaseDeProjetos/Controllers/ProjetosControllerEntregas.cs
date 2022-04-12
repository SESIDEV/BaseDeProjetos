using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Testes_BaseDeProjetos.Controllers;
using Xunit;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ProjetosControllerEntregas : BaseTestes
    {
        private const string _IdValidoProjeto = "proj_637477823195206322";
        private const string _IdValidoEntrega = "seed_0";
        private readonly ApplicationDbContext context;

        public ProjetosControllerEntregas(BaseApplicationFactory<Startup> factory) : base(factory)
        {
            factory.CreateClient();
        }

        [Fact]
        public async Task IncluirEntrega_Valido_Deve_Retonar_OK()
        {
            HttpResponseMessage response = await _client.GetAsync("/Projetos/IncluirEntrega/proj_637477823195206322");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("proj_")]
        public async Task IncluirEntrega_Invalido_Deve_Retornar404(string id)
        {
            HttpResponseMessage response = await _client.GetAsync("/Projetos/IncluirEntrega/" + id);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public void IncluirEntrega_Deve_Retornar_View_Correta()
        {
            ProjetosController _controller = SetupController(base._context);
            ViewResult response = _controller.IncluirEntrega(_IdValidoProjeto) as ViewResult;
            Assert.Equal("IncluirEntrega", response.ViewName);
        }

        [Fact]
        public void IncluirEntrega_Deve_Possuir_Dados_Projeto()
        {
            ProjetosController _controller = SetupController(_context);
            ViewResult response = _controller.IncluirEntrega(_IdValidoProjeto) as ViewResult;
            Projeto projeto = response.ViewData["Projeto"] as Projeto;
            Assert.Equal(_IdValidoProjeto, projeto.Id);
            Assert.Equal("Projeto teste", projeto.NomeProjeto);
            Assert.Equal("TESTE", projeto.NomeLider);
        }

        [Theory]
        [InlineData("")]
        [InlineData("proj_")]
        public async Task IncluirEntrega_POST_Deve_Retornar404_Se_Id_Invalido(string id)
        {
            List<Entrega> entregas = ComEntregas(1, id, id);
            StringContent entrega = new StringContent(JsonConvert.SerializeObject(entregas[0]), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/Projetos/IncluirEntrega/" + id,
                                                   entrega);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task IncluirEntrega_POST_Deve_RetornarOK_Se_Id_valido()
        {
            //Setup
            Entrega entrega = ComEntregas(1, _IdValidoProjeto, _IdValidoProjeto)[0];
            FormUrlEncodedContent content = ToKeyValueURL(entrega);
            //Teste
            HttpResponseMessage response = await _client.PostAsync("/Projetos/IncluirEntrega/" + entrega.Id,
                                                   content);
            Assert.True(response.IsSuccessStatusCode);

            //Teardown
            RemoverEntregaDeTeste(entrega);
        }

        [Fact]
        public void IncluirEntrega_POST_Deve_Adicionar_Uma_Entrega_Ao_Banco()
        {
            //Setup do teste
            ProjetosController _controller = SetupController(_context);
            int qtd_entregas_inicio = _context.Entrega.Count();

            Entrega entrega = ComEntregas(1, _IdValidoProjeto, _IdValidoProjeto)[0];
            ViewResult response = _controller.IncluirEntrega(entrega.Id,
                                                      entrega) as ViewResult;

            //Teste
            Assert.Equal(qtd_entregas_inicio + 1, _context.Entrega.Count());
            RemoverEntregaDeTeste(entrega);
        }

        private void RemoverEntregaDeTeste(Entrega entrega)
        {
            //Teardown
            _context.Entrega.Remove(entrega);
            _context.SaveChanges();
        }

        [Fact]
        public async void EditarEntrega_GET_Deve_Retornar_OK_Se_IdValido()
        {
            HttpResponseMessage response = await _client.GetAsync($"/Projetos/EditarEntrega/{_IdValidoEntrega}");
            Assert.True(response.IsSuccessStatusCode);
        }

        /*
         *
         *  Funções auxiliares de teste
         *
         *
         */

        private ProjetosController SetupController(ApplicationDbContext context)
        {
            ProjetosController _controller = new ProjetosController(context);
            Entrega seed = ComEntregas(1, _IdValidoProjeto, "seed_")[0];

            if (_context.Entrega.FirstOrDefault(e => e.Id == seed.Id) == null)
            {
                //Incluir uma entrega para testes
                _context.Entrega.Add(seed);
                _context.SaveChanges();
            }

            return _controller;
        }

        private List<Entrega> ComEntregas(int qtd_entrega, string proj_id, string prefixo = "test_p")
        {
            List<Entrega> entregas = new List<Entrega>();
            for (int i = 0; i < qtd_entrega; i++)
            {
                entregas.Add(new Entrega
                {
                    Id = prefixo + i,
                    NomeEntrega = "Teste_" + i,
                    DataInicioEntrega = DateTime.Today.AddDays(i),
                    DataFim = DateTime.Today.AddDays(i + 1),
                    ProjetoId = proj_id,
                });
            }
            return entregas;
        }

        private List<Entrega> ComEntregasVencidas(int qtd_entrega, string proj_id)
        {
            List<Entrega> entregas = ComEntregas(qtd_entrega, proj_id, proj_id);
            for (int i = 0; i < qtd_entrega; i++)
            {
                //Atrasando as entegas geradas
                entregas[i].DataInicioEntrega.AddDays(-2 * (i + 1));
            }

            return entregas;
        }
    }
}