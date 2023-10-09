using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class MaskedTextBoxController: Controller
    {
        public ActionResult MaskedTextBoxFeatures()
        {
                          CustomCharacters customChar = new CustomCharacters();
            customChar.P = "P,A,a,p";
            customChar.M = "M,m";
            ViewBag.CustomChars = customChar;
            return View();
         } 
    }
        public class CustomCharacters
        {
            public string P { get; set; }
            public string M { get; set; }
        }
}
