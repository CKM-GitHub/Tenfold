using Models;
using Seruichi.BL;
using Seruichi.Common;
using System;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_uinfoController : BaseController
    {
        // GET: CheckRedirect
        [AllowAnonymous]
        [HttpGet]
        public ActionResult CheckRedirect()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }

            return RedirectToAction("Index", "a_mypage_uinfo");
        }

        // GET: a_mypage_uinfo
        [HttpGet]
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            a_mypage_uinfoModel model = bl.GetSellerData(user.UserID);

            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfAllPrefecture();
            return View(model);
        }

        // POST: GotoLogin
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GotoLogin()
        {
            return RedirectToAction("Index", "a_login");
        }

        // POST: GotoNextPage
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_mypage_uinfo");
        }




        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(string zipCode1, string zipCode2)
        {
            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            if (!bl.CheckZipCode(zipCode1, zipCode2, out string errorcd, out string prefCD, out string cityName, out string townName))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult(new { prefCD, cityName, townName });
        }

        // Ajax: CheckAll
        [HttpPost]
        public ActionResult CheckAll(a_mypage_uinfoModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        // Ajax: InsertSellerData
        [HttpPost]
        public ActionResult UpdateSellerData(a_mypage_uinfoModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.SellerCD = base.GetOperator();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.UpdateSellerData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }




        // Ajax: ChangeMailAddress
        [HttpPost]
        public ActionResult ChangeMailAddress(a_mypage_uinfo_emailModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.SellerCD = base.GetOperator();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();

            var validationResult = bl.ValidateChangeMailAddress(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.InsertCertificationData(model, out string certificationCD, out DateTime effectiveDateTime))
            {
                return ErrorResult();
            }

            SendMail.SendSmtp(bl.GetChangeMailAddressMailInfo(model.NewMailAddress, certificationCD, effectiveDateTime));

            return OKResult();
        }




        // Ajax: ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(a_mypage_uinfo_passwordModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.SellerCD = base.GetOperator();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();

            a_mypage_uinfoBL bl = new a_mypage_uinfoBL();

            var validationResult = bl.ValidateChangePassword(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.UpdatePassword(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            //ログアウト
            SessionAuthenticationHelper.ChangeToAnonymousUser();
            return OKResult();
        }
    }
}