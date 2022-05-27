using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Seruichi.Seller.Web
{
    public class ApiValidateVerificationTokenAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool ignoreVerificationToken = actionContext.ActionDescriptor.GetCustomAttributes<IgnoreVerificationTokenAttribute>(true).Any()
                                || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<IgnoreVerificationTokenAttribute>(true).Any();

            if (!ignoreVerificationToken)
            {
                string requestVerificationToken = actionContext.Request.Headers.GetValues("RequestVerificationToken").FirstOrDefault();

                var user = SessionAuthenticationHelper.GetUserFromSession();
                if (user == null)
                {
                    SetUnauthorized(actionContext);
                    return;
                }

                if (user.VerificationToken != requestVerificationToken)
                {
                    SetUnauthorized(actionContext);
                    return;
                }
            }
        }

        private void SetUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

    }
}