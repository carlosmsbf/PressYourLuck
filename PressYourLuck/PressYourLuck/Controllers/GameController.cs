using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PressYourLuck.Helpers;
using PressYourLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var ctx = HttpContext;

            List<Tile> cGame = GameHelper.GetCurrentGame(ctx);

            if (!(cGame.Count() <= 0))
            {
                ViewBag.CBet = CoinsHelper.GetCurrentBet(ctx);

                GameModel model = new GameModel();

                model.CurrentBet = CoinsHelper.GetCurrentBet(ctx);
                model.Tiles = cGame;

                return View(model);
            }
            else {
                ViewBag.CBet = CoinsHelper.GetCurrentBet(ctx);
                List<Tile> tList = GameHelper.GenerateNewGame();
                GameHelper.SaveCurrentGame(ctx, tList);

                GameModel model = new GameModel();
                model.CurrentBet = CoinsHelper.GetCurrentBet(ctx);
                model.Tiles = tList;

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TurnCard(int id)
        {
            var ctx = HttpContext;

            List<Tile> cGame = GameHelper.GetCurrentGame(ctx);
            double multNumber = double.Parse(cGame[id].Value);

            GameHelper.PickTileAndUpdateGame(ctx, id);

            double bet = double.Parse(ctx.Session.GetString("currentBet"));            
            
            if (bet == 0.00)
            {
                TempData["msg"] = "Oh no! You busted out. Better luck next time!";
                return RedirectToAction("Lose", "Audit");
            }
            else
            {
                TempData["msg"] = $"CONGRATS! You found a {multNumber} multiplier!" +
                    $" All remaining values have doubled! Will you Press Your Luck?";
            }

            return RedirectToAction("Win", "Audit");
        }





    }
}
