using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using System.Web.Security;

namespace Seruichi.Seller.Web
{
    public class SessionAuthenticationAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            bool allowAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                                    || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            var user = SessionAuthenticationHelper.GetUserFromSession();
            if (!allowAnonymous && !SessionAuthenticationHelper.ValidateUser(user))
            {
                SetUnauthorized(request, filterContext);
                return;
            }

            //FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
            //if (identity != null && identity.IsAuthenticated)
            //{
            //    FormsAuthenticationTicket ticket = identity.Ticket;
            //    //if (ticket.Expired)
            //    //{
            //    //    SetUnauthorized(request, filterContext);
            //    //    return;
            //    //}
            //    if (ticket.Name != user.UserID)
            //    {
            //        SetUnauthorized(request, filterContext);
            //        return;
            //    }
            //}

            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (user == null)
                {
                    SetUnauthorized(request, filterContext);
                    return;
                }

                string requestVerificationToken = request.IsAjaxRequest() ?
                    request.Headers["RequestVerificationToken"] : request.Form["RequestVerificationToken"];

                if (user.VerificationToken != requestVerificationToken)
                {
                    SetUnauthorized(request, filterContext);
                    return;
                }
            }
        }

        private void SetUnauthorized(HttpRequestBase request, ActionExecutingContext filterContext)
        {
            if (request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            else
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new RedirectResult("~/a_login/Index");
            }
        }
    }
}