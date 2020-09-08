using BaseDeProjetos.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace SmartTesting.Controllers
{
    public class Mailer
    {
        private EmailAddress From { get; set; }
        public Mailer()
        {
            From = new EmailAddress("l.nasc@live.com", "Leon Nascimento");
        }

        private async Task<Response> Enviar(EmailAddress destinatario, string titulo, string texto_plain, string texto_html)
        {
            string apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = this.From,
                Subject = titulo,
                PlainTextContent = texto_plain,
                HtmlContent = texto_html is null ? texto_plain : texto_html
            };
            msg.AddTo(destinatario);
            var response = await client.SendEmailAsync(msg);

            return response;
        }

        public bool EnviarNotificacao(List<Usuario> pessoas, Notificacao notificacao)
        {
            bool enviado = false;

            foreach (var pessoa in pessoas)
            {

                var email = new EmailAddress(pessoa.Email, pessoa.UserName);
                var resposta = this.Enviar(email, notificacao.Titulo, notificacao.TextoBase, notificacao.HTML);
                resposta.Wait();
                enviado = enviado == true? resposta.Result.StatusCode == System.Net.HttpStatusCode.OK: false;
            }

            return enviado;
        }
    }

    public class Notificacao
    {
        public string Titulo { get; internal set; }
        public string TextoBase { get; internal set; }
        public string HTML { get; internal set; }
    }
}