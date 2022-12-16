using InnerSys.Db;
using InnerSys.Filters;
using InnerSys.Session;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InnerSys.Controllers
{
    [LoginAuthorize]
    public class CheckController : Controller
    {
        private static readonly OrderEntities orderdb = new OrderEntities();

        // GET: Check
        public ActionResult Check()
        {
            var Desk = orderdb.OrderNum.Where(w => w.IsCheck == false).GroupBy(g => g.DeskNum).Select(s => new
            {
                DeskNum = s.Select(ss => ss.DeskNum).FirstOrDefault()
            }).ToList();
            ViewData["CheckDeskLists"] = new SelectList(Desk, "DeskNum", "DeskNum");
            return View("Check");
        }

        public ActionResult GetCheck()
        {
            if (MySession.Desk == "")
            {
                TempData["msg"] = "請選擇需結帳之桌號!!!";
            }
            else
            {
                var DeskNum = Convert.ToInt32(MySession.Desk);
                orderdb.Database.ExecuteSqlCommand($"UPDATE OrderNum SET IsCheck = 1 Where DeskNum = {DeskNum}");
                orderdb.SaveChanges();
            }
            return Check();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetMenu(int? Desk)
        {
            var li = (from a in orderdb.OrderDetail
                     join b in orderdb.OrderNum on a.OrderId equals b.OrderId
                     join c in orderdb.Menu on a.PotId equals c.PotId
                     where b.DeskNum == Desk && b.IsCheck == false
                     select new { Id = c.PotId , 品項 = c.Kind , 價格 = c.Dollar , 鍋數 = a.PotCon , 小計 = a.PotCon * c.Dollar}).ToList();

            li.Add(new { Id = 999, 品項 = "總計", 價格 = 0, 鍋數 = li.Sum(s => s.鍋數), 小計 = li.Sum(s =>s.小計) });
            MySession.Desk = Convert.ToString(Desk);
            return Json(new { data = li });
        }
    }
}