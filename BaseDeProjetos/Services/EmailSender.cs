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

            this.mailer = new Mailer();

            Destinatarios.Add(new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br" , Nivel = Nivel.Dev});
            Destinatarios.Add(new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br"  , Nivel = Nivel.Supervisor});
            Destinatarios.Add(new Usuario { UserName = "Daniella Serrazine", Email = "dserrazine@firjan.com.br" , Nivel = Nivel.Supervisor});
            Destinatarios.Add(new Usuario { UserName = "Sergio Kuriyama", Email = "skuriyama@firjan.com.br",Nivel = Nivel.Supervisor  });
            Destinatarios.Add(new Usuario { UserName = "PMO Integrado", Email = "pmointegrado@firjan.com.br" , Nivel = Nivel.PMO});
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return mailer.Enviar(new EmailAddress(email), subject, htmlMessage, htmlMessage);
        }

        public bool SendEmailAsync(Notificacao notificacao)
        {
            var lista = this.Destinatarios;
            
            switch (notificacao.Status)
            {
                case StatusProspeccao.Discussao_EsbocoProjeto:
                    lista = lista.Where(u => u.Nivel == Nivel.PMO).ToList();
                    break;
                case StatusProspeccao.ComProposta:
                    break;
                case StatusProspeccao.Convertida:
                    break;
                default:
                    lista = lista.Where(u => u.Nivel != Nivel.PMO).ToList();
                    break;
            }
            

            return mailer.EnviarNotificacao(notificacao, lista);
        }

    }
}