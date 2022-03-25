using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Seruichi.Seller.Web.Controllers
{
    [BrowsingHistory(false)]
    //[SessionFilter(false)]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Raise(int? httpCode)
        {
            if (!httpCode.HasValue)
            {
                throw new ApplicationException("エラーテストです。");
            }
            else
            {
                //return new HttpStatusCodeResult((int)httpCode);
                throw new HttpException(
                    httpCode.Value, string.Format("ステータスコード {0} のエラーテストです。", httpCode));
            }
        }
    }
}