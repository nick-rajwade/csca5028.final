using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class ChipsController : Controller
    {
        //
        // GET: /Default/
        public IActionResult ChipsFeatures()
        {
            int[] choiceSelected = { 1 };
            int[] filterSelected = { 1, 2 };
            ViewBag.choiceSelected = choiceSelected;
            ViewBag.filterSelected = filterSelected;
            return View();
        }
    }
}
