using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csca5028.lib;

namespace point_of_sale_app.Controllers
{
    [Route("/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        [HttpGet(Name = "stores")]
        public List<Store> Get()
        {
            StoreDBController storeDb = new(Program.connectionString);
            return (List<Store>) storeDb.GetStoresAsync(Program.dbName).Result;
        }
    }
}
