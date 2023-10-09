using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class CardController : Controller
    {
        //
        // GET: /CardDefault/
        public ActionResult CardFeatures()
        {
            return View();
        }
    }
}
