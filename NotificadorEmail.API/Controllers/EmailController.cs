
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;


namespace NotificadorEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public bool SendEmail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("marcelle1@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("marcelle1@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("rafael344355@gmail.com"));
            email.Subject = "test";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate("marcelle1@ethereal.email", "n7xtTZre2ZZKCpjgjF");
            smtp.Send(email);
            smtp.Disconnect(true);

            return true;

        }
    }
}

