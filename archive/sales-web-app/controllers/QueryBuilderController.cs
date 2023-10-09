using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.QueryBuilder;
namespace saleswebapp.Controllers
{
    public partial class QueryBuilderController : Controller
    {
        public IActionResult QueryBuilderFeatures()
        {
            QueryBuilderRule rule = new QueryBuilderRule()
            {
                Condition = "and",
                Rules = new List<QueryBuilderRule>()
                {
                    new QueryBuilderRule { Label="Employee ID", Field="EmployeeID", Type="number", Operator="equal", Value = 1 },
                    new QueryBuilderRule { Label="Title", Field="Title", Type="string", Operator="equal", Value = "Sales Manager" }
                }
            };
            List<string> values = new List<string> { "Mr.", "Mrs." };
            ViewBag.rule = rule;
            ViewBag.values = values;
            return View();
        }
    }
}
