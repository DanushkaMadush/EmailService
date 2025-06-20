using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using EmailService.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Infrastructure.Services
{
    public class SmtpMailService : IEmailService
    {

        private readonly SmtpSettings _smtpSettings;

        public SmtpMailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public async Task<bool> SendEmailAsync(EmailRequestDto emailRequest, List<AttachmentDto>? attachments = null)
        {
            var message = new MimeMessage();

            // From
            message.From.Add(MailboxAddress.Parse(emailRequest.From));

            // To
            foreach (var recipient in emailRequest.To)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }

            // CC
            if (emailRequest.Cc != null)
            {
                foreach (var cc in emailRequest.Cc)
                {
                    message.Cc.Add(MailboxAddress.Parse(cc));
                }
            }

            // Subject
            message.Subject = emailRequest.Subject;

            // Body
            var builder = new BodyBuilder
            {
                HtmlBody = $"{emailRequest.Body}<br><br>{emailRequest.Signature}"
            };

            // Attachments
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.FileData, ContentType.Parse(attachment.ContentType));
                }
            }

            message.Body = builder.ToMessageBody();

            // Send
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return true;
        }
    }
}
