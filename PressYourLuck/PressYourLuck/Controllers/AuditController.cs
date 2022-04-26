using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PressYourLuck.Helpers;
using PressYourLuck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressYourLuck.Controllers
{
    public class AuditController : Controller
    {

        private AuditContext auditCtx;

        public AuditController(AuditContext ctx)
        {
            this.auditCtx = ctx;
        }

        [HttpGet]
        public IActionResult CashOut()
        {

            var ctx = HttpContext;

            Audit record = new Audit();

            record.Name = ctx.Request.Cookies["player-name"];
            record.CreatedDate = DateTime.Now;
            record.Amount = CoinsHelper.GetTotalCoins(ctx);
            record.AuditTypeId = 2;

            auditCtx.Audits.Update(record);
            auditCtx.SaveChanges();


            ctx.Session.Remove("currentBet");
            ctx.Session.Remove("originalbet");

            ctx.Response.Cookies.Delete("player-name");
            ctx.Response.Cookies.Delete("coinsBalance");

            return RedirectToAction("Index", "Player");
        }

        [HttpGet]
        public IActionResult CashIn(Player player)
        {
            var ctx = HttpContext;

            Audit record = new Audit();

                record.Name = ctx.Request.Cookies["player-name"];
                record.CreatedDate = DateTime.Now;
                record.Amount = player.Balance;
                record.AuditTypeId = 1;

            auditCtx.Audits.Update(record);
            auditCtx.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Lose()
        {
            var ctx = HttpContext;

            Audit record = new Audit()
            {
                Name = ctx.Request.Cookies["player-name"],
                CreatedDate = DateTime.Now,
                Amount = CoinsHelper.GetOriginalBet(ctx),
                AuditTypeId = 4
            };

            auditCtx.Audits.Update(record);
            auditCtx.SaveChanges();

            double balance = CoinsHelper.GetTotalCoins(ctx);
            if (balance <= 0.00)
            {
                TempData["msg"] = "You do not have more coins left. Please, enter more to keep playing.";
                ctx.Response.Cookies.Delete("player-name");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Game");
            }

        }


        [HttpGet]
        public IActionResult Win()
        {
            var ctx = HttpContext;

            double winGame = CoinsHelper.GetCurrentBet(ctx) - CoinsHelper.GetOriginalBet(ctx) ;

            Audit record = new Audit();

                record.Name = ctx.Request.Cookies["player-name"];
                record.CreatedDate = DateTime.Now;
                record.Amount = winGame;
                record.AuditTypeId = 3;

            auditCtx.Audits.Update(record);
            auditCtx.SaveChanges();

            return RedirectToAction("Index", "Game");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("filter"))
                HttpContext.Session.SetString("filter", "all");
            else
                HttpContext.Session.SetString("filter", HttpContext.Request.Query["query"]);

            string tab = HttpContext.Session.GetString("filter");

            var auditList = new List<Audit>();
            var tabFilter = new AuditHelper(tab);

            if (tabFilter.IsAll)
            {
                auditList = auditCtx.Audits.Include(a => a.auditTypes)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
                AuditHelper.Tab = AuditHelper.TabActived.All;
            }

            else if (tabFilter.IsCashIn)
            {
                auditList = auditCtx.Audits.Include(a => a.auditTypes)
                .Where(a => a.auditTypes.Name == tabFilter.Filter)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
                AuditHelper.Tab = AuditHelper.TabActived.CashIn;
            }

            else if (tabFilter.IsCashOut)
            {
                auditList = auditCtx.Audits.Include(a => a.auditTypes)
                .Where(a => a.auditTypes.Name == tabFilter.Filter)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
                AuditHelper.Tab = AuditHelper.TabActived.CashOut;
            }

            else if (tabFilter.IsWin)
            {
                auditList = auditCtx.Audits.Include(a => a.auditTypes)
                .Where(a => a.auditTypes.Name == tabFilter.Filter)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
                AuditHelper.Tab = AuditHelper.TabActived.Win;
            }

            else if (tabFilter.IsLose)
            {
                auditList = auditCtx.Audits.Include(a => a.auditTypes)
                .Where(a => a.auditTypes.Name == tabFilter.Filter)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
                AuditHelper.Tab = AuditHelper.TabActived.Lose;
            }

            List<string> Tab = AuditHelper.GetActiveTab();

            ViewBag.All = Tab[0];
            ViewBag.Win = Tab[1];
            ViewBag.Lose = Tab[2];
            ViewBag.CashOut = Tab[3];
            ViewBag.CashIn = Tab[4];
            
            return View(auditList);
        }




    }
}
