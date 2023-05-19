using Microsoft.AspNetCore.SignalR;
using NotificadorEmail.API.DTOs;

namespace NotificadorEmail.API.Services
{
    public class VerificadorService : IVerificadorService
    {
       

        public string VerificarParametro(RequestDTO1 request )
        {
            var resultado = request.Disp;
            var Senha = "111222333";

            if (resultado == Senha)
            {
                // Ciar uma funçao para seconectar e trazer as tabelas do banco de dados
                return "Parâmetro verificado!";
            }
            else
            {
                return "Parâmetro não encontrado!";
            }
        }
    }
}
