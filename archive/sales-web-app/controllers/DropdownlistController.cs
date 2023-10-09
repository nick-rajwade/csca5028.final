using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class DropdownlistController : Controller
    {
        //
        // GET: /DropdownlistDefault/
        public ActionResult DropdownlistFeatures()
        {
 ViewBag.data = new DropdownlistGameList().DropdownlistGameLists();
		return View();
        }
    }
            public class DropdownlistGameList
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public List<DropdownlistGameList> DropdownlistGameLists()
        {
            List<DropdownlistGameList> game = new List<DropdownlistGameList>();
            game.Add(new DropdownlistGameList { Id = "Game1", Game = "American Football" });
            game.Add(new DropdownlistGameList { Id = "Game2", Game = "Badminton" });
            game.Add(new DropdownlistGameList { Id = "Game3", Game = "Basketball" });
            game.Add(new DropdownlistGameList { Id = "Game4", Game = "Cricket" });
            game.Add(new DropdownlistGameList { Id = "Game5", Game = "Football" });
            game.Add(new DropdownlistGameList { Id = "Game6", Game = "Golf" });
            game.Add(new DropdownlistGameList { Id = "Game7", Game = "Hockey" });
            game.Add(new DropdownlistGameList { Id = "Game8", Game = "Rugby" });
            game.Add(new DropdownlistGameList { Id = "Game9", Game = "Snooker" });
            game.Add(new DropdownlistGameList { Id = "Game10", Game = "Tennis" });
            return game;
        }
    }
}
