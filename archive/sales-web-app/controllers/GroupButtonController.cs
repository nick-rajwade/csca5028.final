using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class GroupButtonController : Controller
    {
        //
        // GET: /GroupButtonDefault/
        public ActionResult GroupButtonFeatures()
        {
            return View();
        }
    }
}
