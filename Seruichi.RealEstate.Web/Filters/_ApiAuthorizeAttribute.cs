using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Seruichi.Seller.Web
{
    public class ApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;

            if (auth == null || auth.Scheme.ToLower() != "basic" || String.IsNullOrEmpty(auth.Parameter))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var authToken = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(auth.Parameter));
                var arrAutoToken = authToken.Split(':');

                if (!IsAuthorizedUser(arrAutoToken[0], arrAutoToken[1]))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            base.OnAuthorization(actionContext);
        }

        private bool IsAuthorizedUser(string user, string password)
        {
            return user.Equals("ogUzkq=EopiYA,U33yzf") && password.Equals("e>gW0BXP85@7-#*~k1@a");
        }   

    }
}