using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using EmailService.Shared.Common;
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
             try
            {
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
                    return Ok(ApiResponse<string>.SuccessResponse("Email sent successfully"));
                else
                    return StatusCode(500, ApiResponse<string>.Failure("Failed to send email"));
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, ApiResponse<string>.Failure($"Internal server error: {ex.Message}"));
            }
        }

    }
}
