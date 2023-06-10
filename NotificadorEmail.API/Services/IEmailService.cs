

namespace NotificadorEmail.API.Services
{
    public interface IEmailService
    {
        
        string SendEmail(string To, string Subject, string Message);
    } 
}
