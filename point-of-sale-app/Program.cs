using Microsoft.Extensions.ObjectPool;
using Prometheus;
using System.Diagnostics;
using csca5028.lib;
using Microsoft.Extensions.Logging.Console;
using System.Collections;
using RabbitMQ.Client;

namespace point_of_sale_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLogging(configure => configure.AddConsole());
            
            builder.Services.AddHostedService<StoreService>();
            
            builder.Services.AddSingleton<IPOSTerminalTaskQueue, POSTerminalTaskQueue>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseMetricServer(url: "/metrics");


            app.MapControllers();

            app.Run();
            
        }
    }
}