using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class MessageController : Controller
    {
        public ActionResult MessageFeatures()
        {
            return View();
        }
    }
}
