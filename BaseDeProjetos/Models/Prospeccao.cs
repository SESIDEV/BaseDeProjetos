﻿using BaseDeProjetos.Data;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BaseDeProjetos.Models
{
    public class Prospeccao : IEquatable<Prospeccao>
    {
        public Prospeccao(FollowUp followUp)
        {
            this.Status = new List<FollowUp> { followUp };
        }

        public Prospeccao()
        {
        }

        [Key]
        public virtual string Id { get; set; }

        [Display(Name = "Nome da prospecção ou potencial projeto")]
        public virtual string NomeProspeccao { get; set; }

        [Display(Name = "Potenciais Parceiros da Prospeccção")]
        public virtual string PotenciaisParceiros { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }

        public int EmpresaId { get; set; }
        public virtual Pessoa Contato { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Equipe")]
        public virtual string MembrosEquipe { get; set; }

        [Display(Name = "Tipo de Contratação")]
        public virtual TipoContratacao TipoContratacao { get; set; }

        [Display(Name = "Linha de Pesquisa")]
        public virtual LinhaPesquisa LinhaPequisa { get; set; }

        public virtual List<FollowUp> Status { get; set; } = new List<FollowUp>();
        public virtual Instituto Casa { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "R{0:C2}")]
        [Display(Name = "Valor da Proposta (R$)")]
        public virtual decimal ValorProposta { get; set; } = 0;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "R{0:C2}")]
        [Display(Name = "Valor Estimado da Prospecção (R$)")]
        public virtual decimal ValorEstimado { get; set; } = 0;

        public bool Equals([AllowNull] Prospeccao other)
        {
            if (other is null) return false;
            return other.Id == Id;
        }

        public override int GetHashCode() => (Id).GetHashCode();

        [Display(Name = "Caminho/Endereço da pasta no sistema")]
        public virtual string CaminhoPasta { get; set; }

        [Display(Name = "Tags")]
        public virtual string Tags { get; set; }

        [Display(Name = "Origem")]
        public virtual Origem Origem { get; set; }

        public virtual bool Ancora { get; set; }
        public virtual String Agregadas { get; set; }


		public List<Usuario> TratarMembrosEquipeString(ApplicationDbContext _context)
		{
			List<string> membrosNaoTratados = MembrosEquipe?.Split(";").ToList();
			List<UsuarioParticipacaoDTO> usuarios = _context.Users
				.Select(u => new UsuarioParticipacaoDTO
				{
					Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id },
					Casa = u.Casa,
					Email = u.Email,
					EmailConfirmed = u.EmailConfirmed,
					Nivel = u.Nivel,
					Id = u.Id,
					UserName = u.UserName
				})
				.ToList();

			List<Usuario> membrosEquipe = new List<Usuario>();

			if (membrosNaoTratados != null)
			{
				foreach (var membro in membrosNaoTratados)
				{
					if (!string.IsNullOrEmpty(membro))
					{
						Usuario usuarioEquivalente = usuarios.Find(u => u.Email == membro).ToUsuario();
						if (usuarioEquivalente != null)
						{
							membrosEquipe.Add(usuarioEquivalente);
						}
					}
				}
			}

			return membrosEquipe;
		}
	}

    public class FollowUp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("OrigemID")]
        public virtual Prospeccao Origem { get; set; }

        public virtual string OrigemID { get; set; }

        [Display(Name = "Anotações")]
        public virtual string Anotacoes { get; set; }

        public virtual DateTime Data { get; set; } = DateTime.Now;

        [Display(Name = "Ano da prospecção")]
        public virtual int AnoFiscal
        {
            get => Data.Year;
            set { }
        }

        public virtual StatusProspeccao Status { get; set; }
        public virtual MotivosNaoConversao MotivoNaoConversao { get; set; }
        public DateTime Vencimento { get; set; } = DateTime.Now.AddDays(14);
        public bool isTratado { get; set; } = false;
    }
}