using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    [AllowAnonymous]
    public class a_registerController : BaseController
    {
        // GET: a_register
        [HttpGet]
        public ActionResult Index(string mail, string setupid)
        {
            //mail = "sae.kotake@gmail.com";
            //setupid = "WhrUvgYwFHYdPapu";

            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(setupid))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            Session.Clear();
            SessionAuthenticationHelper.CreateAnonymousUser();

            a_registerBL bl = new a_registerBL();
            if (!bl.CheckAndUpdateCertification(mail, setupid, out string errorcd))
            {
                return RedirectToAction("BusinessError", "Error", new { id = errorcd });
            }

            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfPrefecture();
            return View(new a_registerModel() { MailAddress = mail });
        }

        // POST: 
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_login");
        }

        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(string zipCode1, string zipCode2)
        {
            a_registerBL bl = new a_registerBL();
            if (!bl.CheckZipCode(zipCode1, zipCode2, out string errorcd, out string prefCD, out string cityName, out string townName))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult(new { prefCD, cityName, townName });
        }

        // Ajax: CheckAll
        [HttpPost]
        public ActionResult CheckAll(a_registerModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            a_registerBL bl = new a_registerBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        // Ajax: InsertSellerData
        [HttpPost]
        public ActionResult InsertSellerData(a_registerModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_registerBL bl = new a_registerBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.InsertSellerData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
    }
}
