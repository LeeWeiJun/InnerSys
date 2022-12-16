using InnerSys.Db;
using InnerSys.Session;
using MvcSiteMapProvider;
using System.Collections.Generic;
using System.Linq;

namespace InnerSys.MvcSiteMapProvider
{
    public class MyDynamicNodeProvider : DynamicNodeProviderBase
    {
         public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            string Acc = MySession.Account;
            using (OrderEntities Orderdb = new OrderEntities())
            {
                List<DynamicNode> returnValue = Orderdb.Employee.Where(w => w.Account == Acc).SelectMany(s => s.EmployeePremission.RolesToMenus.Select(ss => new DynamicNode
                {
                    Title = ss.Menus.Name,
                    Controller = ss.Menus.Controller,
                    Action = ss.Menus.Action
                })).ToList();
                return returnValue;
            }
        }
    }
}
