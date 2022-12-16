using InnerSys.Db;
using InnerSys.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InnerSys.Session;

namespace InnerSys.Controllers.Service
{
    public class LoginService
    {
        public void setSession(LoginViewModel model)
        {
            using (OrderEntities orderdb = new OrderEntities())
            {
                MySession.PreLevel = Convert.ToString(orderdb.Employee.Where(w => w.Account.Equals(model.UserAccount)).Select(s => s.PreLevel).FirstOrDefault());
            }

            MySession.Account = model.UserAccount;
        }
    }
}