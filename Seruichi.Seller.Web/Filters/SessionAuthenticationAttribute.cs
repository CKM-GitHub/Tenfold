using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web
{
    public class SessionAuthenticationAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //user id
            var request = filterContext.HttpContext.Request;

            bool allowAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                                    || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            var user = SessionAuthenticationHelper.GetUserFromSession();

            if (!allowAnonymous)
            {
                if (!SessionAuthenticationHelper.ValidateUser(user))
                {
                    SetUnauthorized(request, filterContext);
                    return;
                }
            }

            //verificationToken
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                bool ignoreVerificationToken = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreVerificationTokenAttribute), true).Any()
                                    || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IgnoreVerificationTokenAttribute), true).Any();

                if (!ignoreVerificationToken)
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
                filterContext.Result = new RedirectResult("~/Error/Unauthorized");
            }
        }
    }
}