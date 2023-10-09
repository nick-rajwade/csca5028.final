using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public class ToolBarController : Controller
    {
        public IActionResult ToolBarFeatures()
        {
          return View();
        }
    }
}
