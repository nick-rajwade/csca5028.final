using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace saleswebapp.Controllers
{
    public class TreeMapController : Controller
    {
        public IActionResult TreeMapFeatures()
        {
	    ViewBag.dataSource = this.getDataSource("Election");
            return View();
        }
	public object getDataSource(string filename)
        {
            string allText = System.IO.File.ReadAllText("wwwroot/scripts/Treemap/Election.js");
            return JsonConvert.DeserializeObject(allText);
        }
    }
}
