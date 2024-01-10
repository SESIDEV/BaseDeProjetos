namespace BaseDeProjetos.Models.ViewModels
{
    /// <summary>
    /// Utilizado para retornar apenas dados necessários para o frontend (módulo de Participação)
    /// Não confundir com UsuarioParticipacaoDTO
    /// </summary>
    public class PesquisadorParticipacaoViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}