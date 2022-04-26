using Microsoft.AspNetCore.Mvc;
using PressYourLuck.Helpers;
using PressYourLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Controllers
{
    public class PlayerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(Player person)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please, correct what is highlighted.");
                return View(person);
            }
            else
            {
                var ctx = HttpContext;
                CoinsHelper.SaveTotalCoins(ctx, person.Balance);
                GameHelper.SaveCurrentName(ctx, person.Name);
                ViewBag.record = person;
                return RedirectToAction("CashIn", "Audit", person);
            }
        }


        [HttpGet]
        public IActionResult CashOut()
        {
            var ctx = HttpContext;
            double total = GameHelper.TakeCoins(ctx);

            TempData["msg"] = $"{GameHelper.GetCurrentName(ctx)}, you cashed out with {total.ToString("C2")}";

            return RedirectToAction("CashOut", "Audit");
        }

        [HttpGet]
        public IActionResult TakeCoins()
        {
            var ctx = HttpContext;
            double total = GameHelper.TakeCoins(ctx);

            TempData["msg"] = $"Congratulations!! You have taken " + $"{CoinsHelper.GetCurrentBet(ctx).ToString("C2")}. " +
                $"Now, the balance of you account is {total.ToString("C2") } ! " + $"\nDo you want to try your luck again?";

            ctx.Session.Remove("currentBet");
            ctx.Session.Remove("originalBet");

            List<Tile> newGame = GameHelper.GenerateNewGame();
            GameHelper.SaveCurrentGame(ctx, newGame);
            return RedirectToAction("Index", "Home");
        }
    }
}
