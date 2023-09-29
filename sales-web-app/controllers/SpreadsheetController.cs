using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.Spreadsheet;
using System.Collections.Generic;
namespace saleswebapp.Controllers
{
    public class SpreadsheetController : Controller
    {
        public IActionResult SpreadsheetFeatures()
        {
            List<OrderDetails> defaultData = new List<OrderDetails>();
            defaultData.Add(new OrderDetails { Item = "Shoes", Quantity = 10, Price = 20, Discount = 10, Amount = 190 });
            defaultData.Add(new OrderDetails { Item = "Sandals", Quantity = 15, Price = 20, Discount = 15, Amount = 285, });
            defaultData.Add(new OrderDetails { Item = "Slippers", Quantity = 30, Price = 10, Discount = 45, Amount = 255 });
            defaultData.Add(new OrderDetails { Item = "Sneakers", Quantity = 40, Price = 20, Discount = 40, Amount = 760 });
            defaultData.Add(new OrderDetails { Item = "Floaters", Quantity = 20, Price = 10, Discount = 10, Amount = 190 });
            defaultData.Add(new OrderDetails { Item = "Loafers", Quantity = 30, Price = 10, Discount = 15, Amount = 285 });
            defaultData.Add(new OrderDetails { Item = "Shirts", Quantity = 40, Price = 30, Discount = 80, Amount = 1120 });
            defaultData.Add(new OrderDetails { Item = "T-Shirts", Quantity = 50, Price = 10, Discount = 25, Amount = 475 });
            ViewBag.defaultData = defaultData;
            return View();
        }
        public IActionResult Open(IFormCollection openRequest)
        {
            OpenRequest open = new OpenRequest();
            open.File = openRequest.Files[0];
            return Content(Workbook.Open(open));
        }
        public IActionResult Save(SaveSettings saveSettings)
        {
            return Workbook.Save(saveSettings);
        }
    }
    public class OrderDetails
    {
        public string Item;
        public int Quantity;
        public int Price;
        public int Discount;
        public int Amount;
    }
}
