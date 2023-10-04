using Microsoft.AspNetCore.Mvc;

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
        public string GetStoreHealth(string name)
        {
            return $"{name} is good";
        }
    }

}
