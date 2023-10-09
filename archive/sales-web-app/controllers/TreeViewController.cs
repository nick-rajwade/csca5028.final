using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Navigations;
namespace saleswebapp.Controllers
{
    public class TreeViewController : Controller
    {
        public IActionResult TreeViewFeatures()
        {
	    TreeViewTemplate template = new TreeViewTemplate();
            ViewBag.data = template.getTreeViewTemplate();
            return View();
        }
    }
	public class TreeViewTemplate
{
    public string name { get; set; }
    public string count { get; set; }
    public int id { get; set; }
    public int pid { get; set; }
    public bool HasChild { get; set; }
    public bool Expanded { get; set; }
    public bool Selected { get; set; }
    public List<TreeViewTemplate> getTreeViewTemplate()
    {
        List<TreeViewTemplate> localData = new List<TreeViewTemplate>();
        localData.Add(new TreeViewTemplate { id = 1, name = "Favorites", HasChild = true });
        localData.Add(new TreeViewTemplate { id = 2, pid = 1, name = "Sales Reports", count = "4" });
        localData.Add(new TreeViewTemplate { id = 3, pid = 1, name = "Sent Items" });
        localData.Add(new TreeViewTemplate { id = 4, pid = 1, name = "Marketing Reports ", count = "6" });
        localData.Add(new TreeViewTemplate { id = 5, name = "My Folder", HasChild = true, Expanded = true });
        localData.Add(new TreeViewTemplate { id = 6, pid = 5, name = "Inbox", Selected = true, count = "20" });
        localData.Add(new TreeViewTemplate { id = 7, pid = 5, name = "Drafts", count = "5" });
        localData.Add(new TreeViewTemplate { id = 8, pid = 5, name = "Deleted Items" });
        localData.Add(new TreeViewTemplate { id = 9, pid = 5, name = "Sent Items" });
        localData.Add(new TreeViewTemplate { id = 10, pid = 5, name = "Sales Reports", count = "4" });
        localData.Add(new TreeViewTemplate { id = 11, pid = 5, name = "Marketing Reports", count = "6" });
        localData.Add(new TreeViewTemplate { id = 12, pid = 5, name = "Outbox" });
        return localData;
    }
}
}
