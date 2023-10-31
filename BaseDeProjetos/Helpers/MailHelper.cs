using BaseDeProjetos.Models;
using MailSenderApp.Services;
using MailSenderHelpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;

namespace BaseDeProjetos.Helpers
{
    public static class MailHelper
    {
        public static bool NotificarProspecção(FollowUp followup, IEmailSender _mailer)
        {
            try
            {
                Notificacao notificacao = GerarNotificacao(followup);
                ((EmailSender)_mailer).SendEmailAsync(notificacao);

                return true;
            }
            catch (ArgumentException)
            {
                //É um status não notificável
                return false;
            }
        }

        private static Notificacao GerarNotificacao(FollowUp followup)
        {
            Notificacao notificacao;
            switch (followup.Status)
            {
                case StatusProspeccao.ComProposta:

                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi enviada!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1> <br> A prospecção com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} teve uma proposta enviada. " +
                         $"<hr>" +
                         $"Mais detalhes do contato: {followup.Anotacoes} <hr>")
                    };
                    break;

                case StatusProspeccao.Convertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi convertida!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1>, <br>A proposta com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi convertida." +
                         $"<hr>" +
                         $"Mais detalhes da conversão: {followup.Anotacoes}<hr>")
                    };
                    break;

                case StatusProspeccao.NaoConvertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial não foi convertida",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A proposta com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} não foi convertida." +
                         $"<hr>" +
                         $"Mais detalhes: {followup.Anotacoes}<hr>")
                    };
                    break;

                case StatusProspeccao.ContatoInicial:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma nova prospecção foi inicializada!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A prospecção com a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi iniciada." +
                         $"<hr>" +
                         $"Mais detalhes: {followup.Anotacoes}<hr>")
                    };
                    break;

                case StatusProspeccao.Discussao_EsbocoProjeto:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma prospecção está avançando!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A prospecção com a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi chegou à etapa do esboço de projeto. A proposta deverá ser enviada até o dia {DateTime.Today.AddDays(14)}." +
                         $"<hr>" +
                         $"Mais detalhes: {followup.Anotacoes}<hr>")
                    };

                    break;

                default:
                    throw new ArgumentException("Status não notificável");
            }

            notificacao.Status = followup.Status;
            return notificacao;
        }
    }
}