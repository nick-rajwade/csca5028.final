using Microsoft.AspNetCore.Mvc;

namespace point_of_sale_app.Controllers
{
    [ApiController]
    [Route("/ping")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet(Name = "ping")]
        public string Get()
        {
            return "pong";
        }
    }
}
