
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Prometheus;
using components;

namespace point_of_sale
{
    internal class Program
    {
        public static bool keepRunning = true;
        //create prometheus counter for the number of stores
        public static Prometheus.Gauge storesGauge = Metrics.CreateGauge("stores", "Number of stores running point-of-sale");
        //create a prometheus uptime gauge
        public static Prometheus.Gauge posuptime = Metrics.CreateGauge("uptime", "Number of seconds the point-of-sale has been running");

        public static string connectionString = "Data Source=localhost;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public static string dbName = "store_db";

        static async Task Main(string[] args)
        {
            posuptime.Set(DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime).TotalSeconds);
            
            //create a prometheus metric server
            MetricServer server = new(hostname: "localhost", port: 1234);
            server.Start();
            
            //register a function to run when the program is terminated using SIGINT
            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                Program.keepRunning = false;
                Log("point-of-sale terminated.");
                server.Stop();
            };

            StoreDBController storeDb = new(connectionString);
            await storeDb.Initialise(dbName);
            List<Store> stores = (List<Store>)await storeDb.GetStoresAsync(dbName);


            while (Program.keepRunning)
            {

                Log("starting point-of-sale...");

                //create a list of store tasks to run
                List<Task> tasks = new List<Task>();
                Task task = Task.Factory.StartNew(PosUptimeCalc);
                tasks.Add(task);
                
                foreach(Store store in stores)
                {
                    //Task storeTask = Task.Factory.StartNew(store.StartHealthCheck);
                    //tasks.Add(storeTask);
                    Task storeTask = Task.Factory.StartNew(store.StartSales);
                    tasks.Add(storeTask);
                    //add the store to the prometheus counter
                    storesGauge.Inc();
                }
                
                Task.WaitAll(tasks.ToArray());
                
            }
        }

        //create a log function that will output to the console
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void PosUptimeCalc()
        {
            while (true) //never terminate
            {
                posuptime.Set(DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime).TotalSeconds);
                //sleep for 5 seconds
                Thread.Sleep(5000);                
            }
        }
    }


}