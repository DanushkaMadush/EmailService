namespace EmailService.Application.DTOs
{
    public class EmailRequestDto
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }
    }
}
