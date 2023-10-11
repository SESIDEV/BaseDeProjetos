using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using System;

namespace TestesBaseDeProjetos1.TestHelper
{
    internal class ObjectCreator
    {
        public ProjetoIndicadores projetoIndicadores;
        public Usuario usuario;
        public Empresa empresa;
        public Cargo cargo;
        public Projeto projeto;

        public void CriarEmpresaMock()
        {
            empresa = new Empresa()
            {
                Id = 1,
                Nome = "Minha Empresa",
                RazaoSocial = "Laticineos Ultimate, Inc",
                CNPJ = "12345678909876",
                Estado = Estado.Rondonia,
            };
        }

        public void CriarCargoMock()
        {
            cargo = new Cargo()
            {
                Id = 1,
                Nome = "Pesquisador",
                Salario = 7500
            };
        }

        public void CriarProjetoIndicadoresMock()
        {
            projetoIndicadores = new ProjetoIndicadores()
            {
                Id = "indicador_1",
                Bolsista = true
            };
        }

        public void CriarUsuarioMock()
        {
            usuario = new Usuario()
            {
                Id = "myUser",
                Cargo = cargo,
                Casa = Instituto.ISIQV,
                CargoId = 1,
                Competencia = "Alguma",
                Email = "meuemail@firjan.com.br",
                Foto = "",
                Matricula = 12312,
                Nivel = Nivel.Usuario,
                Vinculo = TipoVinculo.Empregado,
                EmailConfirmed = true,
                Titulacao = Titulacao.Graduado,
                UserName = "username", // Deve bater com o mock dos claims
            };
        }

        public void CriarProjetoMock()
        {
            projeto = new Projeto(projetoIndicadores)
            {
                Id = "proj_1",
                AreaPesquisa = LinhaPesquisa.Quimica40,
                Casa = Instituto.ISIQV,
                DataInicio = new DateTime(2023, 01, 01),
                DataEncerramento = new DateTime(2023, 12, 31),
                SatisfacaoClienteParcial = 0.97f,
                SatisfacaoClienteFinal = 0.74f,
                NomeProjeto = "Meu Projeto Teste",
                Empresa = empresa,
                Estado = Estado.RioDeJaneiro,
                Inovacao = TipoInovacao.Processo,
                Status = StatusProjeto.Contratado,
                DuracaoProjetoEmMeses = Helpers.DiferencaMeses(new DateTime(2023, 12, 31), new DateTime(2023, 01, 01), true),
                ValorTotalProjeto = 120000,
                Usuario = usuario,
                UsuarioId = usuario.Id,
            };
        }
    }
}