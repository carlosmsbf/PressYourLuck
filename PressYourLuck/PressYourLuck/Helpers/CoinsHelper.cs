using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Helpers
{
    public static class CoinsHelper
    {
        /*
         * Consider using this helper to Get and Set the Current Bet and the original bet
         * (both in session variables), as well as adding a Get and Set for the player's
         * total number of coins (which we'll store in Cookies)
         * 
         * HINT: Remember that HttpContext as well as Response and Request objects are not
         * available from here, so you may need to pass those in from your controller.
         * 
         * I've coded the first one for you and have created placeholders for the rest.
         * 
         */


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
        // persist the cookie (Chapter 9!)
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
