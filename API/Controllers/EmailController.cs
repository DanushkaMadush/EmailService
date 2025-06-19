using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequestDto emailRequest, List<IFormFile>? files)
        {
            // Convert uploaded files to attachment DTOs
            var attachments = new List<AttachmentDto>();

            if (files != null)
            {
                foreach (var file in files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    attachments.Add(new AttachmentDto
                    {
                        FileName = file.FileName,
                        FileData = ms.ToArray(),
                        ContentType = file.ContentType
                    });
                }
            }

            var result = await _emailService.SendEmailAsync(emailRequest, attachments);

            if (result)
                return Ok(new { message = "Email sent successfully" });
            else
                return StatusCode(500, new { message = "Failed to send email" });
        }
    }
}
