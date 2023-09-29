using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class InplaceEditorController : Controller
    {
        public IActionResult InplaceEditorFeatures()
        {
              ViewBag.data = new { placeholder = "Enter employee name" };
			  ViewBag.value = "Andrew";
            return View();
        }        
    }
}
