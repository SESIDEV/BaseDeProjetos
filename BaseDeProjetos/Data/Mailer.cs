using BaseDeProjetos.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
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
            SendGridClient client = new SendGridClient(apiKey);
            SendGridMessage msg = new SendGridMessage()
            {
                From = From,
                Subject = titulo,
                PlainTextContent = texto_plain,
                HtmlContent = texto_html is null ? texto_plain : texto_html
            };
            msg.AddTo(destinatario);
            Response response = await client.SendEmailAsync(msg);

            return response;
        }

        public bool EnviarNotificacao(List<Usuario> pessoas, Notificacao notificacao)
        {
            bool enviado = false;

            foreach (Usuario pessoa in pessoas)
            {

                EmailAddress email = new EmailAddress(pessoa.Email, pessoa.UserName);
                Task<Response> resposta = Enviar(email, notificacao.Titulo, notificacao.TextoBase, notificacao.HTML);
                resposta.Wait();
                enviado = enviado == true ? resposta.Result.StatusCode == System.Net.HttpStatusCode.OK : false;
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