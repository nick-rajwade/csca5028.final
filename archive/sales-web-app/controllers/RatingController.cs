using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public class RatingController : Controller
    {
        public ActionResult RatingFeatures()
        {
            return View();
        }
    }
}
