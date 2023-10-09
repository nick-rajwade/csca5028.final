using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace saleswebapp.Controllers
{
    public class CarouselController : Controller
    {
        public IActionResult CarouselFeatures()
        {
            return View();
        }
    }
}
