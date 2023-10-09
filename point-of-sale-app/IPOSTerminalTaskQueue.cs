using csca5028.lib;
using RabbitMQ.Client;

namespace point_of_sale_app
{
    public interface IPOSTerminalTaskQueue
    {
        void EnqueueTask(POSTerminal terminal, IConnection connection);
        Task<Task> DequeueTask();
    }
}
