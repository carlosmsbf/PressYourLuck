using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PressYourLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Helpers
{
    public static class GameHelper
    {
        //This creates a collection of 12 tiles with randomly generated values

        private static CookieOptions expireDay = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30)
        };


        public static List<Tile> GenerateNewGame()
        {
            var tileList = new List<Tile>();
            Random r = new Random();
            for (int i = 0; i < 12; i++)
            {
                double randomValue = 0;
                if (r.Next(1,4) != 1)
                {
                    randomValue = (r.NextDouble() + 0.5) * 2;
                }

                var tile = new Tile()
                {
                    TileIndex = i,
                    Visible = false,
                    Value = randomValue.ToString("N2")
                };

                tileList.Add(tile);
            }
            return tileList;
        }


        public static List<Tile> GetCurrentGame(HttpContext ctx)
        {
            //code goes here
            List<Tile> listOfTiles = new List<Tile>();
            string amount = ctx.Session.GetString("tList");
            
                listOfTiles = JsonConvert.DeserializeObject<List<Tile>>(amount);
                return listOfTiles;
        }

        // - SaveCurrentGame - Save the current state of the game to session. 
        public static void SaveCurrentGame(HttpContext ctx, List<Tile>tiles)
        {
            ctx.Session.SetString("tList", JsonConvert.SerializeObject(tiles));
        }

       /* - PickATileAndUpdateGame - code that contains the game logic as 
        * mentioned in Part 4 of the assignment. Hint: you'll need to pass in the
        * id of the selected tile as one of the parameters.
        */
        public static void PickTileAndUpdateGame(HttpContext ctx, int id)
        {

            List<Tile> cGame = GetCurrentGame(ctx);
            double multValue = double.Parse(cGame[id].Value);

            SaveCurrentGame(ctx, cGame);

            double newBet = CoinsHelper.GetCurrentBet(ctx) * multValue;

            CoinsHelper.SaveCurrentBet(ctx, newBet);

            if (multValue == 0.00)
            {
                for (int index = 0; index < cGame.Count; index++)
                {
                    cGame[index].Visible = true;
                }
                SaveCurrentGame(ctx, cGame);
            }

            else
            {
                cGame[id].Visible = true;
                for (int index = 0; index < cGame.Count; index++)
                {
                    if (cGame[index] != cGame[id])
                    {
                        cGame[index].Value = (double.Parse(cGame[index].Value) * 2).ToString("N2");
                    }
                }
                SaveCurrentGame(ctx, cGame);
            }
        }

        // - ClearCurrentGame - clear the current game state from session
        public static void ClearCurrentGame(HttpContext ctx)
        {
            
            ctx.Session.Remove("tList");
            ctx.Session.Remove("originalBet");
            ctx.Session.Remove("currentBet");
        }


        public static void SaveCurrentName(HttpContext ctx, string name)
        {
            ctx.Response.Cookies.Append("player-name", name, expireDay);
        }

        public static string GetCurrentName(HttpContext ctx)
        {
            return ctx.Request.Cookies["player-name"];
        }
        public static double TakeCoins(HttpContext context)
        {
            double totalCoins = CoinsHelper.GetTotalCoins(context);
            double currentBet = CoinsHelper.GetCurrentBet(context);
            totalCoins = totalCoins + currentBet;
            CoinsHelper.SaveTotalCoins(context, totalCoins);
            return totalCoins;
        }
    }
}
