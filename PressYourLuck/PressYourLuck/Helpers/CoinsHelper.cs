using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Helpers
{
    public static class CoinsHelper
    {

        private static CookieOptions expireDay = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30)
        };

        public static void SaveCurrentBet(HttpContext ctx, double bet)
        {
            ctx.Session.SetString("currentBet", bet.ToString("N2"));
        }


        //Return the current bet stored in session
        public static double GetCurrentBet(HttpContext ctx)
        {
            if (!string.IsNullOrEmpty(ctx.Session.GetString("currentBet")))
            {
                return double.Parse(ctx.Session.GetString("currentBet")); ;
            }
            else
            {
                return 0.00;
            }
        }

        //Save the original bet into session
        public static void SaveOriginalBet(HttpContext ctx, double amount)
        {
            ctx.Session.SetString("originalBet", amount.ToString("N2"));
        }

        //Get the original bet from session
        public static double GetOriginalBet(HttpContext ctx)
        {
            return double.Parse(ctx.Session.GetString("originalBet"));
        }

        //Save the players total number of coins into a cookie.  Don't forget to
        public static void SaveTotalCoins(HttpContext ctx, double coins)
        {
            ctx.Response.Cookies.Append("coinsBalance", coins.ToString("N2"), expireDay);
        }

        //Get the players total number of coins from a cookie.
        public static double GetTotalCoins(HttpContext ctx)
        {
            return double.Parse(ctx.Request.Cookies["coinsBalance"]); ;
        }

        public static string GetCtxFilter(HttpContext ctx)
        {
            return ctx.Session.GetString("filter");
        }
    }
}
