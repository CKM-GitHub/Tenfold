using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web
{
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        public bool IsRedirectedToLoginPage { get; set; } = true;

        public bool Enabled { get; private set; }

        public SessionFilterAttribute()
        {
            Enabled = true;
        }

        public SessionFilterAttribute(bool enable)
        {
            Enabled = enable;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Enabled)
            {
                if (HttpContext.Current.Session["UserInfo"] == null)
                {
                    if (IsRedirectedToLoginPage)
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        filterContext.Result = new RedirectResult("~/User/Login");
                    }
                    else
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}