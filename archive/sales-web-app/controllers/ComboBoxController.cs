using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace saleswebapp.Controllers
{
    public partial class ComboBoxController: Controller
    {
        public ActionResult ComboBoxFeatures()
        {
		    ViewBag.data = new ComboBoxGameList().ComboBoxGameLists();
            return View();
         } 
    }
		   public class ComboBoxGameList
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public List<ComboBoxGameList> ComboBoxGameLists()
        {
            List<ComboBoxGameList> game = new List<ComboBoxGameList>();
            game.Add(new ComboBoxGameList { Id = "Game1", Game = "American Football" });
            game.Add(new ComboBoxGameList { Id = "Game2", Game = "Badminton" });
            game.Add(new ComboBoxGameList { Id = "Game3", Game = "Basketball" });
            game.Add(new ComboBoxGameList { Id = "Game4", Game = "Cricket" });
            game.Add(new ComboBoxGameList { Id = "Game5", Game = "Football" });
            game.Add(new ComboBoxGameList { Id = "Game6", Game = "Golf" });
            game.Add(new ComboBoxGameList { Id = "Game7", Game = "Hockey" });
            game.Add(new ComboBoxGameList { Id = "Game8", Game = "Rugby" });
            game.Add(new ComboBoxGameList { Id = "Game9", Game = "Snooker" });
            game.Add(new ComboBoxGameList { Id = "Game10", Game = "Tennis" });
            return game;
        }
    }
}
