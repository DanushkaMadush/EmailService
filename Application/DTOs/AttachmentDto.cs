namespace EmailService.Application.DTOs
{
    public class AttachmentDto
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
    }
}
