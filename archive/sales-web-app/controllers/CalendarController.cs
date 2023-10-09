using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class CalendarController : Controller
    {
        //
        // GET: /Default/
        public IActionResult CalendarFeatures()
        {
             ViewBag.minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 05);
             ViewBag.maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27);
            return View();
        }
    }
}
