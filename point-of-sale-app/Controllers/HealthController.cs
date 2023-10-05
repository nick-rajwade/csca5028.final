using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace point_of_sale_app.Controllers
{
    [ApiController]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/ping")]
        public string Get()
        {
            return "pong";
        }

        [HttpGet("/qping")]
        public string GetStoreHealth(string qname)
        {
            var factory = new ConnectionFactory() { HostName = "rmq0" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.ExchangeDeclare(exchange: "healthcheck", type: ExchangeType.Direct);
                    BasicGetResult msgResp = channel.BasicGet(queue: qname, autoAck: true);
                    if(msgResp == null)
                    {
                        BadRequest();
                    }
                    var msgBody = System.Text.Encoding.UTF8.GetString(msgResp.Body.ToArray());
                    return msgBody;
                }
            }
        }
    }

}
