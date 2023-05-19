using NotificadorEmail.API.DTOs;

namespace NotificadorEmail.API.Services
{
    public interface IVerificadorService
    {
        string VerificarParametro(RequestDTO1 request);
    }
}