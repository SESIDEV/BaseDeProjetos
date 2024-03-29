﻿using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using MailSenderHelpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailSenderApp.Services
{
    public class EmailSender : IEmailSender
    {
        public List<Usuario> Destinatarios { get; set; } = new List<Usuario>();

        private readonly MailSenderHelpers.Mailer mailer;

        public EmailSender()
        {
            mailer = new MailSenderHelpers.Mailer();

            Destinatarios.Add(new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br", Nivel = Nivel.Dev });
            Destinatarios.Add(new Usuario { UserName = "Rafael Magno", Email = "istqmat04@firjan.com.br", Nivel = Nivel.Dev });
            Destinatarios.Add(new Usuario { UserName = "Iury Kozlowsky", Email = "iksimoes@firjan.com.br", Nivel = Nivel.Dev });
            /*Destinatarios.Add(new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br", Nivel = Nivel.Supervisor });
            Destinatarios.Add(new Usuario { UserName = "Daniella Serrazine", Email = "dserrazine@firjan.com.br", Nivel = Nivel.Supervisor });
            Destinatarios.Add(new Usuario { UserName = "Ivone Luci Martins", Email = "ilmartins@firjan.com.br", Nivel = Nivel.Supervisor });
            Destinatarios.Add(new Usuario { UserName = "Felipe, Lawrence e Paulo", Email = "pmointegrado@firjan.com.br", Nivel = Nivel.PMO });
            Destinatarios.Add(new Usuario { UserName = "Gabriela e Carlos Eduardo", Email = "nit@firjan.com.br", Nivel = Nivel.PMO });*/
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return mailer.Enviar(new EmailAddress(email), subject, htmlMessage, htmlMessage);
        }

        public bool SendEmailAsync(Notificacao notificacao)
        {
            List<Usuario> lista = Destinatarios;

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