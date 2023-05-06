using NotificadorEmail.API.DTOs;

namespace NotificadorEmail.API.Services
{
    public interface IEmailService
    {

        string SendEmail(RequestDTO request);
    }
}
