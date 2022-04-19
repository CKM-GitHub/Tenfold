using Seruichi.Common;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealState.Web.Controllers
{
    [CustomHandleError(Enabled = false)]
    [BrowsingHistory(Enabled = false)]
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult BusinessError(string id)
        {
            Response.TrySkipIisCustomErrors = true;
            var message = StaticCache.GetMessage(id);
            ViewBag.ErrorTitle = message.MessageText1;
            ViewBag.ErrorMessage = message.MessageText2;
            ViewBag.ErrorDesctiption = message.MessageText3;
            return View();
        }

        [HttpGet]
        public ActionResult InternalServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [HttpGet]
        public ActionResult BadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [HttpGet]
        public ActionResult Forbidden()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [HttpGet]
        public ActionResult Test(int? httpCode)
        {
            if (httpCode.HasValue)
            {
                throw new HttpException(
                    httpCode.Value, string.Format("ステータスコード {0} のエラーテストです。", httpCode));
            }

            throw new ApplicationException("エラーテストです。");
        }
    }
}