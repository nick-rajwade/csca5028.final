using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class TooltipController : Controller
    {
        //
        // GET: /TooltipDefault/
        public ActionResult TooltipFeatures()
        {
            return View();
        }
    }
}
