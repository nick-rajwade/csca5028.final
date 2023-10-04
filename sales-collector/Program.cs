using Prometheus;
using csca5028.lib;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Diagnostics;

namespace sales_collector
{
    public class Program
    {
        public static bool keepRunning = true;
        //create prometheus counter for the number of stores
        //public static Prometheus.Gauge storesGauge = Metrics.CreateGauge("stores", "Number of stores running point-of-sale");

        public static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        public static string dbName = "sales_db";
        public static string hostname = "rmq0";
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

            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = hostname };
            csca5028.lib.SalesDBController salesDBController = new(connectionString);
            await salesDBController.Initialise(dbName);
            while(true)
            {
                try
                {
                    using (var connection = factory.CreateConnection())
                    {
                        //create a channel to communicate with the rabbitmq server
                        using (var channel = connection.CreateModel())
                        {
                            //declare a queue to send messages to. queue name should be based on the store ID
                            var queueName = "salesq";
                            Log("Connect to sales-exchange...");
                            
                            channel.ExchangeDeclare("sales-exchange", "x-consistent-hash", true, false);
                            //bind weight at 10% to the queue

                            Log($"bind to {queueName} queue");

                            channel.QueueBind(queue: queueName, exchange: "sales-exchange", "42", new Dictionary<string, object>() { { "x-weight", 10 } });
                            //create a consumer to consume messages from the queue
                            var consumer = new EventingBasicConsumer(channel);
                            //register a callback to be executed when a message is received

                            Log("Register Callback...");

                            var summary = Metrics.CreateSummary("sales_collector_db_insert", "Summary for sales_collector Database insert", new SummaryConfiguration
                            {
                                LabelNames = new[] { "method", "endpoint" },
                                SuppressInitialValue = true,
                                Objectives = new[]
                                {
                                    new QuantileEpsilonPair(0.5, 0.05),
                                    new QuantileEpsilonPair(0.9, 0.05),
                                    new QuantileEpsilonPair(0.95, 0.01),
                                    new QuantileEpsilonPair(0.99, 0.005),
                                },
                            });
                            

                            consumer.Received += async (model, ea) =>
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                //get the message body
                                Log("Message Received...");
                                var body = ea.Body.ToArray();
                                var messageString = System.Text.Encoding.UTF8.GetString(body);
                                Log(messageString);
                                //deserialize the message
                                var sale = Newtonsoft.Json.JsonConvert.DeserializeObject<Sale>(messageString);
                                //process the sale
                                if (sale == null)
                                {
                                    Log("Cannot deserialise the sale object...");
                                    channel.BasicNack(ea.DeliveryTag, false, true);
                                }
                                else
                                {
                                    Log("Insert into DB...");
                                    await salesDBController.Insert(dbName, sale);//ProcessSale(sale);
                                    channel.BasicAck(ea.DeliveryTag, false);
                                }
                                stopwatch.Stop();
                                summary.WithLabels("MQ Receive", "salesq").Observe(stopwatch.ElapsedMilliseconds);

                            };

                            //start consuming messages
                            Log("Start consuming messages...");
                            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                            //block here until the program is terminated
                            RunWebEndPoint(args);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    //Log(ex.StackTrace);
                }
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