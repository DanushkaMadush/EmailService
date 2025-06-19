using EmailService.Application.DTOs;
using System.Threading.Tasks;

namespace EmailService.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequestDto emailRequest, List<AttachmentDto>? attachments = null);
    }
}
