
//using components;
using Microsoft.Extensions.ObjectPool;
//using Prometheus;
using System.Diagnostics;
using csca5028.lib;
using Microsoft.Extensions.Logging.Console;

namespace point_of_sale_app
{
    public class Program
    {
        public static bool keepRunning = true;
        //create prometheus counter for the number of stores
        //public static Prometheus.Gauge storesGauge = Metrics.CreateGauge("stores", "Number of stores running point-of-sale");
        
        public static string connectionString = "Server=tcp:host.docker.internal,1433;Database=store_db;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        public static string dbName = "store_db";

        public static async Task Main(string[] args)
        {
            //create a prometheus metric server
          //  MetricServer server = new(hostname: "localhost", port: 1234);
            //server.Start();
            //register a function to run when the program is terminated using SIGINT
            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                Program.keepRunning = false;
                Log("point-of-sale terminated.");
              //  server.Stop();
            };

            StoreDBController storeDb = new(connectionString);
            await storeDb.Initialise(dbName);
            List<Store> stores = (List<Store>)await storeDb.GetStoresAsync(dbName);


            while (Program.keepRunning)
            {

                Log("starting point-of-sale...");

                //create a list of store tasks to run
                List<Task> tasks = new List<Task>();
                //create a task to run the web endpoint
                Task webTask = Task.Factory.StartNew(() => RunWebEndPoint(args));
                tasks.Add(webTask);

                foreach (Store store in stores)
                {
                    //Task storeTask = Task.Factory.StartNew(store.StartHealthCheck);
                    //tasks.Add(storeTask);
                    Task storeTask = Task.Factory.StartNew(store.StartSales);
                    tasks.Add(storeTask);
                    //add the store to the prometheus counter
                //    storesGauge.Inc();
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private static void RunWebEndPoint(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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


            app.MapControllers();

            app.Run();
        }

        //create a log function that will output to the console
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}