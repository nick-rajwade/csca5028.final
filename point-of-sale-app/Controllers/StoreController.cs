using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csca5028.lib;

namespace point_of_sale_app.Controllers
{
    [Route("/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        private static string dbName = "sales_db";
        [HttpGet(Name = "stores")]
        public List<Store> Get()
        {
            StoreDBController storeDb = new(connectionString);
            return (List<Store>) storeDb.GetStoresAsync(dbName).Result;
        }
    }
}
