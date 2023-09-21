using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class ParticipacaoController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private Dictionary<string, float> despesas = new Dictionary<string, float>
        {
            {"2021", 290000.0f},
            {"2022", 400000.0f},
            {"2023", 440000.0f}
        };

        private Dictionary<string, int> pesquisadores = new Dictionary<string, int>
        {
            {"2021", 20},
            {"2022", 20},
            {"2023", 23}
        };

        public ParticipacaoController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Efetua o cálculo relativo à participação de cada tipo de membro de acordo com a quantidade de membros de uma equipe
        /// </summary>
        /// <param name="pesquisadores">Número de Pesquisadores no Projeto</param>
        /// <param name="bolsistas">Número de Bolsistas no Projeto</param>
        /// <param name="estagiarios">Número de Estagiários no Projeto</param>
        /// <returns></returns>
        private Dictionary<string, decimal> CalculoParticipacao(int pesquisadores, int bolsistas, int estagiarios)
        {
            int numeroMembros = 1 + pesquisadores + bolsistas + estagiarios;

            decimal valorPorPesquisador = CalculoValorPesquisador(numeroMembros, pesquisadores);
            decimal valorPorBolsista = CalculoValorBolsista(numeroMembros, bolsistas);
            decimal valorPorEstagiario = CalculoValorEstagiario(numeroMembros, estagiarios);

            decimal totalPesquisadores = valorPorPesquisador * pesquisadores;
            decimal totalBolsistas = valorPorBolsista * bolsistas;
            decimal totalEstagiarios = valorPorEstagiario * estagiarios;
            decimal totalLider = 1 - (totalBolsistas + totalEstagiarios + totalPesquisadores);

            //decimal totalProjeto = totalLider + totalBolsistas + totalEstagiarios + totalPesquisadores;

            return new Dictionary<string, decimal>
            {
                { "totalPesquisadores", totalPesquisadores },
                { "totalBolsistas", totalBolsistas },
                { "totalEstagiarios", totalEstagiarios },
                { "totalLider", totalLider },
                { "valorPorPesquisador", valorPorPesquisador },
                { "valorPorBolsista", valorPorBolsista },
                { "valorPorEstagiario", valorPorEstagiario },
            };
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao estagiário
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="estagiarios">Número de estagiários no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorEstagiario(decimal numeroMembros, decimal estagiarios)
        {
            if (estagiarios == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - 1 / numeroMembros) * 1 / 10 * (1 / (estagiarios + 1));
                return resultado;
            }
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao bolsista
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="bolsistas">Número de bolsistas no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorBolsista(decimal numeroMembros, decimal bolsistas)
        {
            if (bolsistas == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - (1 / numeroMembros)) * 3 / 10 * (1 / (bolsistas + 1));
                return resultado;
            }
        }

        /// <summary>
        /// Método para calculo da porcentagem relativa ao pesquisador
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="pesquisadores">Número de pesquisadores no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorPesquisador(decimal numeroMembros, decimal pesquisadores)
        {
            if (pesquisadores == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - (1 / numeroMembros)) * 3 / 5 * (1 / (pesquisadores + 1));
                return resultado;
            }
        }

        [HttpGet("Participacao/RetornarDadosGraficoTemporal/{idUsuario}")]
        public async Task<IActionResult> RetornarDadosGraficoTemporal(string idUsuario)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = new Usuario();

                if (!string.IsNullOrEmpty(idUsuario))
                {
                    usuario = await _context.Users.Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
                }

                var participacao = await GetParticipacaoTotalUsuario(usuario);

                if (participacao != null)
                {
                    return Ok(JsonConvert.SerializeObject(participacao));
                }
                else
                {
                    return Ok(null);
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        /// <summary>
        /// Retorna os dados para o gráfico de participação do usuário
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("Participacao/RetornarDadosGrafico")]
        public async Task<IActionResult> RetornarDadosGrafico()
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var participacoes = await GetParticipacoesTotaisUsuarios();
                List<decimal> rankingsMedios = new List<decimal>();

                if (participacoes.Count > 0)
                {
                    RankearParticipacoes(participacoes, true);
                    AcertarValorRankParticipacoes(participacoes);
                    rankingsMedios = ObterRankingsMedios(participacoes);
                }

                var participacaoUsuario = participacoes.FirstOrDefault(p => p.Lider == usuario);
                var participacaoSerialized = JsonConvert.SerializeObject(participacaoUsuario);
                var rankingsSerialized = JsonConvert.SerializeObject(rankingsMedios);

                var finalObject = $"{{\"Participacao\": {participacaoSerialized}, \"Rankings\": {rankingsSerialized}}}";


                if (usuario != null)
                {
                    return Ok(finalObject);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        private void DefinirValoresMinMax(List<ParticipacaoTotalViewModel> participacoes)
        {
            PropertyInfo[] atributos = typeof(ParticipacaoTotalViewModel).GetProperties();

            Dictionary<string, decimal> valoresMaximos = new Dictionary<string, decimal>();
            Dictionary<string, decimal> valoresMinimos = new Dictionary<string, decimal>();

            foreach (var property in atributos)
            {
                if (property.PropertyType == typeof(decimal))
                {
                    decimal valorMaximo = participacoes.Max(x => (decimal)property.GetValue(x));
                    decimal valorMinimo = participacoes.Min(x => (decimal)property.GetValue(x));

                    valoresMaximos[$"{property.Name}"] = valorMaximo;
                    valoresMinimos[$"{property.Name}"] = valorMinimo;
                }

                if (property.PropertyType == typeof(int))
                {
                    int valorMaximo = participacoes.Max(x => (int)property.GetValue(x));
                    int valorMinimo = participacoes.Min(x => (int)property.GetValue(x));

                    valoresMaximos[$"{property.Name}"] = (decimal)valorMaximo;
                    valoresMinimos[$"{property.Name}"] = (decimal)valorMinimo;
                }
            }

            ViewData["ValoresMaximos"] = valoresMaximos;
            ViewData["ValoresMinimos"] = valoresMinimos;
        }

        /// <summary>
        /// Obter as prospecções de um usuário, incluindo as que ele é participante
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private async Task<List<Prospeccao>> GetProspeccoesUsuario(Usuario usuario)
        {
            return await _context.Prospeccao.Where(p => p.Usuario == usuario || p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();
        }

        /// <summary>
        /// Percorre o range de data inicio até data fim e cria valores e labels
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="prospeccoesUsuario"></param>
        private void ComputarRangeData(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            DateTime dataInicial = prospeccoesUsuario.Min(p => p.Status.Min(f => f.Data));
            DateTime dataIterada = dataInicial;
            DateTime dataAtual = DateTime.Now;
            List<string> mesAno = new List<string>();
            List<decimal> valoresProposta = new List<decimal>();

            while (dataIterada <= dataAtual)
            {
                string dataFormatada = $"{dataIterada:MMM} {dataIterada.Year}";
                mesAno.Add(dataFormatada);
                decimal valorSomado = prospeccoesUsuario
                    .FindAll(p => p.Status
                    .Any(f => new DateTime(f.Data.Year, f.Data.Month, 1) <= new DateTime(dataIterada.Year, dataIterada.Month, 1)))
                    .Sum(p => p.ValorProposta);
                valoresProposta.Add(valorSomado);
                dataIterada = dataIterada.AddMonths(1);
            }

            participacao.Valores = valoresProposta;
            participacao.Labels = mesAno;
        }


        /// <summary>
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, string mesInicio = null, string anoInicio = null, string mesFim = null, string anoFim = null)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel() { Participacoes = new List<ParticipacaoViewModel>() };

            // Líder e Membro
            var prospeccoesUsuario = await GetProspeccoesUsuario(usuario);
            var prospeccoesUsuarioMembro = await GetProspeccoesUsuarioMembro(usuario);

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }
            else
            {
                ComputarRangeData(participacao, prospeccoesUsuario);
            }

            var prospeccoesUsuarioComProposta = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.ComProposta)).ToList();
            var prospeccoesUsuarioConvertidas = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida)).ToList();

            if (!string.IsNullOrEmpty(anoFim) && !string.IsNullOrEmpty(mesFim))
            {
                if (!string.IsNullOrEmpty(mesInicio) && !string.IsNullOrEmpty(anoInicio))
                {
                    prospeccoesUsuario = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuario);
                    prospeccoesUsuarioComProposta = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuarioComProposta);
                    prospeccoesUsuarioConvertidas = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuarioConvertidas);
                }
                else
                {
                    prospeccoesUsuario = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuario);
                    prospeccoesUsuarioComProposta = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuarioComProposta);
                    prospeccoesUsuarioConvertidas = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuarioConvertidas);
                }
            }

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }

            await AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes = 0;
            decimal valorTotalProspeccoesComProposta = 0;
            decimal valorMedioProspeccoes = 0;
            decimal valorMedioProspeccoesComProposta = 0;
            decimal valorMedioProspeccoesConvertidas = 0;
            decimal valorTotalProspeccoesConvertidas = 0;
            decimal taxaConversaoProposta;
            decimal taxaConversaoProjeto;
            int quantidadeProspeccoes;
            int quantidadeProspeccoesMembro;
            int quantidadeProspeccoesComProposta;
            int quantidadeProspeccoesConvertidas;

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes = ExtrairValorProspeccoes(prospeccoesUsuario);
            participacao.ValorTotalProspeccoesComProposta = valorTotalProspeccoesComProposta = ExtrairValorProspeccoes(prospeccoesUsuarioComProposta);
            participacao.ValorTotalProspeccoesConvertidas = valorTotalProspeccoesConvertidas = ExtrairValorProspeccoes(prospeccoesUsuarioConvertidas);

            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta = prospeccoesUsuarioComProposta.Count();
            participacao.QuantidadeProspeccoesProjeto = quantidadeProspeccoesConvertidas = prospeccoesUsuarioConvertidas.Count();
            participacao.QuantidadeProspeccoesMembro = quantidadeProspeccoesMembro = prospeccoesUsuarioMembro.Count();

            participacao.Lider = usuario;

            // Evita divisão por 0
            if (prospeccoesUsuario.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
                participacao.ValorMedioProspeccoesComProposta = 0;
                participacao.TaxaConversaoProposta = 0;
                participacao.TaxaConversaoProjeto = 0;
            }
            else
            {
                participacao.TaxaConversaoProposta = taxaConversaoProposta = quantidadeProspeccoesComProposta / (decimal)quantidadeProspeccoes;
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;

                if (quantidadeProspeccoesComProposta > 0)
                {
                    participacao.ValorMedioProspeccoesComProposta = valorMedioProspeccoesComProposta = valorTotalProspeccoesComProposta / quantidadeProspeccoesComProposta;
                }

                if (quantidadeProspeccoesConvertidas > 0)
                {
                    participacao.ValorMedioProspeccoesConvertidas = valorMedioProspeccoesConvertidas = valorTotalProspeccoesConvertidas / quantidadeProspeccoesConvertidas;
                }

                if (valorMedioProspeccoesConvertidas != 0)
                {
                    var calculoAbsoluto = Math.Abs((valorMedioProspeccoesConvertidas - valorMedioProspeccoesComProposta) / valorMedioProspeccoesComProposta);
                    // var calculoAbsoluto = ((valorMedioProspeccoesComProposta - valorMedioProspeccoesConvertidas) / valorMedioProspeccoesConvertidas);
                    if (calculoAbsoluto != 0)
                    {
                        participacao.Propositividade = 1 / calculoAbsoluto;
                    }
                }

                if (quantidadeProspeccoesComProposta != 0)
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = quantidadeProspeccoesConvertidas / (decimal)quantidadeProspeccoesComProposta;
                }
                else
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = 0;
                }

                // TODO: Preciso de valores de despesa do ISI
                participacao.FatorDeContribuicaoFinanceira = taxaConversaoProposta * valorMedioProspeccoesComProposta * quantidadeProspeccoesComProposta;
            }

            return participacao;
        }

        /// <summary>
        /// Extrai o valor total das prospecções considerando o valor da proposta se presente, se não, o valor estimado
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="valorTotalProspeccoes"></param>
        /// <returns></returns>
        private static decimal ExtrairValorProspeccoes(List<Prospeccao> prospeccoes)
        {
            decimal valorTotalProspeccoes = 0;
            foreach (var prospeccao in prospeccoes)
            {
                if (prospeccao.ValorProposta != 0)
                {
                    valorTotalProspeccoes += prospeccao.ValorProposta;
                }
                else
                {
                    valorTotalProspeccoes += prospeccao.ValorEstimado;
                }
            }

            return valorTotalProspeccoes;
        }

        /// <summary>
        /// Filtra as prospecções passadas por parâmetro com status até o mês e ano especificado
        /// </summary>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        private static List<Prospeccao> FiltrarProspeccoesPorPeriodo(string mesFim, string anoFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Data.Year <= int.Parse(anoFim) && f.Data.Month <= int.Parse(mesFim))).ToList();
        }

        /// <summary>
        /// Filtra as prospecções passadas por parâmtro com status do mês e ano inicial até o mês e ano final
        /// </summary>
        /// <param name="mesInicio"></param>
        /// <param name="anoInicio"></param>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        private static List<Prospeccao> FiltrarProspeccoesPorPeriodo(string mesInicio, string anoInicio, string mesFim, string anoFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Data.Year >= int.Parse(anoInicio) && f.Data.Year <= int.Parse(anoFim) && f.Data.Month >= int.Parse(mesInicio) && f.Data.Month <= int.Parse(mesFim))).ToList();
        }

        /// <summary>
        /// Obter todas as prospecções em que o usuário é um membro (apenas)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private async Task<List<Prospeccao>> GetProspeccoesUsuarioMembro(Usuario usuario)
        {
            // Somente membro
            return await _context.Prospeccao.Where(p => p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();
        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário</param>
        private async Task AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            List<Usuario> usuarios = await _context.Users.ToListAsync();

            foreach (var prospeccao in prospeccoesUsuario)
            {
                List<Usuario> membrosEquipe = new List<Usuario>();

                bool prospConvertida = false;
                bool prospPlanejada = false;
                bool prospSuspensa = false;
                bool prospNaoConvertida = false;
                bool prospEmDiscussao = false;
                bool prospComProposta = false;

                int qtdBolsistas = 0;
                int qtdPesquisadores = 0;
                int qtdEstagiarios = 0;
                int qtdMembros = 0; // Não inclui o líder

                string nomeProjeto = !string.IsNullOrEmpty(prospeccao.NomeProspeccao) ? prospeccao.NomeProspeccao : $"{prospeccao.Empresa.Nome} (Empresa)";

                // Tratar prospecções que tem "projeto" no nome (...)
                // i.e: Remover na hora de apresentar o nome casos em que temos "Projeto projeto XYZ"
                // TODO: Transformar em REGEX
                if (!string.IsNullOrEmpty(nomeProjeto) && nomeProjeto.ToLowerInvariant().Contains("projeto") && !nomeProjeto.ToLowerInvariant().Contains("projetos"))
                {
                    nomeProjeto = nomeProjeto.Replace("projeto", "");
                    nomeProjeto = nomeProjeto.Replace("Projeto", "");
                }

                List<string> membrosNaoTratados = new List<string>();

                if (!string.IsNullOrEmpty(prospeccao.MembrosEquipe))
                {
                    membrosNaoTratados = prospeccao.MembrosEquipe.Split(";").ToList();

                    foreach (var membro in membrosNaoTratados)
                    {
                        if (!string.IsNullOrEmpty(membro))
                        {
                            Usuario usuarioEquivalente = usuarios.Find(u => u.UserName == membro);
                            if (usuarioEquivalente != null)
                            {
                                membrosEquipe.Add(usuarioEquivalente);
                            }
                        }
                    }

                    qtdBolsistas = membrosEquipe.Count(u => u.Cargo == Cargo.PesquisadorBolsista);
                    qtdEstagiarios = membrosEquipe.Count(u => u.Cargo == Cargo.EstagiarioNivelSuperior || u.Cargo == Cargo.EstagiarioNivelTecnico);
                    qtdPesquisadores = membrosEquipe.Count(u => u.Cargo == Cargo.PesquisadorQMS);

                    qtdMembros = qtdBolsistas + qtdEstagiarios + qtdPesquisadores;
                }

                Dictionary<string, decimal> calculoParticipantes = CalculoParticipacao(qtdPesquisadores, qtdBolsistas, qtdEstagiarios);

                var valorLider = calculoParticipantes["totalLider"] * prospeccao.ValorProposta;
                var valorPesquisadores = calculoParticipantes["totalPesquisadores"] * prospeccao.ValorProposta;
                var valorBolsistas = calculoParticipantes["totalBolsistas"] * prospeccao.ValorProposta;
                var valorEstagiarios = calculoParticipantes["totalEstagiarios"] * prospeccao.ValorProposta;
                var valorPorBolsista = calculoParticipantes["valorPorBolsista"] * prospeccao.ValorProposta;
                var valorPorPesquisador = calculoParticipantes["valorPorPesquisador"] * prospeccao.ValorProposta;
                var valorPorEstagiario = calculoParticipantes["valorPorEstagiario"] * prospeccao.ValorProposta;

                if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida)
                {
                    prospConvertida = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Planejada)
                {
                    prospPlanejada = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Suspensa)
                {
                    prospSuspensa = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.NaoConvertida)
                {
                    prospNaoConvertida = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ContatoInicial ||
                    prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Discussao_EsbocoProjeto)
                {
                    prospEmDiscussao = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta)
                {
                    prospComProposta = true;
                }
                else
                {
                    prospSuspensa = false;
                    prospConvertida = false;
                    prospPlanejada = false;
                    prospNaoConvertida = false;
                    prospEmDiscussao = false;
                    prospComProposta = false;
                }

                participacao.Participacoes.Add(new ParticipacaoViewModel()
                {
                    Id = Guid.NewGuid(),
                    NomeProjeto = nomeProjeto,
                    EmpresaProjeto = prospeccao.Empresa.Nome,
                    ComProposta = prospComProposta,
                    EmDiscussao = prospEmDiscussao,
                    NaoConvertida = prospNaoConvertida,
                    Convertida = prospConvertida,
                    Planejada = prospPlanejada,
                    Suspensa = prospSuspensa,
                    ValorNominal = prospeccao.ValorProposta,
                    MembrosEquipe = prospeccao.MembrosEquipe,
                    ValorLider = valorLider,
                    ValorPesquisadores = valorPesquisadores,
                    ValorBolsistas = valorBolsistas,
                    ValorEstagiarios = valorEstagiarios,
                    ValorPorBolsista = valorPorBolsista,
                    ValorPorEstagiario = valorPorEstagiario,
                    ValorPorPesquisador = valorPorPesquisador,
                    QuantidadeBolsistas = qtdBolsistas,
                    QuantidadeEstagiarios = qtdEstagiarios,
                    QuantidadePesquisadores = qtdPesquisadores,
                    QuantidadeMembros = qtdMembros + 1 // 1 == Líder
                });
            }
        }

        /// <summary>
        /// Obtém uma lista de participações de todos os usuários, com base na casa do usuário que está acessando.
        /// </summary>
        /// <returns></returns>
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(string mesInicio = null, string anoInicio = null, string mesFim = null, string anoFim = null)
        {
            Usuario usuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            List<Usuario> usuarios;

            // List<Prospeccao> prospeccoesTotais = _context.Prospeccao.ToList();

            if (usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
            {
                usuarios = await _context.Users.Where(u => u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO).ToListAsync();
            }
            else
            {
                usuarios = await _context.Users.Where(u => u.Casa == usuarioAtivo.Casa).ToListAsync();
            }

            // List<Prospeccao> prospeccoesUsuarios = prospeccoesTotais.Where(p => usuarios.Contains(p.Usuario)).ToList();

            List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();

            foreach (var usuario in usuarios)
            {
                var participacao = await GetParticipacaoTotalUsuario(usuario, mesInicio, anoInicio, mesFim, anoFim);

                if (participacao != null)
                {
                    participacoes.Add(participacao);
                }
            }

            return participacoes;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string mesInicio, string anoInicio, string mesFim, string anoFim)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewData["mesInicio"] = mesInicio;
            ViewData["anoInicio"] = anoInicio;
            ViewData["mesFim"] = mesFim;
            ViewData["anoFim"] = anoFim;

            var participacoes = await GetParticipacoesTotaisUsuarios(mesInicio, anoInicio, mesFim, anoFim);

            if (participacoes.Count > 0)
            {
                RankearParticipacoes(participacoes, false);
                AcertarValorRankParticipacoes(participacoes);
                //ObterRankingsMedios(participacoes);
                DefinirValoresMinMax(participacoes);

                participacoes = participacoes.OrderByDescending(p => p.MediaFatores).ToList();
            }

            ViewBag.usuarioFoto = usuario.Foto;
            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;
            ViewBag.usuarioId = usuario.Id;

            return View(participacoes);
        }

        private List<decimal> ObterRankingsMedios(List<ParticipacaoTotalViewModel> participacoes)
        {
            List<decimal> rankings = new List<decimal>();

            decimal rankMedio = participacoes.Average(p => p.MediaFatores);
            decimal rankMedioIndice = participacoes.Average(p => p.FatorDeContribuicaoFinanceira);

            decimal rankMedioValorTotalProspeccao = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoes"]);
            decimal rankMedioValorTotalProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoesComProposta"]);
            decimal rankMedioValorMedioProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoes"]);
            decimal rankMedioValorMedioProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoesComProposta"]);
            decimal rankMedioValorTotalProspeccoesConvertidas = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoesConvertidas"]);
            decimal rankMedioValorMedioProspeccoesConvertidas = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoesConvertidas"]);
            decimal rankMedioQuantidadeProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoes"]);
            decimal rankMedioQuantidadeProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesComProposta"]);
            decimal rankMedioQuantidadeProspeccoesProjeto = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesProjeto"]);
            decimal rankMedioQuantidadeProspeccoesMembro = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesMembro"]);
            decimal rankMedioPropositividade = participacoes.Average(p => p.RankPorIndicador["RankPropositividade"]);

            rankings.Add(rankMedio);
            rankings.Add(rankMedioIndice);
            rankings.Add(rankMedioValorTotalProspeccao);
            rankings.Add(rankMedioValorTotalProspeccoesComProposta);
            rankings.Add(rankMedioValorMedioProspeccoes);
            rankings.Add(rankMedioValorMedioProspeccoesComProposta);
            rankings.Add(rankMedioValorTotalProspeccoesConvertidas);
            rankings.Add(rankMedioValorMedioProspeccoesConvertidas);
            rankings.Add(rankMedioQuantidadeProspeccoes);
            rankings.Add(rankMedioQuantidadeProspeccoesComProposta);
            rankings.Add(rankMedioQuantidadeProspeccoesProjeto);
            rankings.Add(rankMedioQuantidadeProspeccoesMembro);
            rankings.Add(rankMedioPropositividade);

            ViewData[nameof(rankMedio)] = rankMedio;
            ViewData[nameof(rankMedioIndice)] = rankMedioIndice;
            ViewData[nameof(rankMedioValorTotalProspeccao)] = rankMedioValorTotalProspeccao;
            ViewData[nameof(rankMedioValorTotalProspeccoesComProposta)] = rankMedioValorTotalProspeccoesComProposta;
            ViewData[nameof(rankMedioValorMedioProspeccoes)] = rankMedioValorMedioProspeccoes;
            ViewData[nameof(rankMedioValorMedioProspeccoesComProposta)] = rankMedioValorMedioProspeccoesComProposta;
            ViewData[nameof(rankMedioValorTotalProspeccoesConvertidas)] = rankMedioValorTotalProspeccoesConvertidas;
            ViewData[nameof(rankMedioValorTotalProspeccoesConvertidas)] = rankMedioValorMedioProspeccoesConvertidas;
            ViewData[nameof(rankMedioQuantidadeProspeccoes)] = rankMedioQuantidadeProspeccoes;
            ViewData[nameof(rankMedioQuantidadeProspeccoesComProposta)] = rankMedioQuantidadeProspeccoesComProposta;
            ViewData[nameof(rankMedioQuantidadeProspeccoesProjeto)] = rankMedioQuantidadeProspeccoesProjeto;
            ViewData[nameof(rankMedioQuantidadeProspeccoesMembro)] = rankMedioQuantidadeProspeccoesMembro;
            ViewData[nameof(rankMedioPropositividade)] = rankMedioPropositividade;

            return rankings;
        }

        private void AcertarValorRankParticipacoes(List<ParticipacaoTotalViewModel> participacoes)
        {
            decimal mediaFatorContribuicaoFinanceira = participacoes.Average(p => p.FatorDeContribuicaoFinanceira);

            foreach (var participacao in participacoes)
            {
                if (mediaFatorContribuicaoFinanceira != 0)
                {
                    participacao.FatorDeContribuicaoFinanceira = participacao.FatorDeContribuicaoFinanceira / mediaFatorContribuicaoFinanceira;
                }
                else
                {
                    participacao.FatorDeContribuicaoFinanceira = 0;
                }
            }
        }

        /// <summary>
        /// Atribui os rankings as participações passadas por parâmetro, para que sejam exibidas na View. Valores de 0 a 1
        /// </summary>
        /// <param name="participacoes">Lista de participações (normalmente de um usuário específico mas pode ser genérica)</param>
        private static void RankearParticipacoes(List<ParticipacaoTotalViewModel> participacoes, bool rankPorMaximo)
        {
            decimal RankValorTotalProspeccoesComProposta = 0;
            decimal RankValorTotalProspeccoesConvertidas = 0;
            decimal RankValorMedioProspeccoesComProposta = 0;
            decimal RankValorMedioProspeccoesConvertidas = 0;
            decimal RankQuantidadeProspeccoesProjeto = 0;
            decimal RankQuantidadeProspeccoes = 0;
            decimal RankQuantidadeProspeccoesMembro = 0;
            decimal RankPropositividade = 0;

            decimal medValorTotalProsp = participacoes.Average(p => p.ValorTotalProspeccoes);
            decimal medValorMedioProsp = participacoes.Average(p => p.ValorMedioProspeccoes);
            decimal medValorMedioProspComProposta = participacoes.Average(p => p.ValorMedioProspeccoesComProposta);
            decimal medValorMedioProspConvertidas = participacoes.Average(p => p.ValorMedioProspeccoesConvertidas);
            decimal medTotalProspComProposta = participacoes.Average(p => p.ValorTotalProspeccoesComProposta);
            decimal medTotalProspConvertidas = participacoes.Average(p => p.ValorTotalProspeccoesConvertidas);
            decimal medQtdProspeccoes = (decimal)participacoes.Average(p => p.QuantidadeProspeccoes);
            decimal medQtdProspeccoesMembro = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesMembro);
            decimal medQtdProspeccoesComProposta = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesComProposta);
            decimal medQtdProspProjetizadas = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesProjeto);
            decimal medConversaoProjeto = participacoes.Average(p => p.TaxaConversaoProjeto);
            decimal medConversaoProposta = participacoes.Average(p => p.TaxaConversaoProposta);

            foreach (var participacao in participacoes)
            {
                participacao.RankPorIndicador = new Dictionary<string, decimal>();

                decimal RankQuantidadeProspeccoesComProposta = 0;
                decimal RankValorTotalProspeccoes = 0;
                decimal RankValorMedioProspeccoes = 0;

                CalcularMediaFatores(participacoes, participacao);

                if (medValorTotalProsp != 0)
                {
                    RankValorTotalProspeccoes = rankPorMaximo ? participacao.ValorTotalProspeccoes / participacoes.Max(p => p.ValorTotalProspeccoes) : participacao.ValorTotalProspeccoes / medValorTotalProsp;
                }
                if (medValorMedioProsp != 0)
                {
                    RankValorMedioProspeccoes = rankPorMaximo ? participacao.ValorMedioProspeccoes / participacoes.Max(p => p.ValorMedioProspeccoes) : participacao.ValorMedioProspeccoes / medValorMedioProsp;
                }
                if (medTotalProspComProposta != 0)
                {
                    RankValorTotalProspeccoesComProposta = rankPorMaximo ? participacao.ValorTotalProspeccoesComProposta / participacoes.Max(p => p.ValorTotalProspeccoesComProposta) : participacao.ValorTotalProspeccoesComProposta / medTotalProspComProposta;
                }
                if (medTotalProspConvertidas != 0)
                {
                    RankValorTotalProspeccoesConvertidas = rankPorMaximo ? participacao.ValorTotalProspeccoesConvertidas / participacoes.Max(p => p.ValorTotalProspeccoesConvertidas) : participacao.ValorTotalProspeccoesConvertidas / medTotalProspConvertidas;
                }
                if (medValorMedioProspComProposta != 0)
                {
                    RankValorMedioProspeccoesComProposta = rankPorMaximo ? participacao.ValorMedioProspeccoesComProposta / participacoes.Max(p => p.ValorMedioProspeccoesComProposta) : participacao.ValorMedioProspeccoesComProposta / medValorMedioProspComProposta;
                }
                if (medValorMedioProspConvertidas != 0)
                {
                    RankValorMedioProspeccoesConvertidas = rankPorMaximo ? participacao.ValorMedioProspeccoesConvertidas / participacoes.Max(p => p.ValorMedioProspeccoesConvertidas) : participacao.ValorMedioProspeccoesConvertidas / medValorMedioProspConvertidas;
                }
                if (medQtdProspeccoes != 0)
                {
                    RankQuantidadeProspeccoes = rankPorMaximo ? participacao.QuantidadeProspeccoes / (decimal)participacoes.Max(p => p.QuantidadeProspeccoes) : participacao.QuantidadeProspeccoes / medQtdProspeccoes;
                }
                if (medQtdProspProjetizadas != 0)
                {
                    RankQuantidadeProspeccoesProjeto = rankPorMaximo ? participacao.QuantidadeProspeccoesProjeto / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto) : participacao.QuantidadeProspeccoesProjeto / medQtdProspProjetizadas;
                }
                if (medQtdProspeccoesComProposta != 0)
                {
                    RankQuantidadeProspeccoesComProposta = rankPorMaximo ? participacao.QuantidadeProspeccoesComProposta / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta) : participacao.QuantidadeProspeccoesComProposta / medQtdProspeccoesComProposta;
                }
                if (medQtdProspeccoesMembro != 0)
                {
                    RankQuantidadeProspeccoesMembro = rankPorMaximo ? participacao.QuantidadeProspeccoesMembro / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro) : participacao.QuantidadeProspeccoesMembro / medQtdProspeccoesMembro;
                }
                if (participacoes.Average(p => p.Propositividade) != 0)
                {
                    RankPropositividade = rankPorMaximo ? participacao.Propositividade / participacoes.Max(p => p.Propositividade) : participacao.Propositividade / participacoes.Average(p => p.Propositividade);
                }

                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoes)] = RankValorTotalProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesComProposta)] = RankValorTotalProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesConvertidas)] = RankValorTotalProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoes)] = RankValorMedioProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesComProposta)] = RankValorMedioProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesConvertidas)] = RankValorMedioProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoes)] = RankQuantidadeProspeccoes;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesComProposta)] = RankQuantidadeProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesProjeto)] = RankQuantidadeProspeccoesProjeto;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesMembro)] = RankQuantidadeProspeccoesMembro;
                participacao.RankPorIndicador[nameof(RankPropositividade)] = RankPropositividade;
            }
        }

        /// <summary>
        /// Realiza o cálculo da média dos fatores
        /// </summary>
        /// <param name="participacoes"></param>
        /// <param name="participacao"></param>
        private static void CalcularMediaFatores(List<ParticipacaoTotalViewModel> participacoes, ParticipacaoTotalViewModel participacao)
        {
            decimal calculoMediaFatores = 0;

            if (participacoes.Max(p => p.ValorTotalProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.ValorTotalProspeccoes / participacoes.Max(p => p.ValorTotalProspeccoes);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoes / participacoes.Max(p => p.ValorMedioProspeccoes);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoes / (decimal)participacoes.Max(p => p.QuantidadeProspeccoes);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesProjeto / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesComProposta / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoesComProposta) != 0)
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoesComProposta / participacoes.Max(p => p.ValorMedioProspeccoesComProposta);
            }
            if (participacoes.Max(p => p.TaxaConversaoProjeto) != 0)
            {
                calculoMediaFatores += participacao.TaxaConversaoProjeto / participacoes.Max(p => p.TaxaConversaoProjeto);
            }
            if (participacoes.Max(p => p.TaxaConversaoProposta) != 0)
            {
                calculoMediaFatores += participacao.TaxaConversaoProposta / participacoes.Max(p => p.TaxaConversaoProposta);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesMembro / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro);
            }
            if (participacoes.Max(p => p.Propositividade) != 0)
            {
                calculoMediaFatores += participacao.Propositividade / participacoes.Max(p => p.Propositividade);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoesConvertidas != 0))
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoesConvertidas / participacoes.Max(p => p.ValorMedioProspeccoesConvertidas);
            }
            if (participacoes.Max(p => p.ValorTotalProspeccoesConvertidas != 0))
            {
                calculoMediaFatores += participacao.ValorTotalProspeccoesConvertidas / participacoes.Max(p => p.ValorTotalProspeccoesConvertidas);
            }

            participacao.MediaFatores = calculoMediaFatores /= 12;
        }
    }
}
