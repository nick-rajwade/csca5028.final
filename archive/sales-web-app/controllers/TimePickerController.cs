using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace saleswebapp.Controllers
{
    public class TimeValue
    {
        [Required(ErrorMessage = "Please enter the value")]
        public DateTime? value { get; set; }
    }
    public partial class TimePickerController : Controller
    {
        //
        // GET: /TimePickerDefault/
        public ActionResult TimePickerFeatures()
        {
            TimeValue val = new TimeValue();
            val.value = DateTime.Now;
            return View(val);
            
        }
        [HttpPost]
        public IActionResult TimePickerFeatures(TimeValue model)
        {
            TimeValue val = new TimeValue();
            val.value = model.value;
            return View(val);
        }
    }
}
