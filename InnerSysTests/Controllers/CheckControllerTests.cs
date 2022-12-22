using InnerSys.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace InnerSys.Controllers.Tests
{
    [TestClass()]
    public class CheckControllerTests
    {
        private static readonly OrderEntities orderdb = new OrderEntities();

        [TestMethod()]
        public void GetMenu_輸入無點餐之桌號_回傳0()
        {
            var Desk = 999;
            var li = (from a in orderdb.OrderDetail
                      join b in orderdb.OrderNum on a.OrderId equals b.OrderId
                      join c in orderdb.Menu on a.PotId equals c.PotId
                      where b.DeskNum == Desk && b.IsCheck == false
                      select new { Id = c.PotId, 品項 = c.Kind, 價格 = c.Dollar, 鍋數 = a.PotCon, 小計 = a.PotCon * c.Dollar }).ToList();
            Assert.AreEqual(li.Count, 0);
        }

        [TestMethod()]
        public void GetMenu_輸入點餐桌號_回傳所點之品項總數()
        {
            var Desk = 10;
            var li = (from a in orderdb.OrderDetail
                      join b in orderdb.OrderNum on a.OrderId equals b.OrderId
                      join c in orderdb.Menu on a.PotId equals c.PotId
                      where b.DeskNum == Desk && b.IsCheck == false
                      select new { Id = c.PotId, 品項 = c.Kind, 價格 = c.Dollar, 鍋數 = a.PotCon, 小計 = a.PotCon * c.Dollar }).ToList();
            Assert.AreEqual(li.Count, 0 ,$"該桌號總數:{li.Count}");
        }
    }
}