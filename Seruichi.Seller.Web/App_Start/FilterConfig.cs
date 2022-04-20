using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new AuthorizeAttribute());
            filters.Add(new SessionAuthenticationAttribute());
            filters.Add(new BrowsingHistoryAttribute());
            filters.Add(new CustomHandleErrorAttribute());
        }

        public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new System.Web.Http.AuthorizeAttribute());
            filters.Add(new ApiExceptionAttribute());
        }
    }
}
