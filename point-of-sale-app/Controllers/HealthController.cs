using Microsoft.AspNetCore.Mvc;

namespace point_of_sale_app.Controllers
{
    [ApiController]
    [Route("/{action}/{id?}")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("ping")]
        public string Get()
        {
            return "pong";
        }

        [HttpGet]
        [ActionName("ping")]
        public string GetStoreHealth(string name)
        {
            return $"{name} is good";
        }
    }

}
