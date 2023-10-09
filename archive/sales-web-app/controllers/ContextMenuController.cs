using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class ContextMenuController : Controller
    {
        public ActionResult ContextMenuFeatures()
        {
	    List<object> menuItems = new List<object>();
            menuItems.Add(new
            {
                text = "Cut"
            });
            menuItems.Add(new
            {
                text = "Copy"
            });
            menuItems.Add(new
            {
                text = "Paste",
                items = new List<object>()
                {
                    new { text = "Paste Text" },
                    new { text = "Paste Special" }
                }
            });
            menuItems.Add(new
            {
                separator = true
            });
            menuItems.Add(new
            {
                text = "Link"
            });
            menuItems.Add(new
            {
                text = "New Comment"
            });
            ViewBag.menuItems = menuItems;
            return View();
        }
    }
}
