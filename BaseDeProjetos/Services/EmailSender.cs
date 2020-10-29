using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.CodeStyle;
using SendGrid.Helpers.Mail;
using SmartTesting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailSenderApp.Services
{
    public class EmailSender : IEmailSender
    {
        public List<Usuario> Destinatarios { get; set; } = new List<Usuario>();

        Mailer mailer;

        public EmailSender()
        {

            /* TODO: Configurar uma lista de destinatarios para passar como dependência */
            Usuario leon = new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br" };
            //Usuario chefe = new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br" };
            Destinatarios.Add(leon);
            //Destinatarios.Add(chefe);
            this.mailer = new Mailer(Destinatarios);

        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return mailer.Enviar(new EmailAddress(email), subject, htmlMessage, htmlMessage);
        }

        public bool SendEmailAsync(Notificacao notificacao)
        {
            return mailer.EnviarNotificacao(notificacao);
        }

    }
}