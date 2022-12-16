using System.Web;
using System.Web.Mvc;

namespace InnerSys.Session
{
    public class MySession
    {
        public static string Account
        {
            get
            {
                return HttpContext.Current.Session["Account"] as string;
            }
            set
            {
                HttpContext.Current.Session["Account"] = value;
            }
        }

        public static string PreLevel
        {
            get
            {
                return HttpContext.Current.Session["PreLevel"] as string;
            }
            set
            {
                HttpContext.Current.Session["PreLevel"] = value;
            }
        }

        public static string Desk
        {
            get
            {
                return HttpContext.Current.Session["Desk"] as string;
            }
            set
            {
                HttpContext.Current.Session["Desk"] = value;
            }
        }
    }
}