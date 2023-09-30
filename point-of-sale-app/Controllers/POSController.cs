using csca5028.lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace point_of_sale_app.Controllers
{
    [Route("/terminals")]
    [ApiController]
    public class POSController : ControllerBase
    {
        /*[HttpGet("{id}", Name = "terminalcount")]
        public int Get(string id)
        {
            StoreDBController storeDb = new(Program.connectionString);
            List<POSTerminal> terminals = (List<POSTerminal>)storeDb.GetTerminalsAsync(Guid.Parse(id), Program.dbName).Result;
            return terminals.Count;
        }*/

        [HttpGet("{storeid}", Name = "terminals")]
        public List<POSTerminal> GetTerminals(string id)
        {
            StoreDBController storeDb = new(Program.connectionString);
            List<POSTerminal> terminals = (List<POSTerminal>)storeDb.GetTerminalsAsync(Guid.Parse(id), Program.dbName).Result;
            return terminals;
        }
    }
}
