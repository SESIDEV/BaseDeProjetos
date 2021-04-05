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
            if (System.Environment.GetEnvironmentVariable("Ambiente") == "Web")
            {
                Destinatarios.Add(new Usuario { UserName = "Sergio Kuriyama", Email = "skuriyama@firjan.com.br" });
                Destinatarios.Add(new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br" });
                Destinatarios.Add(new Usuario { UserName = "Daniella Serrazine", Email = "dserrazine@firjan.com.br" });
                Destinatarios.Add(new Usuario { UserName = "PMO Integrado", Email = "pmointegrado@firjan.com.br" });
            }
            Usuario leon = new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br" };
            Destinatarios.Add(leon);
            this.mailer = new Mailer();

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