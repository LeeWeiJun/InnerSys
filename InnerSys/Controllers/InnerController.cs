using InnerSys.Db;
using InnerSys.Filters;
using InnerSys.Views.ViewModels;
using MvcSiteMapProvider.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace InnerSys.Controllers
{
    [LoginAuthorize]
    public class InnerController : Controller
    {
        private static readonly OrderEntities orderdb = new OrderEntities();

        // GET: Inner
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetEmployee()
        {
            var li = from E in orderdb.Employee   
                     join EP in orderdb.EmployeePremission on E.PreLevel equals EP.PreLevel   
                     where E.IsExit == false
                     select new { E.Id , E.Account , E.Password , EP.PreDescription };

            return Json(new { data = li });
        }

        public ActionResult Delete(int Id)
        {
            var model = orderdb.Employee.SingleOrDefault(s => s.Id == Id);
            if (model != null)
            {
                model.IsExit = true;
                orderdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var model = (from E in orderdb.Employee
                     join EP in orderdb.EmployeePremission on E.PreLevel equals EP.PreLevel
                     where E.IsExit == false & E.Id == id
                     select new EditViewModel { Id = E.Id, Account = E.Account, Password = E.Password, PreDescription = EP.PreDescription }).Single();
            var PreDescription = orderdb.EmployeePremission.Where(w => w.PreLevel != 1).Select(s => new { s.PreDescription }).ToList();
            ViewData["CheckPreList"] = new SelectList(PreDescription, "PreDescription", "PreDescription");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Employee model , string CheckPre)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var tmp = orderdb.Employee.SingleOrDefault(s => s.Id == model.Id);
            if (tmp != null)
            {
                if(!tmp.Password.Equals(model.Password))
                    tmp.Password = model.Password;
                if(!tmp.EmployeePremission.PreDescription.Equals(CheckPre) &&　CheckPre != "")
                {
                    var Lev = orderdb.EmployeePremission.Where(s => s.PreDescription == CheckPre).Select(s => s.PreLevel).Single();
                    tmp.PreLevel = Lev;
                }
                orderdb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Update()
        {
            var Acc = orderdb.Employee.Max(s => s.Account);
            var AccLen = Acc.Length;
            Acc = Convert.ToString(Convert.ToInt32(Acc) + 1);
            while (Acc.Length < AccLen)
                Acc = "0" + Acc;
            EditViewModel model = new EditViewModel
            {
                Id = orderdb.Employee.Max(m => m.Id) + 1,
                Account = Acc
            };
            var PreDescription = orderdb.EmployeePremission.Where(w => w.PreLevel != 1).Select(s => new { PreDescription = s.PreDescription }).ToList();
            ViewData["CheckPreList"] = new SelectList(PreDescription, "PreDescription", "PreDescription");
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Employee model, string CheckPre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var Lev = orderdb.EmployeePremission.Where(s => s.PreDescription == CheckPre).Select(s => s.PreLevel).Single();
            var li = new List<Employee>
            {
                new Employee { Account = model.Account, Password = model.Password, PreLevel = Lev, IsExit = false }
            };
            orderdb.Employee.AddRange(li);
            orderdb.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: EditMenu
        public ActionResult EditMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditMenu(EditMenuViewModel model)
        {
            foreach (var i in model.Menu)
            {
                if (i.Dollar != orderdb.Menu.Where(w => w.PotId == i.PotId).Select(s => s.Dollar).FirstOrDefault())
                    orderdb.Database.ExecuteSqlCommand($"UPDATE Menu SET Dollar = {i.Dollar} Where PotId = {i.PotId}");
            }
            orderdb.SaveChanges();

            return EditMenu();
        }

        public ActionResult GetMenu()
        {
            var li = from H in orderdb.Menu
                      select new { H.PotId, H.Kind, H.Dollar, H.IsSell };
            return Json(new { data = li });
        }

        public ActionResult ChangeType(int Id , bool IsSell)
        {
            var model = orderdb.Menu.SingleOrDefault(s => s.PotId == Id);
            if (model != null)
            {
                if(IsSell == false)
                    model.IsSell = true;
                else
                    model.IsSell = false;
                orderdb.SaveChanges();
            }
            return RedirectToAction("EditMenu");
        }

        // GET: Total
        public ActionResult Total(DateTime? chartdate)
        {
            DateTime dt = DateTime.Today; //當日起
            if (chartdate != null)
                dt = (DateTime)chartdate;
            ViewBag.Date = dt.ToString("yyyy-MM-dd");
            DateTime endDay = dt.AddDays(1).AddMilliseconds(-1); //當日迄
            DateTime startMonth = dt.AddDays(1 - dt.Day); //月初
            DateTime endMonth = startMonth.AddMonths(1).AddMilliseconds(-1); //月底
            DateTime startQuarter = dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);  //本季度初
            DateTime endQuarter = startQuarter.AddMonths(3).AddMilliseconds(-1);  //本季度末
            DateTime startYear = new DateTime(dt.Year, 1, 1);  //本年年初
            DateTime endYear = new DateTime(dt.Year, 12, 31).AddDays(1).AddMilliseconds(-1);  //本年年末 
            var Data = orderdb.Menu.Select(s => new 
            {
                s.Kind, 
                day = s.OrderDetail.Where(w => w.OrderNum.OrderTime > dt && w.OrderNum.OrderTime < endDay).Sum(ss => (int?)ss.PotCon),
                mon = s.OrderDetail.Where(w => w.OrderNum.OrderTime > startMonth && w.OrderNum.OrderTime < endMonth).Sum(ss => (int?)ss.PotCon),
                qua = s.OrderDetail.Where(w => w.OrderNum.OrderTime > startQuarter && w.OrderNum.OrderTime < endQuarter).Sum(ss => (int?)ss.PotCon),
                year = s.OrderDetail.Where(w => w.OrderNum.OrderTime > startYear && w.OrderNum.OrderTime < endYear).Sum(ss => (int?)ss.PotCon),
                tot = s.OrderDetail.Sum(ss => ss.PotCon) 
            }).AsEnumerable();
            ViewBag.Total = new 
            { 
                label = Data.Select(s => s.Kind), 
                day = Data.Select(s => s.day), 
                mon = Data.Select(s => s.mon), 
                qua = Data.Select(s => s.qua),
                year = Data.Select(s => s.year),
                tot = Data.Select(s => s.tot) 
            };
            return View();
        }
    }
}