using InnerSys.Db;
using InnerSys.Views.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace InnerSys.Controllers.Tests
{
    [TestClass()]
    public class LoginControllerTests
    {
        private static readonly OrderEntities orderdb = new OrderEntities();

        [TestMethod()]
        public void Login_輸入正確參數_回傳正確()
        {
            LoginViewModel viewModel = new LoginViewModel
            {
                UserAccount = "0001",
                Password = "0001"
            };
            var Dbacc = orderdb.Employee.Where(w => w.Account.Equals(viewModel.UserAccount) && w.Password.Equals(viewModel.Password)).Select(s => new { pwd = s.Password, level = s.PreLevel }).FirstOrDefault();
            Assert.IsNotNull(Dbacc);    
        }

        [TestMethod()]
        public void Login_輸入錯誤密碼_回傳密碼錯誤()
        {
            LoginViewModel viewModel = new LoginViewModel
            {
                UserAccount = "0001",
                Password = "1234"
            };
            var Dbacc = orderdb.Employee.Where(w => w.Account.Equals(viewModel.UserAccount) && w.Password.Equals(viewModel.Password)).Select(s => new { pwd = s.Password, level = s.PreLevel }).FirstOrDefault() == null ? true : false;
            Assert.IsFalse(Dbacc,"PWD ERROR");
        }

        [TestMethod()]
        public void Login_輸入錯誤帳號_回傳帳號錯誤()
        {
            LoginViewModel viewModel = new LoginViewModel
            {
                UserAccount = "00011",
                Password = "0001"
            };
            var Dbacc = orderdb.Employee.Where(w => w.Account.Equals(viewModel.UserAccount) && w.Password.Equals(viewModel.Password)).Select(s => new { pwd = s.Password, level = s.PreLevel }).FirstOrDefault() == null ? true : false;
            Assert.IsFalse(Dbacc, "ACC ERROR");
        }
    }
}