using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotificadorEmail.API.Classes;

using NotificadorEmail.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotificadorEmail.API.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.json");

        [HttpGet("atrasados")]
        public ActionResult<IEnumerable<Paciente>> GetPacientesAtrasados()
        {
            var json = System.IO.File.ReadAllText(_filePath);
            var database = JsonConvert.DeserializeObject<Database>(json);

            if (database != null && database.Pacientes != null && database.Agendamentos != null && database.AgentesSaude != null)
            {
                Console.WriteLine("Dados do arquivo JSON foram recebidos com sucesso!");

                var pacientesAtrasados = new List<Paciente>();

                foreach (var agendamento in database.Agendamentos)
                {
                    var dataAtual = DateTime.Now.Date;
                    var horaAtual = DateTime.Now.TimeOfDay;

                    if (agendamento.Data.Date == dataAtual && horaAtual - agendamento.Hora.TimeOfDay >= TimeSpan.FromHours(3))
                    {
                        agendamento.SetConfirmacaoAtraso(true);

                        // Encontre o paciente correspondente ao agendamento
                        var paciente = database.Pacientes.FirstOrDefault(p => p.Nome == agendamento.NomePaciente);
                        if (paciente != null)
                        {
                            pacientesAtrasados.Add(paciente);
                        }
                    }
                }

                return Ok(pacientesAtrasados);
            }

            return NotFound();
        }
    }
}
