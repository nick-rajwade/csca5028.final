using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class BreadcrumbController : Controller
    {
        public ActionResult BreadcrumbFeatures()
        {
            return View();
        }
    }
}
