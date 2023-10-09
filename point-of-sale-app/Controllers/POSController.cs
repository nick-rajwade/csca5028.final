using csca5028.lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace point_of_sale_app.Controllers
{
    [Route("/terminals")]
    [ApiController]
    public class POSController : ControllerBase
    {
        private static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        private static string dbName = "sales_db";

        [HttpGet("{storeid}", Name = "terminals")]
        public List<POSTerminal> GetTerminals(string id)
        {
            StoreDBController storeDb = new(connectionString);
            List<POSTerminal> terminals = (List<POSTerminal>)storeDb.GetTerminalsAsync(Guid.Parse(id), dbName).Result;
            return terminals;
        }
    }
}
