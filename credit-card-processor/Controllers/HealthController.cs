using Microsoft.AspNetCore.Mvc;

namespace credit_card_processor.Controllers
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
