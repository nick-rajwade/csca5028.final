using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class DropdownTreeController : Controller
    {
        public ActionResult DropdownTreeFeatures()
        {
		 List<TreeData> TreeDataSource = new List<TreeData>();
            TreeDataSource.Add(new TreeData
            {
                Id = 1,
                Name = "China",
                HasChild = true,
                Code = "CN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 2,
                Pid = 1,
                Name = "Guangzhou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 3,
                Pid = 1,
                Name = "Shanghai"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 4,
                Pid = 1,
                Name = "Beijing"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 5,
                Pid = 1,
                Name = "Shantou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 6,
                Name = "India",
                HasChild = true,
                Code = "IN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 7,
                Pid = 6,
                Name = "Assam"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 8,
                Pid = 6,
                Name = "Bihar"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 9,
                Pid = 6,
                Name = "Tamil Nadu"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 10,
                Pid = 6,
                Name = "Punjab"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 11,
                Name = "Australia",
                HasChild = true,
                Code = "AU",
                Expanded = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 12,
                Pid = 11,
                Name = "Victoria",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 13,
                Pid = 11,
                Name = "New South Wales",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 14,
                Pid = 11,
                Name = "Western Australia",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 15,
                Pid = 11,
                Name = "South Australia"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 16,
                Name = "France",
                HasChild = true,
                Code = "FR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 17,
                Pid = 16,
                Name = "Pays de la Loire"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 18,
                Pid = 16,
                Name = "Aquitaine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 19,
                Pid = 16,
                Name = "Brittany"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 20,
                Pid = 16,
                Name = "Lorraine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 21,
                Name = "Brazil",
                HasChild = true,
                Code = "BR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 22,
                Pid = 21,
                Name = "Mato Grosso"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 23,
                Pid = 21,
                Name = "Roraima"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 24,
                Pid = 21,
                Name = "Acre"
            });
            ViewBag.dataSource = TreeDataSource;
            return View();
        }
        public IActionResult CheckBox()
        {
            List<TreeData> TreeDataSource = new List<TreeData>();
            TreeDataSource.Add(new TreeData
            {
                Id = 1,
                Name = "China",
                HasChild = true,
                Code = "CN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 2,
                Pid = 1,
                Name = "Guangzhou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 3,
                Pid = 1,
                Name = "Shanghai"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 4,
                Pid = 1,
                Name = "Beijing"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 5,
                Pid = 1,
                Name = "Shantou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 6,
                Name = "India",
                HasChild = true,
                Code = "IN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 7,
                Pid = 6,
                Name = "Assam"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 8,
                Pid = 6,
                Name = "Bihar"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 9,
                Pid = 6,
                Name = "Tamil Nadu"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 10,
                Pid = 6,
                Name = "Punjab"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 11,
                Name = "Australia",
                HasChild = true,
                Code = "AU",
                Expanded = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 12,
                Pid = 11,
                Name = "Victoria",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 13,
                Pid = 11,
                Name = "New South Wales",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 14,
                Pid = 11,
                Name = "Western Australia",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 15,
                Pid = 11,
                Name = "South Australia"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 16,
                Name = "France",
                HasChild = true,
                Code = "FR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 17,
                Pid = 16,
                Name = "Pays de la Loire"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 18,
                Pid = 16,
                Name = "Aquitaine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 19,
                Pid = 16,
                Name = "Brittany"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 20,
                Pid = 16,
                Name = "Lorraine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 21,
                Name = "Brazil",
                HasChild = true,
                Code = "BR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 22,
                Pid = 21,
                Name = "Mato Grosso"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 23,
                Pid = 21,
                Name = "Roraima"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 24,
                Pid = 21,
                Name = "Acre"
            });
            ViewBag.dataSource = TreeDataSource;
            return View();
        }
        public IActionResult MultiSelection()
        {
            List<TreeData> TreeDataSource = new List<TreeData>();
            TreeDataSource.Add(new TreeData
            {
                Id = 1,
                Name = "China",
                HasChild = true,
                Code = "CN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 2,
                Pid = 1,
                Name = "Guangzhou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 3,
                Pid = 1,
                Name = "Shanghai"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 4,
                Pid = 1,
                Name = "Beijing"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 5,
                Pid = 1,
                Name = "Shantou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 6,
                Name = "India",
                HasChild = true,
                Code = "IN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 7,
                Pid = 6,
                Name = "Assam"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 8,
                Pid = 6,
                Name = "Bihar"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 9,
                Pid = 6,
                Name = "Tamil Nadu"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 10,
                Pid = 6,
                Name = "Punjab"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 11,
                Name = "Australia",
                HasChild = true,
                Code = "AU",
                Expanded = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 12,
                Pid = 11,
                Name = "Victoria",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 13,
                Pid = 11,
                Name = "New South Wales",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 14,
                Pid = 11,
                Name = "Western Australia",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 15,
                Pid = 11,
                Name = "South Australia"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 16,
                Name = "France",
                HasChild = true,
                Code = "FR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 17,
                Pid = 16,
                Name = "Pays de la Loire"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 18,
                Pid = 16,
                Name = "Aquitaine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 19,
                Pid = 16,
                Name = "Brittany"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 20,
                Pid = 16,
                Name = "Lorraine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 21,
                Name = "Brazil",
                HasChild = true,
                Code = "BR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 22,
                Pid = 21,
                Name = "Mato Grosso"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 23,
                Pid = 21,
                Name = "Roraima"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 24,
                Pid = 21,
                Name = "Acre"
            });
            ViewBag.dataSource = TreeDataSource;
            return View();
        }
        public IActionResult Template()
        {
            List<TreeData> TreeDataSource = new List<TreeData>();
            TreeDataSource.Add(new TreeData
            {
                Id = 1,
                Name = "China",
                HasChild = true,
                Code = "CN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 2,
                Pid = 1,
                Name = "Guangzhou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 3,
                Pid = 1,
                Name = "Shanghai"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 4,
                Pid = 1,
                Name = "Beijing"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 5,
                Pid = 1,
                Name = "Shantou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 6,
                Name = "India",
                HasChild = true,
                Code = "IN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 7,
                Pid = 6,
                Name = "Assam"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 8,
                Pid = 6,
                Name = "Bihar"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 9,
                Pid = 6,
                Name = "Tamil Nadu"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 10,
                Pid = 6,
                Name = "Punjab"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 11,
                Name = "Australia",
                HasChild = true,
                Code = "AU",
                Expanded = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 12,
                Pid = 11,
                Name = "Victoria",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 13,
                Pid = 11,
                Name = "New South Wales",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 14,
                Pid = 11,
                Name = "Western Australia",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 15,
                Pid = 11,
                Name = "South Australia"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 16,
                Name = "France",
                HasChild = true,
                Code = "FR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 17,
                Pid = 16,
                Name = "Pays de la Loire"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 18,
                Pid = 16,
                Name = "Aquitaine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 19,
                Pid = 16,
                Name = "Brittany"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 20,
                Pid = 16,
                Name = "Lorraine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 21,
                Name = "Brazil",
                HasChild = true,
                Code = "BR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 22,
                Pid = 21,
                Name = "Mato Grosso"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 23,
                Pid = 21,
                Name = "Roraima"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 24,
                Pid = 21,
                Name = "Acre"
            });
            ViewBag.dataSource = TreeDataSource;
            return View();
        }
        public IActionResult Filtering()
        {
            List<TreeData> TreeDataSource = new List<TreeData>();
            TreeDataSource.Add(new TreeData
            {
                Id = 1,
                Name = "China",
                HasChild = true,
                Code = "CN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 2,
                Pid = 1,
                Name = "Guangzhou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 3,
                Pid = 1,
                Name = "Shanghai"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 4,
                Pid = 1,
                Name = "Beijing"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 5,
                Pid = 1,
                Name = "Shantou",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 6,
                Name = "India",
                HasChild = true,
                Code = "IN"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 7,
                Pid = 6,
                Name = "Assam"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 8,
                Pid = 6,
                Name = "Bihar"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 9,
                Pid = 6,
                Name = "Tamil Nadu"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 10,
                Pid = 6,
                Name = "Punjab"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 11,
                Name = "Australia",
                HasChild = true,
                Code = "AU",
                Expanded = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 12,
                Pid = 11,
                Name = "Victoria",
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 13,
                Pid = 11,
                Name = "New South Wales",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 14,
                Pid = 11,
                Name = "Western Australia",
                IsSelected = true
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 15,
                Pid = 11,
                Name = "South Australia"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 16,
                Name = "France",
                HasChild = true,
                Code = "FR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 17,
                Pid = 16,
                Name = "Pays de la Loire"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 18,
                Pid = 16,
                Name = "Aquitaine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 19,
                Pid = 16,
                Name = "Brittany"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 20,
                Pid = 16,
                Name = "Lorraine"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 21,
                Name = "Brazil",
                HasChild = true,
                Code = "BR"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 22,
                Pid = 21,
                Name = "Mato Grosso"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 23,
                Pid = 21,
                Name = "Roraima"
            });
            TreeDataSource.Add(new TreeData
            {
                Id = 24,
                Pid = 21,
                Name = "Acre"
            });
            ViewBag.dataSource = TreeDataSource;
            return View();
        }
    }
     class TreeData
        {
            public int Id { get; set; }
            public int? Pid { get; set; }
            public bool HasChild { get; set; }
            public string Code { get; set; }
            public bool Expanded { get; set; }
            public bool IsSelected { get; set; }
            public string Name { get; set; }
        }
}
