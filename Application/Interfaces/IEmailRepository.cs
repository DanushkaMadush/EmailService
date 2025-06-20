using EmailService.Application.DTOs;

namespace EmailService.Application.Interfaces
{
    public interface IEmailRepository
    {
        Task<int> LogEmailAsync(string from, List<string> to, string subject, string body);
        Task<bool> SaveAttachmentAsync(int emailId, AttachmentDto attachment);
    }
}
