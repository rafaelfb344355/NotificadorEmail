using Newtonsoft.Json;
using NotificadorEmail.API.Classes;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NotificadorEmail.API.Services
{
    public class QuartzService
    {
        private readonly string _filePath;

        public QuartzService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.json");

            // Chame a função Config do ConfigEmailService
        }

        public async Task ConfigureQuartz()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            Console.WriteLine("Criando o job MyJobService...");

            IJobDetail job = JobBuilder.Create<MyJobService>()
                .UsingJobData("filePath", _filePath)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();

            Console.WriteLine("Job MyJobService criado e agendado com sucesso.");
        }

        public class MyJobService : IJob
        {
            private string _filePath;

            public Task Execute(IJobExecutionContext context)
            {
                _filePath = context.JobDetail.JobDataMap.GetString("filePath");

                Console.WriteLine("Job executado. Chamando o método VerificarAgendamentos...");

                VerificarAgendamentos();

                return Task.CompletedTask;
            }

            public void VerificarAgendamentos()
            {
                var json = File.ReadAllText(_filePath);
                var database = JsonConvert.DeserializeObject<Database>(json);

                if (database != null && database.Agendamentos != null)
                {
                    Console.WriteLine("Dados do arquivo JSON foram recebidos com sucesso!");

                    foreach (var agendamento in database.Agendamentos)
                    {
                        var dataAtual = DateTime.Now.Date;
                        var horaAtual = DateTime.Now.TimeOfDay;

                        if (agendamento.Data.Date == dataAtual)
                        {
                            if (horaAtual - agendamento.Hora.TimeOfDay >= TimeSpan.FromHours(3))
                            {
                                agendamento.SetConfirmacaoAtraso(true);

                                Console.WriteLine("Paciente: " + agendamento.NomePaciente + " confirmou presença.");

                                // Crie uma instância do ConfigEmailService e chame a função Config, passando o Id do agendamento
                                ConfigEmailService configEmailService = new ConfigEmailService();
                                configEmailService.Config(agendamento.ID);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Falha ao receber os dados do arquivo JSON. Verifique se o caminho do arquivo está correto.");
                }
            }
        }
    }

    public class Database
    {
        public List<Paciente> Pacientes { get; set; }
        public List<Agendamento> Agendamentos { get; set; }
        public List<AgenteSaude> AgentesSaude { get; set; }
    }
}
