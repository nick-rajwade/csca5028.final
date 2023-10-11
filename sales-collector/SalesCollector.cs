using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using csca5028.lib;
using Prometheus;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace sales_collector
{
    public class SalesCollector : BackgroundService, IDisposable
    {
        private readonly ILogger<SalesCollector> _logger;
        public static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        public static string dbName = "sales_db";
        public static string hostname = "host.docker.internal";
        private static string queueName = "salesq";
        private RabbitMQ.Client.IConnectionFactory connectionFactory = new ConnectionFactory() { HostName = hostname };
        private readonly IConnection? connection = null;
        private readonly IModel? channel = null;
        private readonly EventingBasicConsumer? consumer = null;

        private csca5028.lib.SalesDBController salesDBController = new(connectionString);

        public SalesCollector(ILogger<SalesCollector> logger)
        {
            _logger = logger;
            salesDBController.Initialise(dbName).Wait();
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            
            _logger.LogInformation("Connect to sales-exchange...");
            channel.ExchangeDeclare("sales-exchange", "x-consistent-hash", true, false);
            //bind weight at 10% to the queue
            _logger.LogInformation($"bind to {queueName} queue");
            channel.QueueBind(queue: queueName, exchange: "sales-exchange", "42", new Dictionary<string, object>() { { "x-weight", 10 } });
            //create a consumer to consume messages from the queue
            consumer = new EventingBasicConsumer(channel);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (channel != null && consumer != null)
            {
                consumer.Received += async (model, result) =>
                {

                    if (result != null)
                    {
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

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        //get the message body
                        _logger.LogInformation("Message Received...");
                        var body = result.Body.ToArray();
                        var messageString = System.Text.Encoding.UTF8.GetString(body);
                        _logger.LogInformation(messageString);
                        //deserialize the message
                        var sale = Newtonsoft.Json.JsonConvert.DeserializeObject<Sale>(messageString);
                        //process the sale
                        if (sale == null)
                        {
                            _logger.LogInformation("Cannot deserialise the sale object...");
                            channel.BasicNack(result.DeliveryTag, false, true);
                        }
                        else
                        {
                            _logger.LogInformation("Insert into DB...");
                            await salesDBController.Insert(dbName, sale);//ProcessSale(sale);
                            channel.BasicAck(result.DeliveryTag, false);
                        }
                        stopwatch.Stop();
                        summary.WithLabels("MQ Receive", "salesq").Observe(stopwatch.ElapsedMilliseconds);
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                //start consuming messages
                _logger.LogInformation("Start consuming messages...");
            }
            else
            {
                _logger.LogError("Cannot start event collaboration messaging handler");
            }
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();

            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
