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
using System.Net;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ProjetoControllerEntregas : BaseTestes
    {
        private const string _IdValidoProjeto = "proj_637477823195206322";
        private const string _IdValidoEntrega = "seed_0";

        public ProjetoControllerEntregas(BaseApplicationFactory<Startup> factory) : base(factory)
        {
            factory.CreateClient();
        }

        [Fact]
        public async Task IncluirEntrega_Valido_Deve_Retonar_OK()
        {
            var response = await _client.GetAsync($"/Projetos/IncluirEntrega/{_IdValidoProjeto}");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("proj_")]
        public async Task IncluirEntrega_Invalido_Deve_Retornar404(string id)
        {
            var response = await _client.GetAsync("/Projetos/IncluirEntrega/" + id);
            Assert.False(response.IsSuccessStatusCode);
        }


        [Fact]
        public void IncluirEntrega_Deve_Retornar_View_Correta()
        {
            ProjetosController _controller = SetupController(base._context);
            var response = _controller.IncluirEntrega(_IdValidoProjeto) as ViewResult;
            Assert.Equal("IncluirEntrega", response.ViewName);
        }

        [Fact]
        public void IncluirEntrega_Deve_Possuir_Dados_Projeto()
        {
            var _controller = SetupController(_context);
            var response = _controller.IncluirEntrega(_IdValidoProjeto) as ViewResult;
            Projeto projeto = response.ViewData["Projeto"] as Projeto;
            Assert.Equal(_IdValidoProjeto, projeto.Id);
            Assert.Equal("Projeto teste", projeto.NomeProjeto);
            Assert.Equal("TESTE", projeto.NomeLider);

        }

        [Theory]
        [InlineData("")]
        [InlineData("proj_")]
        [InlineData(null)]
        public async Task IncluirEntrega_POST_Deve_Retornar404_Se_Id_Invalido(string id)
        {
            List<Entrega> entregas = ComEntregas(1, id, id);
            var entrega = new StringContent(JsonConvert.SerializeObject(entregas[0]), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Projetos/IncluirEntrega/" + id,
                                                   entrega);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task IncluirEntrega_POST_Deve_RetornarOK_Se_Id_valido()
        {

            //Setup
            Entrega entrega = ComEntregas(1, _IdValidoProjeto, _IdValidoProjeto)[0];
            var content = ToKeyValueURL(entrega);
            //Teste
            var response = await _client.PostAsync("/Projetos/IncluirEntrega/" + entrega.Id,
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
            var response = _controller.IncluirEntrega(entrega.Id,
                                                      entrega) as ViewResult;

            //Teste
            Assert.Equal(qtd_entregas_inicio + 1, _context.Entrega.Count());
            RemoverEntregaDeTeste(entrega);
        }


        [Fact]
        public async void EditarEntrega_GET_Deve_Retornar_OK_Se_IdValido()
        {
            var response = await _client.GetAsync($"/Projetos/EditarEntrega/{_IdValidoEntrega}");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void EditarEntrega_Deve_Alterar_Campos_Originais()
        {
            ProjetosController _controller = SetupController(_context);

            Entrega alterado = new Entrega {Id = _IdValidoEntrega, 
                                            NomeEntrega = "ABC",
                                            ProjetoId = _IdValidoProjeto,
                                            DescricaoEntrega = "EntregaAlterada",
                                            DataInicioEntrega = DateTime.Today,
                                            DataFim = DateTime.Today.AddDays(7) };

//            var response = await _controller.EditarEntrega(_IdValidoEntrega, alterado) as ViewResult;

            var content = ToKeyValueURL(alterado);
            //Teste
            var response = await _client.PostAsync("/Projetos/EditarEntrega/" + alterado.Id,
                                                   content);

            //Teste
            Entrega salva = _context.Entrega.FirstOrDefault(e=> e.Id == _IdValidoEntrega);
            Assert.Equal(alterado.Id, salva.Id);
            Assert.Equal(alterado.NomeEntrega, salva.NomeEntrega);
            Assert.Equal(alterado.ProjetoId, salva.ProjetoId);
            Assert.Equal(alterado.DescricaoEntrega, salva.DescricaoEntrega);
            Assert.Equal(alterado.DataInicioEntrega, salva.DataInicioEntrega);
            Assert.Equal(alterado.DataFim, salva.DataFim);
        }

        [Theory]
        [InlineData("")]
        [InlineData("!@#24231")]
        [InlineData(null)]
        public async void EditarEntrega_GET_Deve_Retornar_Erro_Se_IdInvalido(string id)
        {
            var response = await _client.GetAsync($"/Projetos/EditarEntrega/{id}");
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void RemoverEntrega_Deve_Retornar_OK_Se_IdValido()
        {
            var response = await _client.GetAsync($"/Projetos/RemoverEntrega/{_IdValidoEntrega}");
            Assert.True(response.IsSuccessStatusCode);

            //Teardown
            CriarSeedDB();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("proj_")]
       public async void RemoverEntrega_Deve_Retornar_404_Se_IdInvalido(string id)
        {
            var response = await _client.GetAsync($"/Projetos/RemoverEntrega/{id}");
            Assert.False(response.IsSuccessStatusCode);

            //Teardown
            CriarSeedDB();
        }


        [Fact]
        public async void RemoverEntrega_Deve_RemoverUmaEntrada()
        {
            //Setup
            CriarSeedDB();
            int pre_count = _context.Entrega.ToList().Count;
            var response = await _client.GetAsync($"/Projetos/RemoverEntrega/{_IdValidoEntrega}");

            //Test
            if (response.IsSuccessStatusCode)
            {
                var post_count = _context.Entrega.Count();
                Assert.Equal(pre_count - 1, post_count);
                return;
            }

            Assert.True(false, "Ocorreu uma falha na requisição");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("proj_")]
        public async void RemoverEntrega_Deve_Nao_Deve_Remover_Nada_Se_IdInvalido(string Id)
        {
            CriarSeedDB();
            var response = await _client.GetAsync($"/Projetos/RemoverEntrega/{Id}");
        }

        [Fact]
        public async void RemoverEntrega_Deve_Remover_A_Entrada_Correta()
        {
            //Setup
            CriarSeedDB();
            Entrega entrega = _context.Entrega.First(e => e.Id == "seed_0");
            var response = await _client.GetAsync($"/Projetos/RemoverEntrega/{_IdValidoEntrega}");

            //Test
            if (response.IsSuccessStatusCode)
            {
                Assert.DoesNotContain<Entrega>(entrega, _context.Entrega.ToList());
                return;
            }
            Assert.True(false, "Ocorreu uma falha na requisição");
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
            CriarSeedDB();
            return _controller;


        }

        private void CriarSeedDB()
        {
            Entrega seed = ComEntregas(1, _IdValidoProjeto, "seed_")[0];

            if (_context.Entrega.FirstOrDefault(e => e.Id == seed.Id) == null)
            {
                //Incluir uma entrega para testes
                _context.Entrega.Add(seed);
                _context.SaveChanges();
            }
        }

        private List<Entrega> ComEntregas(int qtd_entrega, string proj_id, string prefixo = "test_p")
        {

            List<Entrega> entregas = new List<Entrega>();
            for (var i = 0; i < qtd_entrega; i++)
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
            var entregas = ComEntregas(qtd_entrega, proj_id, proj_id);
            for (var i = 0; i < qtd_entrega; i++)
            {
                //Atrasando as entegas geradas
                entregas[i].DataInicioEntrega.AddDays(-2 * (i + 1));
            }

            return entregas;
        }
        private void RemoverEntregaDeTeste(Entrega entrega)
        {
            //Teardown
            _context.Entrega.Remove(entrega);
            _context.SaveChanges();
        }

    }

}