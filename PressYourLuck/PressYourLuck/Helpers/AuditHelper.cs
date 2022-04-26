using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Helpers
{
    public class AuditHelper
    {

        public enum TabActived
        {
            All,
            CashIn,
            CashOut,
            Lose,
            Win
        };

        public AuditHelper(string filter = "all")
        {
            Filter = filter;
        }

        public string Filter { get; set; }

        public bool IsAll => Filter == "all";
        public bool IsWin => Filter.ToLower() == "win";
        public bool IsLose => Filter.ToLower() == "lose";
        public bool IsCashIn => Filter.ToLower() == "cash in";
        public bool IsCashOut => Filter.ToLower() == "cash out";


        public static TabActived Tab { get; set; }

        public static List<string> GetActiveTab()
        {
            List<string> openedTab = new List<string>();
            openedTab.Add("");
            openedTab.Add("");
            openedTab.Add("");
            openedTab.Add("");
            openedTab.Add("");

            if(Tab == TabActived.All)
            {
                AllFilter(openedTab);
            }
            else if (Tab == TabActived.CashIn)
            {
                CashInFilter(openedTab);
            }
            else if (Tab == TabActived.Win)
            {
                WinFilter(openedTab);
            }
            else if (Tab == TabActived.Lose)
            {
                LoseFilter(openedTab);
            }
            else if (Tab == TabActived.CashOut)
            {
                CashOutFilter(openedTab);
            }
            return openedTab;
        }

        private static void AllFilter(List<string> tab)
        {
            tab[0] = "nav-link active";
            tab[1] = "nav-link";
            tab[2] = "nav-link";
            tab[3] = "nav-link";
            tab[4] = "nav-link";
        }

        private static void CashInFilter(List<string> tab)
        {
            tab[0] = "nav-link";
            tab[1] = "nav-link";
            tab[2] = "nav-link";
            tab[3] = "nav-link";
            tab[4] = "nav-link active";
        }
        private static void WinFilter(List<string> tab)
        {
            tab[0] = "nav-link";
            tab[1] = "nav-link active";
            tab[2] = "nav-link";
            tab[3] = "nav-link";
            tab[4] = "nav-link";
        }
        private static void LoseFilter(List<string> tab)
        {
            tab[0] = "nav-link";
            tab[1] = "nav-link";
            tab[2] = "nav-link active";
            tab[3] = "nav-link";
            tab[4] = "nav-link";
        }
        private static void CashOutFilter(List<string> tab)
        {
            tab[0] = "nav-link";
            tab[1] = "nav-link";
            tab[2] = "nav-link";
            tab[3] = "nav-link active";
            tab[4] = "nav-link";
        }

    }
}
