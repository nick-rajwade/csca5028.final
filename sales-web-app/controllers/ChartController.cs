using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult ChartFeatures()
        {
            return View();
        }
    }
}
