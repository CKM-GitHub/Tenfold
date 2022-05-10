using Seruichi.Common;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public bool Enabled { get; set; } = true;

        public CustomHandleErrorAttribute()
        {
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var ex = filterContext.Exception;

            int statusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 0;
            if (statusCode != (int)HttpStatusCode.BadRequest &&
                statusCode != (int)HttpStatusCode.Unauthorized &&
                statusCode != (int)HttpStatusCode.Forbidden &&
                statusCode != (int)HttpStatusCode.NotFound)
            {
                Logger.GetInstance().Error(filterContext.Exception);
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //Application_Errorは呼ばれない
                HandleAjaxRequestException(filterContext, statusCode);
            }
            else
            {
                //custom errorが有効ならApplication_Errorは呼ばれない
                base.OnException(filterContext);
            }
        }

        private void HandleAjaxRequestException(ExceptionContext filterContext, int statusCode)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode == 0 ? (int)HttpStatusCode.InternalServerError : statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    Message = filterContext.Exception.ToString(),
                    Message2 = filterContext.ToString()
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}