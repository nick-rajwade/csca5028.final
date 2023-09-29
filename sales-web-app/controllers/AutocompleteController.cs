using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class AutocompleteController : Controller
    {
        // GET: /AutocompleteDefault/       
        public ActionResult AutocompleteFeatures()
        {
              ViewBag.data = new AutocompleteGameList().AutocompleteGameLists();
            return View();
        }
    }
              public class AutocompleteGameList
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public List<AutocompleteGameList> AutocompleteGameLists()
        {
            List<AutocompleteGameList> game = new List<AutocompleteGameList>();
            game.Add(new AutocompleteGameList { Id = "Game1", Game = "American Football" });
            game.Add(new AutocompleteGameList { Id = "Game2", Game = "Badminton" });
            game.Add(new AutocompleteGameList { Id = "Game3", Game = "Basketball" });
            game.Add(new AutocompleteGameList { Id = "Game4", Game = "Cricket" });
            game.Add(new AutocompleteGameList { Id = "Game5", Game = "Football" });
            game.Add(new AutocompleteGameList { Id = "Game6", Game = "Golf" });
            game.Add(new AutocompleteGameList { Id = "Game7", Game = "Hockey" });
            game.Add(new AutocompleteGameList { Id = "Game8", Game = "Rugby" });
            game.Add(new AutocompleteGameList { Id = "Game9", Game = "Snooker" });
            game.Add(new AutocompleteGameList { Id = "Game10", Game = "Tennis" });
            return game;
        }
    }
}
