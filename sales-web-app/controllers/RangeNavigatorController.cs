using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class RangeNavigatorController : Controller
    {
        //
        // GET: /Default/
        public ActionResult RangeNavigatorFeatures()
        {
            return View();
        }
    }
}
