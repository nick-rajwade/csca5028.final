using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace saleswebapp.Controllers
{
    public partial class DashboardLayoutController : Controller
    {
        public ActionResult DashboardLayoutFeatures()
        {
            ViewBag.usmap = getusMap();
            ViewBag.electiondata = GetElectiondata();
            ViewBag.marker = getmarker();
            return View();
        }

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
