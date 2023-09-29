using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace saleswebapp.Controllers
{
    public class DateValue
    {
        [Required(ErrorMessage = "Please enter the value")]
        public DateTime? value { get; set; }
    }
    public partial class DatePickerController : Controller
    {
        public IActionResult DatePickerFeatures()
        {
            DateValue val = new DateValue();
            val.value = DateTime.Now;
            return View(val);
        }
	[HttpPost]
        public IActionResult DatePickerFeatures(DateValue model)
        {
            DateValue val = new DateValue();
            val.value = model.value;
            return View(val);
        }
    }
}
