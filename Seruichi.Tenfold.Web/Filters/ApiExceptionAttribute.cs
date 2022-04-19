using Seruichi.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Seruichi.Tenfold.Web
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException("actionExecutedContext");
            }

            Exception ex = actionExecutedContext.Exception;

            if (ex is NotImplementedException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }

            var logInfo = new
            {
                HTTPStatus = (actionExecutedContext.Response == null) ? "" : actionExecutedContext.Response.StatusCode.ToString(),
                Request = actionExecutedContext.Request.ToString(),
                Parameter = actionExecutedContext.ActionContext.ActionArguments,
                Response = (actionExecutedContext.Response == null || actionExecutedContext.Response.Content == null) ? ""
                    : actionExecutedContext.Response.Content.ReadAsStringAsync().Result
            };

            Logger.GetInstance().Error(ex, Newtonsoft.Json.JsonConvert.SerializeObject(logInfo));

            base.OnException(actionExecutedContext);
        }
    }
}