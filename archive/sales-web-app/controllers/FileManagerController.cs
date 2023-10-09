using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class FileManagerController : Controller
    {
        public ActionResult FileManagerFeatures()
        {
		 String[] items = new string[11] {"NewFolder", "Upload", "Delete", "Download", "Rename", "SortBy", "Refresh", "Selection", "View", "Details", "Custom Toolbar"};
            ViewBag.items = items;
            return View();
        }
    }
}
