﻿using csca5028.lib;
using Microsoft.AspNetCore.Connections;
using Prometheus;
using RabbitMQ.Client;
using System.Collections;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace point_of_sale_app
{
    public class StoreService : BackgroundService, IDisposable
    {
        private readonly ILogger<StoreService> _logger;
        private readonly IPOSTerminalTaskQueue _taskQueue;
        private static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        private readonly string dbName = "sales_db";
        private static string hostName = "host.docker.internal";
        private StoreDBController storeDb = new(connectionString);
        private Dictionary<string,Tuple<Store,List<POSTerminal>>> storeAndTerminals = new();
        private List<System.Threading.Timer> checkOutTimers = new();
        private IConnectionFactory connectionFactory = new ConnectionFactory() { HostName = hostName };
        Gauge storesOnline = Metrics.CreateGauge("point_of_sale_app_stores_online", "Number of stores running point-of-sale");
        Gauge posOnline = Metrics.CreateGauge("point_of_sale_app_pos_online", "Number of point-of-sale terminals running");

        public StoreService(ILogger<StoreService> logger, IPOSTerminalTaskQueue taskQueue)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            storeDb.Initialise(dbName).Wait();
            storeAndTerminals = storeDb.GetStoresAndTerminalsAsync(dbName).Result;   //start queueing up the checkout timers
            
            storesOnline.Set(storeAndTerminals.Count);
            
            foreach (string storeName in storeAndTerminals.Keys)
            {
                List<POSTerminal> terminals = storeAndTerminals[storeName].Item2;
                foreach (POSTerminal terminal in terminals)
                {
                    _taskQueue.EnqueueTask(terminal, connectionFactory.CreateConnection()); //1st Task to be queued and timer to be started
                    Timer checkoutIntervalTimer = new Timer(OnCheckOutIntervalExpired,terminal,0, terminal.checkoutTime * 1000);
                    checkOutTimers.Add(checkoutIntervalTimer);
                }
                posOnline.Set(storeAndTerminals.Count);
            }
        }

        private void OnCheckOutIntervalExpired(object state)
        {
            POSTerminal terminal = (POSTerminal)state;
            _taskQueue.EnqueueTask(terminal, connectionFactory.CreateConnection());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                _logger.LogInformation("TerminalService running at: {time}", DateTimeOffset.Now);
                var terminalTask = await _taskQueue.DequeueTask();
                try
                {
                    await terminalTask;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {TerminalTask}.", nameof(terminalTask));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TerminalService is stopping.");
            foreach (System.Threading.Timer timer in checkOutTimers)
            {
                timer.Dispose();
            }
            posOnline.Set(0);
            storesOnline.Set(0);
            await base.StopAsync(stoppingToken);
        }
    }
}
