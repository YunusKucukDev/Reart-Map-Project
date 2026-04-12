using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

namespace MapProject.WebUI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendContactEmailAsync(string fromName, string fromEmail, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mail.To.Add(new MailboxAddress(_settings.SenderName, _settings.ReceiverEmail));
            mail.ReplyTo.Add(new MailboxAddress(fromName, fromEmail));
            mail.Subject = $"[İletişim Formu] {subject}";

            var body = new BodyBuilder
            {
                HtmlBody = BuildHtmlBody(fromName, fromEmail, subject, message)
            };
            mail.Body = body.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port,
                _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
        }

        private static string BuildHtmlBody(string name, string email, string subject, string message)
        {
            return $"""
            <!DOCTYPE html>
            <html lang="tr">
            <head><meta charset="utf-8"></head>
            <body style="margin:0;padding:0;background:#f0f2f5;font-family:Arial,sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#f0f2f5;padding:40px 0;">
                <tr><td align="center">
                  <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:10px;overflow:hidden;box-shadow:0 4px 16px rgba(0,0,0,.08);">
                    <tr>
                      <td style="background:linear-gradient(135deg,#3b82f6,#1d4ed8);padding:32px 40px;">
                        <h1 style="margin:0;color:#fff;font-size:22px;font-weight:700;">Yeni İletişim Mesajı</h1>
                        <p style="margin:6px 0 0;color:rgba(255,255,255,.75);font-size:14px;">Web sitenizin iletişim formundan gönderildi</p>
                      </td>
                    </tr>
                    <tr>
                      <td style="padding:32px 40px;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #f1f5f9;">
                              <span style="font-size:12px;color:#94a3b8;text-transform:uppercase;letter-spacing:.05em;font-weight:600;">Gönderen</span>
                              <p style="margin:4px 0 0;font-size:15px;color:#1e293b;font-weight:600;">{name}</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #f1f5f9;">
                              <span style="font-size:12px;color:#94a3b8;text-transform:uppercase;letter-spacing:.05em;font-weight:600;">E-posta</span>
                              <p style="margin:4px 0 0;font-size:15px;color:#3b82f6;">{email}</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #f1f5f9;">
                              <span style="font-size:12px;color:#94a3b8;text-transform:uppercase;letter-spacing:.05em;font-weight:600;">Konu</span>
                              <p style="margin:4px 0 0;font-size:15px;color:#1e293b;">{subject}</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:16px 0 0;">
                              <span style="font-size:12px;color:#94a3b8;text-transform:uppercase;letter-spacing:.05em;font-weight:600;">Mesaj</span>
                              <div style="margin-top:10px;padding:16px;background:#f8fafc;border-radius:8px;border-left:4px solid #3b82f6;font-size:15px;color:#334155;line-height:1.7;">
                                {message.Replace("\n", "<br>")}
                              </div>
                            </td>
                          </tr>
                        </table>
                        <p style="margin:24px 0 0;font-size:12px;color:#94a3b8;">Bu e-posta {DateTime.Now:dd.MM.yyyy HH:mm} tarihinde otomatik olarak gönderilmiştir. Yanıtlamak için direkt bu maile cevap verin.</p>
                      </td>
                    </tr>
                  </table>
                </td></tr>
              </table>
            </body>
            </html>
            """;
        }
    }
}
