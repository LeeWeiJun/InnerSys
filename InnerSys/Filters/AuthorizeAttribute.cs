using InnerSys.Db;
using InnerSys.Session;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Linq;
using System.Linq;
using System.Web.Mvc;

namespace InnerSys.Filters
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                var url = filterContext.HttpContext.Request.Url.ToString().Replace("http:", "https:");
                filterContext.Result = new RedirectResult(url);
            }

            //判斷是否跳過授權過濾器
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            using(OrderEntities orderdb = new OrderEntities())
            {
                string userName = MySession.Account;
                int acccon = orderdb.Employee.Where(w => w.Account.Equals(userName)).Count();
                var actionName = filterContext.ActionDescriptor.ActionName;
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                int funcCon = orderdb.Menus.Where(w => w.Controller.Equals(controllerName) && w.Action.Equals(actionName)).Count();
            //判斷登入情況
            if (acccon < 1 || funcCon < 1)
                {
                    filterContext.Result = new RedirectResult("~/Login/Login");
                    return;
                }
                else
                {
                    SiteMaps.ReleaseSiteMap(); //Mvc.sitemap 清cache
                }
            }
        }
    }
}