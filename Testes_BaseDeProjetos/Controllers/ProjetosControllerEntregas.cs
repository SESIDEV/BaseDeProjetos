using System.Net.Http;
using Xunit;
using Testes_BaseDeProjetos.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BaseDeProjetos.Controllers.Tests
{
    public class ProjetoControllerEntregas : BaseTestes
    {
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
            var response = await _controller.IncluirEntrega("proj_637477823195206322") as ViewResult;
            Assert.Equal("IncluirEntrega", response.ViewName);
        }

        [Fact]
        public async void IncluirEntrega_Deve_Possuir_Dados_Projeto()
        {
            var _controller = SetupController(_context);
            var response = await _controller.IncluirEntrega("proj_637477823195206322") as ViewResult;
            Projeto projeto = response.ViewData["Projeto"] as Projeto;
            Assert.Equal("proj_637477823195206322", projeto.Id);
            Assert.Equal("Projeto teste", projeto.NomeProjeto);
            Assert.Equal("TESTE", projeto.NomeLider);

        }

      
        private ProjetosController SetupController(ApplicationDbContext context)
        {
            var _controller = new ProjetosController(context);
            return _controller;
        }
    }
}