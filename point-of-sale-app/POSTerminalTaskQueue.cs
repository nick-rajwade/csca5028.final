using csca5028.lib;
using RabbitMQ.Client;
using System.Collections.Concurrent;

namespace point_of_sale_app
{
    public class POSTerminalTaskQueue : IPOSTerminalTaskQueue
    {
        private readonly ConcurrentQueue<Task> _workItems = new ConcurrentQueue<Task>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void EnqueueTask(POSTerminal terminal, IConnection connection)
        {
            //Queue the checkout task
            _workItems.Enqueue(terminal.Checkout(terminal, connection));
        }

        public async Task<Task> DequeueTask()
        {
            await _signal.WaitAsync();
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }
    }
}
