using NotificadorEmail.API.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace NotificadorEmail.API.Services
{
    public class ConfigEmailService
    {
        

        public ConfigEmailService()
        {
            
        }

        public void Config(int id)
        {
            Console.WriteLine("Configuração iniciada. ID: " + id);

            // Ler o arquivo JSON da tabela Agendamento
            string agendamentoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.json");
            var agendamentos = ReadAgendamentosFromJson(agendamentoFilePath);

            // Encontrar o agendamento correspondente ao ID fornecido
            var agendamento = FindAgendamentoById(agendamentos, id);
            if (agendamento != null)
            {
                // Obter o nome do paciente do agendamento encontrado
                string nomePaciente = agendamento.NomePaciente;

                // Ler o arquivo JSON da tabela Paciente
                string pacienteFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.json");
                var pacientes = ReadPacientesFromJson(pacienteFilePath);

                // Encontrar o paciente correspondente ao nome
                var paciente = FindPacienteByNome(pacientes, nomePaciente);
                if (paciente != null)
                {
                    // Obter a região do paciente encontrado
                    string regiao = paciente.Regiao;

                    // Ler o arquivo JSON da tabela AgenteSaude
                    string agenteSaudeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.json");
                    var agentesSaude = ReadAgentesSaudeFromJson(agenteSaudeFilePath);

                    // Encontrar o agente de saúde correspondente à região
                    var agenteSaude = FindAgenteSaudeByRegiao(agentesSaude, regiao);
                    if (agenteSaude != null)
                    {
                        // Agente de saúde correspondente encontrado
                        Console.WriteLine("Agente de saúde correspondente encontrado: " + agenteSaude.Nome);

                        string to = agenteSaude.Email;
                        string subject = agendamento.TipoAgendamento;
                        string message = "Mensagem de exemplo";

                        // Chamar a função SendEmail do EmailService
                        IEmailService iEmailService = new EmailService();
                        iEmailService.SendEmail(to, subject, message);


                    }
                    else
                    {
                        Console.WriteLine("Agente de saúde não encontrado para a região: " + regiao);
                    }
                }
                else
                {
                    Console.WriteLine("Paciente não encontrado para o nome: " + nomePaciente);
                }
            }
            else
            {
                Console.WriteLine("Agendamento não encontrado para o ID: " + id);
            }
        }

        // Métodos auxiliares para ler e buscar nos arquivos JSON
        private List<Agendamento> ReadAgendamentosFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var database = JsonConvert.DeserializeObject<Database>(json);
            return database?.Agendamentos;
        }

        private Agendamento FindAgendamentoById(List<Agendamento> agendamentos, int id)
        {
            return agendamentos.Find(a => a.ID == id);
        }

        private List<Paciente> ReadPacientesFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var database = JsonConvert.DeserializeObject<Database>(json);
            return database?.Pacientes;
        }

        private Paciente FindPacienteByNome(List<Paciente> pacientes, string nome)
        {
            return pacientes.Find(p => p.Nome == nome);
        }

        private List<AgenteSaude> ReadAgentesSaudeFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var database = JsonConvert.DeserializeObject<Database>(json);
            return database?.AgentesSaude;
        }

        private AgenteSaude FindAgenteSaudeByRegiao(List<AgenteSaude> agentesSaude, string regiao)
        {
            return agentesSaude.Find(a => a.Regiao == regiao);
        }
    }
}
