﻿using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Usuario : IdentityUser
    {
        public Instituto Casa { get; set; }

        public Nivel Nivel { get; set; }

        [Display(Name = "Matrícula")]
        public int Matricula { get; set; }

        [Display(Name = "Titulação Máxima")]
        public Titulacao Titulacao { get; set; }

        public TipoVinculo Vinculo { get; set; }

        [Display(Name = "Foto do Perfil")]
        public string Foto { get; set; }

        [Display(Name = "Competências")]
        public string Competencia { get; set; }

        [ForeignKey("CargoId")]
        public virtual Cargo? Cargo { get; set; }

        public virtual int? CargoId { get; set; }

        public UsuarioParticipacaoDTO ToUsuarioParticipacao()
        {
            return new UsuarioParticipacaoDTO
            {
                Id = Id,
                Cargo = new CargoDTO { Id = Cargo.Id, Nome = Cargo.Nome },
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                Nivel = Nivel,
                Casa = Casa,
                UserName = UserName,
            };
        }

        internal PesquisadorParticipacaoViewModel ToPesquisadorViewModel()
        {
            return new PesquisadorParticipacaoViewModel
            {
                Id = Id,
                Email = Email,
                UserName = UserName,
            };
        }
    }
}