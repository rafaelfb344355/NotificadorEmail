using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificadorEmail.API.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

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
            builder.Services.AddScoped<ConfigEmailService>();

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

            // Adicione a configuração para servir arquivos estáticos
            app.UseStaticFiles("/wwroot");

            var quartzService = new QuartzService();
            await quartzService.ConfigureQuartz();

            // Inicie o aplicativo
            await app.RunAsync();
        }
    }
}
