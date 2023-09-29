using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace saleswebapp.Controllers
{
    public partial class MapsController : Controller
    {
        //
        // GET: /DaraMarker/
        public ActionResult MapsFeatures()
        {
	    ViewBag.usmap = getusMap();
            ViewBag.electiondata = GetElectiondata();
            ViewBag.marker = getmarker();
            return View();
        }
        public object getmarker()
        {
            string allText = System.IO.File.ReadAllText("wwwroot/scripts/Maps/markerlocation.js");
            return JsonConvert.DeserializeObject(allText);
        }
        public object getusMap()
        {
            string allText = System.IO.File.ReadAllText("wwwroot/scripts/Maps/usMap.js");
            return JsonConvert.DeserializeObject(allText);
        }
        public object GetElectiondata()
        {
            string allText = System.IO.File.ReadAllText("wwwroot/scripts/Maps/electiondata.js");
            return JsonConvert.DeserializeObject(allText);
        }
    }
}
