using BaseDeProjetos.Models;
using MailSenderApp.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTesting.Controllers
{
    public class Mailer:EmailSender
    {
        private EmailAddress From { get; set; }
        private List<Usuario> destinatarios;
        public Mailer(List<Usuario> destinatarios)
        {
            From = new EmailAddress("l.nasc@live.com", "Leon Nascimento");
            this.destinatarios = destinatarios;
        }

        public async Task<Response> Enviar(EmailAddress destinatario, string titulo, string texto_plain, string texto_html)
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

        public bool EnviarNotificacao(Notificacao notificacao)
        {
            bool enviado = false;

            foreach (Usuario pessoa in this.destinatarios)
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

    public class EmailTemplate
    {
        public static string header = @"
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>
<head>
  <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
  <!--[if !mso]><!-->
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <!--<![endif]-->
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <meta name='format-detection' content='telephone=no'>
  <meta name='x-apple-disable-message-reformatting'>
  <title></title>
  <style type='text/css'>
    @media screen {
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 400;
        src: local('Fira Sans Regular'), local('FiraSans-Regular'), url(https://fonts.gstatic.com/s/firasans/v8/va9E4kDNxMZdWfMOD5Vvl4jLazX3dA.woff2) format('woff2');
        unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 400;
        src: local('Fira Sans Regular'), local('FiraSans-Regular'), url(https://fonts.gstatic.com/s/firasans/v8/va9E4kDNxMZdWfMOD5Vvk4jLazX3dGTP.woff2) format('woff2');
        unicode-range: U+0400-045F, U+0490-0491, U+04B0-04B1, U+2116;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 500;
        src: local('Fira Sans Medium'), local('FiraSans-Medium'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnZKveRhf6Xl7Glw.woff2) format('woff2');
        unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 500;
        src: local('Fira Sans Medium'), local('FiraSans-Medium'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnZKveQhf6Xl7Gl3LX.woff2) format('woff2');
        unicode-range: U+0400-045F, U+0490-0491, U+04B0-04B1, U+2116;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 700;
        src: local('Fira Sans Bold'), local('FiraSans-Bold'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnLK3eRhf6Xl7Glw.woff2) format('woff2');
        unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 700;
        src: local('Fira Sans Bold'), local('FiraSans-Bold'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnLK3eQhf6Xl7Gl3LX.woff2) format('woff2');
        unicode-range: U+0400-045F, U+0490-0491, U+04B0-04B1, U+2116;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 800;
        src: local('Fira Sans ExtraBold'), local('FiraSans-ExtraBold'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnMK7eRhf6Xl7Glw.woff2) format('woff2');
        unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;
      }
      @font-face {
        font-family: 'Fira Sans';
        font-style: normal;
        font-weight: 800;
        src: local('Fira Sans ExtraBold'), local('FiraSans-ExtraBold'), url(https://fonts.gstatic.com/s/firasans/v8/va9B4kDNxMZdWfMOD5VnMK7eQhf6Xl7Gl3LX.woff2) format('woff2');
        unicode-range: U+0400-045F, U+0490-0491, U+04B0-04B1, U+2116;
      }
    }
  </style>
  <style type='text/css'>
    #outlook a {
      padding: 0;
    }

    .ReadMsgBody,
    .ExternalClass {
      width: 100%;
    }

    .ExternalClass,
    .ExternalClass p,
    .ExternalClass td,
    .ExternalClass div,
    .ExternalClass span,
    .ExternalClass font {
      line-height: 100%;
    }

    div[style*='margin: 14px 0'],
    div[style*='margin: 16px 0'] {
      margin: 0 !important;
    }

    table,
    td {
      mso-table-lspace: 0;
      mso-table-rspace: 0;
    }

    table,
    tr,
    td {
      border-collapse: collapse;
    }

    body,
    td,
    th,
    p,
    div,
    li,
    a,
    span {
      -webkit-text-size-adjust: 100%;
      -ms-text-size-adjust: 100%;
      mso-line-height-rule: exactly;
    }

    img {
      border: 0;
      outline: none;
      line-height: 100%;
      text-decoration: none;
      -ms-interpolation-mode: bicubic;
    }

    a[x-apple-data-detectors] {
      color: inherit !important;
      text-decoration: none !important;
    }

    body {
      margin: 0;
      padding: 0;
      width: 100% !important;
      -webkit-font-smoothing: antialiased;
    }

    .pc-gmail-fix {
      display: none;
      display: none !important;
    }

    @media screen and (min-width: 621px) {
      .pc-email-container {
        width: 620px !important;
      }
    }
  </style>
  <style type='text/css'>
    @media screen and (max-width:620px) {
      .pc-sm-p-24-20-30 {
        padding: 24px 20px 30px !important
      }
      .pc-sm-p-35-10-15 {
        padding: 35px 10px 15px !important
      }
      .pc-sm-p-21-10-14 {
        padding: 21px 10px 14px !important
      }
      .pc-sm-h-20 {
        height: 20px !important
      }
      .pc-sm-mw-100pc {
        max-width: 100% !important
      }
    }
  </style>
  <style type='text/css'>
    @media screen and (max-width:525px) {
      .pc-xs-p-15-10-20 {
        padding: 15px 10px 20px !important
      }
      .pc-xs-h-100 {
        height: 100px !important
      }
      .pc-xs-br-disabled br {
        display: none !important
      }
      .pc-xs-fs-30 {
        font-size: 30px !important
      }
      .pc-xs-lh-42 {
        line-height: 42px !important
      }
      .pc-xs-p-25-0-5 {
        padding: 25px 0 5px !important
      }
      .pc-xs-p-5-0 {
        padding: 5px 0 !important
      }
    }
  </style>
  <!--[if mso]>
    <style type='text/css'>
        .pc-fb-font {
            font-family: Helvetica, Arial, sans-serif !important;
        }
    </style>
    <![endif]-->
  <!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG/><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]-->
</head>
<body style='width: 100% !important; margin: 0; padding: 0; mso-line-height-rule: exactly; -webkit-font-smoothing: antialiased; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; background-color: #f4f4f4' class=''>
  <div style='display: none !important; visibility: hidden; opacity: 0; overflow: hidden; mso-hide: all; height: 0; width: 0; max-height: 0; max-width: 0;'>
    ‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;
  </div>
  <table class='pc-email-body' width='100%' bgcolor='#f4f4f4' border='0' cellpadding='0' cellspacing='0' role='presentation' style='table-layout: fixed;'>
    <tbody>
      <tr>
        <td class='pc-email-body-inner' align='center' valign='top'>
          <!--[if gte mso 9]>
            <v:background xmlns:v='urn:schemas-microsoft-com:vml' fill='t'>
                <v:fill type='tile' src='' color='#f4f4f4'/>
            </v:background>
            <![endif]-->
          <!--[if (gte mso 9)|(IE)]><table width='620' align='center' border='0' cellspacing='0' cellpadding='0' role='presentation'><tr><td width='620' align='center' valign='top'><![endif]-->
          <table class='pc-email-container' width='100%' align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='margin: 0 auto; max-width: 620px;'>
            <tbody>
              <tr>
                <td align='left' valign='top' style='padding: 0 10px;'>
                  <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation'>
                    <tbody>
                      <tr>
                        <td height='20' style='font-size: 1px; line-height: 1px;'>&nbsp;</td>
                      </tr>
                    </tbody>
                  </table>
                  <table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'>
                    <tbody>
                      <tr>
                        <td valign='top'>
                          <!-- BEGIN MODULE: Header 1 -->
                          <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation'>
                            <tbody>
                              <tr>
                                <td bgcolor='#1b1b1b' valign='top' style='background-color: #1b1b1b; background-position: top center; background-size: cover'>
                                  <!--[if gte mso 9]>
            <v:rect xmlns:v='urn:schemas-microsoft-com:vml' fill='true' stroke='false' style='width: 600px;'>
                <v:fill type='frame'  color='#1b1b1b'></v:fill>
                <v:textbox style='mso-fit-shape-to-text: true;' inset='0,0,0,0'>
                    <div style='font-size: 0; line-height: 0;'>
                        <table width='600' border='0' cellpadding='0' cellspacing='0' role='presentation' align='center'>
                            <tr>
                                <td style='font-size: 14px; line-height: 1.5;' valign='top'>
                                    <table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'>
                                        <tr>
                                            <td colspan='3' height='24' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width='30' valign='top' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                            <td valign='top' align='left'>
            <![endif]-->
                                  <!--[if !gte mso 9]><!-->
                                  <table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'>
                                    <tbody>
                                      <tr>
                                        <td class='pc-sm-p-24-20-30 pc-xs-p-15-10-20' valign='top' style='padding: 24px 30px 40px;'>
                                          <!--<![endif]-->
                                          <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation'>
                                            <tbody>
                                              <tr>
                                                <td class='pc-xs-h-100' height='135' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                              </tr>
                                              <tr>
                                                <td class='pc-xs-fs-30 pc-xs-lh-42 pc-fb-font' valign='top' style='padding: 13px 10px 0; letter-spacing: -0.7px; line-height: 46px; font-family: Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; color: #ffffff;'><h1 style='color:#ffffff'>A base foi atualizada!</h1></td>
                                              </tr>
                                            </tbody>
                                          </table>
                                          <!--[if !gte mso 9]><!-->
                                        </td>
                                      </tr>
                                    </tbody>
                                  </table>
                                  <!--<![endif]-->
                                  <!--[if gte mso 9]>
                                            </td>
                                            <td width='30' style='line-height: 1px; font-size: 1px;' valign='top'>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan='3' height='40' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </v:textbox>
            </v:rect>
            <![endif]-->
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!-- END MODULE: Header 1 -->
                          <!-- BEGIN MODULE: Content 1 -->
                          <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                            <tbody>
                              <tr>
                                <td class='pc-sm-p-35-10-15 pc-xs-p-25-0-5' style='padding: 40px 20px 20px; background-color: #ffffff' valign='top' bgcolor='#ffffff'>
                                  <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                    <tbody>
                                      <tr>
                                        <td class='pc-fb-font' style='padding: 0 20px; text-align: center; font-family: 'Fira Sans', Helvetica, Arial, sans-serif; font-size: 24px; font-weight: 700; line-height: 1.42; letter-spacing: -0.4px; color: #151515;' valign='top'>
            ";


        public static string footer = @"
                                        </td>
                                      </tr>
                                      <tr>
                                        <td height='10' style='font-size: 1px; line-height: 1px;'>&nbsp;</td>
                                      </tr>
                                    </tbody>
                                    <tbody>
                                    </tbody>
                                  </table>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!-- END MODULE: Content 1 -->
                          <!-- BEGIN MODULE: Footer 1 -->
                          <table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'>
                            <tbody>
                              <tr>
                                <td height='8' style='font-size: 1px; line-height: 1px;'>&nbsp;</td>
                              </tr>
                            </tbody>
                          </table>
                          <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                            <tbody>
                              <tr>
                                <td class='pc-sm-p-21-10-14 pc-xs-p-5-0' style='padding: 21px 20px 14px 20px; background-color: #1B1B1B;' valign='top' bgcolor='#1B1B1B' role='presentation'>
                                  <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                    <tbody>
                                      <tr>
                                        <td style='font-size: 0;' valign='top'>
                                          <!--[if (gte mso 9)|(IE)]><table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'><tr><td width='280' valign='top'><![endif]-->
                                          <div class='pc-sm-mw-100pc' style='display: inline-block; width: 100%; max-width: 280px; vertical-align: top;'>
                                            <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                              <tbody>
                                                <tr>
                                                  <td style='padding: 20px;' valign='top'>
                                                    <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                                      <tbody>
                                                        <tr>
                                                          <td class='pc-fb-font' style='font-family: Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 500; line-height: 24px; letter-spacing: -0.2px; color: #ffffff;' valign='top'>Base de Projetos</td>
                                                        </tr>
                                                        <tr>
                                                          <td height='11' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                                        </tr>
                                                      </tbody>
                                                      <tbody>
                                                        <tr>
                                                          <td class='pc-fb-font' style='font-family: Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; letter-spacing: -0.2px; color: #D8D8D8;' valign='top'>Atualização da base de projetos enviada automaticamente após a realização de uma ação no sistema. Você recebeu este e-mail por estar na lista de destinatários.</td>
                                                        </tr>
                                                        <tr>
                                                          <td class='pc-sm-h-20' height='56' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                                        </tr>
                                                      </tbody>
                                                      <tbody>
                                                      </tbody>
                                                    </table>
                                                  </td>
                                                </tr>
                                              </tbody>
                                            </table>
                                          </div>
                                          <!--[if (gte mso 9)|(IE)]></td><td width='280' valign='top'><![endif]-->
                                          <div class='pc-sm-mw-100pc' style='display: inline-block; width: 100%; max-width: 280px; vertical-align: top;'>
                                            <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                              <tbody>
                                                <tr>
                                                  <td style='padding: 20px;' valign='top'>
                                                    <table border='0' cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                                                      <tbody>
                                                        <tr>
                                                          <td class='pc-fb-font' style='font-family: Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 500; line-height: 24px; letter-spacing: -0.2px; color: #ffffff;' valign='top'>SGI-GGI</td>
                                                        </tr>
                                                        <tr>
                                                          <td height='11' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                                        </tr>
                                                      </tbody>
                                                      <tbody>
                                                        <tr>
                                                          <td class='pc-fb-font' style='font-family: Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; letter-spacing: -0.2px; color: #D8D8D8;' valign='top'>Rua Morais e Silva, 53</td>
                                                        </tr>
                                                        <tr>
                                                          <td class='pc-sm-h-20' height='45' style='line-height: 1px; font-size: 1px;'>&nbsp;</td>
                                                        </tr>
                                                      </tbody>
                                                      <tbody>
                                                        <tr>
                                                          <td class='pc-fb-font' style='font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 500; line-height: 24px;' valign='top'>
                                                            <a href='https://sgi-ggi-isi.azurewebsites.net' style='text-decoration: none; color: #1595E7;'>sgi-ggi-isi.azurewebsites.net</a>
                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>
                                                  </td>
                                                </tr>
                                              </tbody>
                                            </table>
                                          </div>
                                          <!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]-->
                                        </td>
                                      </tr>
                                    </tbody>
                                  </table>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!-- END MODULE: Footer 1 -->
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation'>
                    <tbody>
                      <tr>
                        <td height='20' style='font-size: 1px; line-height: 1px;'>&nbsp;</td>
                      </tr>
                    </tbody>
                  </table>
                </td>
              </tr>
            </tbody>
          </table>
          <!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]-->
        </td>
      </tr>
    </tbody>
  </table>
  <!-- Fix for Gmail on iOS -->
  <div class='pc-gmail-fix' style='white-space: nowrap; font: 15px courier; line-height: 0;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </div>
</body>
</html>
            ";
        public static string MontarTemplate(string texto)
        {
            return header + texto + footer;
        }
    }
}