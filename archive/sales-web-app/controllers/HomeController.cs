using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using saleswebapp.Models;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;

namespace saleswebapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int TotalTxnPerMin()
        {
			Random random = new Random();
            return random.Next(1, 100);
		}
    }

	public class SpacingModel
	{
		public double[] cellSpacing { get; set; }
	}
}

 