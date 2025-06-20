namespace EmailService.Domain.Entities
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
    }
}
