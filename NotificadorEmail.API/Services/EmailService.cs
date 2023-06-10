
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using NotificadorEmail.API.Classes;

namespace NotificadorEmail.API.Services
{
        public class EmailService : IEmailService
    {
        

        public string SendEmail(string To, string Subject, string Message)
        {
            Console.WriteLine("email sendo enviado");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("zelda.hagenes52@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(To));

            email.Subject = Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = Message };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls, default); 

                smtp.Authenticate("zelda.hagenes52@ethereal.email", "B9ubActTwUzxmDBHNx");
                smtp.Send(email);
                smtp.Disconnect(true);
                return "Mail Sent!";
            }

      
    }
    }
    