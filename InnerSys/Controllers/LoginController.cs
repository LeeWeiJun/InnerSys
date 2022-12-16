using InnerSys.Db;
using System;
using System.Collections.Generic;
using InnerSys.Views.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InnerSys.Controllers.Service;

namespace InnerSys.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private static readonly OrderEntities orderdb = new OrderEntities();
        private readonly LoginService loginService = new LoginService();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var Dbacc = orderdb.Employee.Where(w => w.Account.Equals(model.UserAccount) && w.Password.Equals(model.Password)).Select(s => new { pwd = s.Password, level = s.PreLevel }).FirstOrDefault();
            if (Dbacc == null)
            {
                TempData["msg"] = "Login Fail!!";
                return View();
            }

            loginService.setSession(model);
            return RedirectToAction("Order", "Order");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();//清除session
            return RedirectToAction("Login", "Login");
        }
    }
}