using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class SmithChartController : Controller
    {
        //
        // GET: /SmithChart/
        public ActionResult SmithChartFeatures()
        {
            return View();
        }
    }
}
