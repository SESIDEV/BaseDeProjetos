using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Services
{
    public class AdministradorDadosParticipacao : IAdministradorDadosParticipacao
    {
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;
        private readonly IConstrutorParticipacao _construtorParticipacao;

        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private const string nomeCargoEstagiário = "Estagiário";
        private const string nomeCargoPesquisador = "Pesquisador QMS";

        public AdministradorDadosParticipacao(ApplicationDbContext context, DbCache cache, IConstrutorParticipacao construtor)
        {
            _context = context;
            _cache = cache;
            _construtorParticipacao = construtor;
        }

        /// <summary>
        /// Obtém a lista de prospecções do usuário passa por parâmetro nas quais ele é lider (somente)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioLider(Usuario usuario)
        {
            return _context.Prospeccao
                .Include(p => p.Status)
                .Where(p => p.Usuario.Id == usuario.Id && p.Status.OrderBy(f => f.Data)
                                                                  .LastOrDefault().Status != StatusProspeccao.Planejada).ToList();
        }


        /// <summary>
        /// Obter todas as prospecções em que o usuário é um membro (apenas)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioMembro(Usuario usuario)
        {
            // Somente membro
            return _context.Prospeccao.Include(p => p.EquipeProspeccao)
                                      .Include(p => p.Status)
                                      .Where(p => p.EquipeProspeccao != null
                                                  && p.EquipeProspeccao.Any(e => e.Usuario.Id == usuario.Id)
                                                  && (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault().Status != StatusProspeccao.Planejada))
                                      .ToList();
        }

        /// <summary>
        /// Obter as prospecções de um usuário, incluindo as que ele é participante
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioMembroEquipe(Usuario usuario)
        {
            return _context.Prospeccao
                .Include(p => p.EquipeProspeccao)
                .Include(p => p.Status)
                .Where(p =>
                (p.Usuario.Id == usuario.Id || p.EquipeProspeccao != null && p.EquipeProspeccao.Any(e => e.Usuario.Id == usuario.Id)) &&
                (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault().Status != StatusProspeccao.Planejada)
            ).ToList();
        }


        /// <summary>
        /// Obtém as prospecções do usuário para serem utilizadas em participação
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ProspeccoesUsuarioParticipacao ObterProspeccoesUsuario(Usuario usuario)
        {
            var prospeccoesTotais = GetProspeccoesUsuarioMembroEquipe(usuario);
            var prospeccoesTotaisConvertidas = ParticipacaoHelper.FiltrarProspeccoesConvertidas(prospeccoesTotais);
            var prospeccoesTotaisComProposta = ParticipacaoHelper.FiltrarProspeccoesComProposta(prospeccoesTotais);
            var prospeccoesLider = GetProspeccoesUsuarioLider(usuario);
            var prospeccoesLiderConvertidas = ParticipacaoHelper.FiltrarProspeccoesConvertidas(prospeccoesLider);
            var prospeccoesMembro = GetProspeccoesUsuarioMembro(usuario);

            var prospeccoesUsuario = new ProspeccoesUsuarioParticipacao
            {
                ProspeccoesLider = prospeccoesLider,
                ProspeccoesLiderConvertidas = prospeccoesLiderConvertidas,
                ProspeccoesMembro = prospeccoesMembro,
                ProspeccoesTotais = prospeccoesTotais,
                ProspeccoesTotaisConvertidas = prospeccoesTotaisConvertidas,
                ProspeccoesTotaisComProposta = prospeccoesTotaisComProposta,
            };
            return prospeccoesUsuario;
        }

        /// <summary>
        /// Obtém uma lista de participações de todos os usuários, com base na casa do usuário que está acessando.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(Usuario usuarioAtivo, DateTime dataInicio, DateTime dataFim)
        {
            List<UsuarioParticipacaoDTO> usuariosDTO;

            // Obs: Incluir pesquisadores da Q4.0 que façam prospecções abaixo. Eles possuem nível 3/2 logo podem acabar não apareçendo na listagem de participação
            if (usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
            {
                usuariosDTO = await _cache.GetCachedAsync($"Usuarios:Participacao:{dataInicio}:{dataFim}:{usuarioAtivo.Casa}", () =>
                    _context.Users.Select(u => new UsuarioParticipacaoDTO { Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id }, Casa = u.Casa, Email = u.Email, EmailConfirmed = u.EmailConfirmed, Nivel = u.Nivel, Id = u.Id, UserName = u.UserName })
                        .Where(u => (u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO) && u.Cargo.Nome == nomeCargoPesquisador && u.EmailConfirmed == true && u.Nivel == Nivel.Usuario || u.Email.Contains("lednascimento"))
                        .ToListAsync());
            }
            else
            {
                usuariosDTO = await _cache.GetCachedAsync($"Usuarios:Participacao:{dataInicio}:{dataFim}:{usuarioAtivo.Casa}", () =>
                    _context.Users.Select(u => new UsuarioParticipacaoDTO { Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id }, Casa = u.Casa, Email = u.Email, EmailConfirmed = u.EmailConfirmed, Nivel = u.Nivel, Id = u.Id, UserName = u.UserName })
                        .Where(u => u.Casa == usuarioAtivo.Casa && u.EmailConfirmed == true && u.Cargo.Nome == nomeCargoPesquisador && u.Nivel == Nivel.Usuario || u.Email.Contains("lednascimento"))
                        .ToListAsync());
            }

            List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();

            foreach (var usuarioDTO in usuariosDTO)
            {
                var participacao = await GetParticipacaoTotalUsuario(usuarioDTO.ToUsuario(), dataInicio, dataFim);

                if (participacao != null)
                {
                    participacoes.Add(participacao);
                }
            }

            return participacoes;
        }

        public async Task<List<Prospeccao>> ObterProspeccoesParaParticipacao()
        {
            return await _cache.GetCachedAsync("Prospeccoes:Participacao", () => _context.Prospeccao.Include(p => p.Status)
                                                                                                    .Include(p => p.Usuario)
                                                                                                    .Include(p => p.Empresa)
                                                                                                    .Include(p => p.EquipeProspeccao).ThenInclude(p => p.Usuario)
                                                                                                    .ToListAsync());
        }

        /// <summary>
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, DateTime dataInicio, DateTime dataFim)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel
            {
                Participacoes = new List<ParticipacaoViewModel>(),
                Lider = usuario.ToUsuarioParticipacao(),
            };

            ProspeccoesUsuarioParticipacao prospeccoesUsuario = ObterProspeccoesUsuario(usuario);

            ParticipacaoHelper.PeriodizarProspeccoesUsuario(prospeccoesUsuario, dataInicio, dataFim);

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.ProspeccoesTotais.Count == 0)
            {
                return null;
            }

            await _construtorParticipacao.AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario.ProspeccoesTotais);

            _construtorParticipacao.AtribuirQuantidadesDeProspeccao(usuario, participacao, prospeccoesUsuario);
            _construtorParticipacao.AtribuirValoresFinanceirosDeProspeccao(usuario, participacao, prospeccoesUsuario);

            await _construtorParticipacao.AtribuirAssertividadePrecificacao(usuario, dataInicio, dataFim, participacao, prospeccoesUsuario);

            // TODO: Código inutilizado??
            //double quantidadePesquisadores = CalculoNumeroPesquisadores(dataInicio.Year, dataFim.Year);

            return participacao;
        }
    }
}
