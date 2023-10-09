using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csca5028.blazor.webapp.Controllers
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
