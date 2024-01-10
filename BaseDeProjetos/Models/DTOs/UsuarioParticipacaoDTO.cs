using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.ViewModels;
using System;

namespace BaseDeProjetos.Models.DTOs
{
    public class UsuarioParticipacaoDTO
    {
        public string Id { get; set; }
        public Instituto Casa { get; set; }
        public Cargo Cargo { get; set; }
        public Nivel Nivel { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public Usuario ToUsuario()
        {
            return new Usuario
            {
                Id = Id,
                Casa = Casa,
                Cargo = Cargo,
                Nivel = Nivel,
                EmailConfirmed = EmailConfirmed,
                Email = Email,
                UserName = UserName
            };
        }

        internal PesquisadorParticipacaoViewModel ToPesquisadorViewModel()
        {
            return new PesquisadorParticipacaoViewModel { Id = Id, Email = Email, UserName = UserName };
        }
    }
}