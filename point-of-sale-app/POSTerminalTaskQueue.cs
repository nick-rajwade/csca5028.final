using csca5028.lib;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using Prometheus;

namespace point_of_sale_app
{
    public class POSTerminalTaskQueue : IPOSTerminalTaskQueue
    {
        private readonly ConcurrentQueue<Task> _workItems = new ConcurrentQueue<Task>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        Gauge terminalQueueGauge = Metrics.CreateGauge("point_of_sale_app_terminal_queue_length", "Checkout Queue Length for POS Terminal Sales");

        public void EnqueueTask(POSTerminal terminal, IConnection connection)
        {
            //Queue the checkout task
            _workItems.Enqueue(terminal.Checkout(terminal, connection));
            terminalQueueGauge.Inc();
        }

        public async Task<Task> DequeueTask()
        {
            await _signal.WaitAsync();
            _workItems.TryDequeue(out var workItem);
            terminalQueueGauge.Dec();
            return workItem;
        }
    }
}
