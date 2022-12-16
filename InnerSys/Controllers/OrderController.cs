using InnerSys.Db;
using InnerSys.Filters;
using InnerSys.Views.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InnerSys.Controllers
{
    [LoginAuthorize]
    public class OrderController : Controller
    {
        private static readonly OrderEntities orderdb = new OrderEntities();

        // GET: Order
        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderViewModel model)
        {         
            orderdb.OrderNum.Add(new OrderNum { DeskNum = model.Desk, OrderTime = DateTime.Now, IsCheck = false });
            orderdb.SaveChanges();
            var OrderId = orderdb.OrderNum.Max(m => m.OrderId);
            model.OrderDetails.ForEach(f => f.OrderId = OrderId);
            var li2 = model.OrderDetails.Where(w => w.PotCon > 0).ToList();
            orderdb.OrderDetail.AddRange(li2);
            orderdb.SaveChanges();

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetMenu()
        {
            var li = orderdb.Menu.Where(w => w.IsSell == true).Select(s => new
            {
                鍋種 = s.Kind,
                價格 = s.Dollar,
                Id = s.PotId
            }).ToList();

            return Json(new { data = li });
        }
    }
}