using Models;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Seruichi.RealState.Web
{
    public class SessionAuthenticationAttribute : FilterAttribute, IActionFilter
    {
        public bool Enabled { get; set; } = true;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Enabled) return;

            var request = filterContext.HttpContext.Request;
            var user = SessionAuthenticationHelper.GetUserFromSession();

            if (user == null)
            {
                SetUnauthorized(request, filterContext);
                return;
            }

            FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
            if (identity != null)
            {
                FormsAuthenticationTicket ticket = identity.Ticket;
                if (ticket.Expired)
                {
                    SetUnauthorized(request, filterContext);
                    return;
                }
                if (ticket.Name != user.UserID)
                {
                    SetUnauthorized(request, filterContext);
                    return;
                }
            }
            else
            {
                if (request.HttpMethod == WebRequestMethods.Http.Post)
                {
                    if (request.IsAjaxRequest())
                    {
                        if (user.VerificationToken != request.Headers["RequestVerificationToken"])
                        {
                            SetUnauthorized(request, filterContext);
                            return;
                        }
                    }
                    else
                    {
                        if (user.VerificationToken != request.Form["RequestVerificationToken"])
                        {
                            SetUnauthorized(request, filterContext);
                            return;
                        }
                    }
                }
            }
        }

        private void SetUnauthorized(HttpRequestBase request, ActionExecutingContext filterContext)
        {
            if (request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                filterContext.Result = new RedirectResult("~/a_index/Index");
            }
        }
    }
}