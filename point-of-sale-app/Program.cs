using Microsoft.Extensions.ObjectPool;
using Prometheus;
using System.Diagnostics;
using csca5028.lib;
using Microsoft.Extensions.Logging.Console;
using System.Collections;

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
            //List<Store> stores = (List<Store>)await storeDb.GetStoresAsync(dbName);
            Hashtable storeAndTerminals = (Hashtable)await storeDb.GetStoresAndTerminalsAsync(dbName);

            //storeAndTerminals[]
            while (Program.keepRunning)
            {

                Log("starting point-of-sale...");

                //create a list of store tasks to run
                List<Task> tasks = new List<Task>();
                //create a task to run the web endpoint
                Task webTask = Task.Factory.StartNew(() => RunWebEndPoint(args));
                tasks.Add(webTask);

                var storesOnline = Metrics.CreateGauge("point_of_sale_app_stores_online", "Number of stores running point-of-sale");
                storesOnline.Set(storeAndTerminals.Count);
                var posOnline = Metrics.CreateGauge("point_of_sale_app_pos_online", "Number of point-of-sale terminals running");
                
                foreach (Store store in storeAndTerminals.Keys)
                {
                    //get terminal from storeAndTerminals
                    List<POSTerminal> terminals = (List<POSTerminal>)storeAndTerminals[store];
                    posOnline.WithLabels(store.Name).Set(terminals.Count);
                    foreach (var terminal in terminals)
                    {
                        Task terminalTask = Task.Factory.StartNew(() => store.StartSales(terminal));
                        tasks.Add(terminalTask);
                    }
                }
                //wait for all tasks to complete - some never will
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
            app.UseMetricServer(url: "/metrics");
            

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