using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PressYourLuck.Helpers;
using PressYourLuck.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var ctx = HttpContext;

            if (ctx.Request.Cookies.ContainsKey("player-name"))
            {
                List<Tile> newGame = GameHelper.GenerateNewGame();
                GameHelper.SaveCurrentGame(ctx, newGame);
                return View();
            }
               
            else
            {
                return RedirectToAction("Index", "Player");
            }

        }


        [HttpPost]
        public IActionResult Index(PlayerBet cBet)
        {
            if (!ModelState.IsValid)
            {
                return View(cBet);
            }
            else
            {
                var ctx = HttpContext;
                double totalCoins = CoinsHelper.GetTotalCoins(ctx);
                totalCoins = totalCoins - cBet.CurrentBet;
                CoinsHelper.SaveTotalCoins(ctx, totalCoins);
                CoinsHelper.SaveOriginalBet(ctx, cBet.CurrentBet);
                CoinsHelper.SaveCurrentBet(ctx, cBet.CurrentBet);
                return RedirectToAction("Index", "Game");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
