
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using NotificadorEmail.API.DTOs;
using NotificadorEmail.API.Services;
using System.Threading;


namespace NotificadorEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(RequestDTO request)
        {
            var result = emailService.SendEmail(request);

            return Ok(result);

        }

    }
    
}
       


