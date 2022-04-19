using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());


            ////filters.Add(new AuthorizeAttribute());
            ////filters.Add(new SessionAuthenticationAttribute());
            ////filters.Add(new BrowsingHistoryAttribute());
            ////filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
