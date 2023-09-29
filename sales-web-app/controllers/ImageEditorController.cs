using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace saleswebapp.Controllers
{
    public class ImageEditorController : Controller
    {
        //
        // GET: /Default/
        public IActionResult ImageEditorFeatures()
        {
            return View();
        }
    }
}
