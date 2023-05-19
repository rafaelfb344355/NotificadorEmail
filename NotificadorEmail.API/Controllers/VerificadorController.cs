using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificadorEmail.API.DTOs;
using NotificadorEmail.API.Services;

namespace NotificadorEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificadorController : ControllerBase
    {
        private readonly IVerificadorService verificadorService;

        public VerificadorController(IVerificadorService verificadorService)
        {
            this.verificadorService = verificadorService;
        }

        [HttpPost]
        public IActionResult VerificarParametro(RequestDTO1 request)
        {
            var result = verificadorService.VerificarParametro(request);

            return Ok(result);
        }
    }
}

