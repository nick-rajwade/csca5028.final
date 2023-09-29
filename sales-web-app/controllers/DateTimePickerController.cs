using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace saleswebapp.Controllers
{
    public class DateTimeValue
    {
        [Required(ErrorMessage = "Please enter the value")]
        public DateTime? value { get; set; }
    }
    public partial class DateTimePickerController : Controller
    {
        public ActionResult DateTimePickerFeatures()
        {
            DateTimeValue val = new DateTimeValue();
            val.value = DateTime.Now;
            return View(val);
            
        }
        [HttpPost]
        public IActionResult DateTimePickerFeatures(DateTimeValue model)
        {
            DateTimeValue val = new DateTimeValue();
            val.value = model.value;
            return View(val);
        }
    }
}
