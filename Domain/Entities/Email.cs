using EmailService.Domain.Enums;

namespace EmailService.Domain.Entities
{
    public class Email
    {
        public int Id { get; set; } 
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
        public DateTime SentAt { get; set; }
        public EmailStatus Status { get; set; }
    }
}
