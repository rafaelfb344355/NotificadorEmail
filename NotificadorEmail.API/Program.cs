using NotificadorEmail.API.Services;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;
using System.Text;

namespace NotificadorEmail.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicione os serviços ao container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IVerificadorService, VerificadorService>();
            var app = builder.Build();

            // Configure o pipeline de requisição HTTP.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Configure e inicie o serviço do Quartz
            var quartzService = new QuartzService();
            await quartzService.ConfigureQuartz();

            // Inicie o aplicativo
            await app.RunAsync();
        }
    }

    public class QuartzService

    {       // Executar a cada 3 minutos
        public async Task ConfigureQuartz()
        {
            // Inicialize o scheduler do Quartz
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            // Defina o job e o trigger
            IJobDetail job = JobBuilder.Create<MyJobService>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(3).RepeatForever()) 
                .Build();

            // Agende o job com o trigger
            await scheduler.ScheduleJob(job, trigger);

            // Inicie o scheduler
            await scheduler.Start();
        }
    }
    // Executar a cada 3 horas
    /*  public async Task ConfigureQuartz()
      {
          // Inicialize o scheduler do Quartz
          ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
          IScheduler scheduler = await schedulerFactory.GetScheduler();

          // Defina o job e o trigger
          IJobDetail job = JobBuilder.Create<MyJobService>().Build();

          ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x.WithIntervalInHours(3).RepeatForever()) 
              .Build();

          // Agende o job com o trigger
          await scheduler.ScheduleJob(job, trigger);

          // Inicie o scheduler
          await scheduler.Start();
      }*/
    public class MyJobService : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // Enviar POST de um arquivo JSON
            SendJsonData();

            return Task.CompletedTask;
        }

        private void SendJsonData()
        {
            // Criar um HttpClient
            using (var httpClient = new HttpClient())
            {
                // Criar o objeto de parâmetros
                var parameters = new
                {
                    Disp = "111222333",
                };

                // Serializar o objeto de parâmetros para JSON
                var jsonData = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Enviar a requisição POST
                var response = httpClient.PostAsync("https://localhost:7025/api/Verificador", content).Result;

                // Verificar a resposta
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("JSON data sent successfully");
                }
                else
                {
                    Console.WriteLine("Failed to send JSON data. Status code: " + response.StatusCode);
                }
            }
        }
    }

}
