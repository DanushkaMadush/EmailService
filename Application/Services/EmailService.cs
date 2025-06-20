using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task<bool> SendEmailAsync(EmailRequestDto emailRequest, List<AttachmentDto>? attachments = null)
        {
            // Combine body and signature
            string fullBody = new StringBuilder()
                .AppendLine(emailRequest.Body)
                .AppendLine("<br><br>")
                .AppendLine(emailRequest.Signature)
                .ToString();

            // Save metadata to DB via stored proc
            int emailId = await _emailRepository.LogEmailAsync(
                emailRequest.From, emailRequest.To, emailRequest.Subject, fullBody
            );

            // Save attachments to DB if any
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    await _emailRepository.SaveAttachmentAsync(emailId, attachment);
                }
            }

            // Note: Actual SMTP sending will be handled in Infrastructure layer
            return true; // Stub return for now
        }
    }
}
