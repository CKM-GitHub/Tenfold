using Models;
using Seruichi.BL;
using Seruichi.Common;
using System;
using System.Web.Mvc;
//using System.Web.Security;

namespace Seruichi.Seller.Web.Controllers
{
    [AllowAnonymous]
    public class a_loginController : BaseController
    {
        // GET: a_login
        [HttpGet]
        public ActionResult Index()
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    GotoTopPage();
            //}
            Session.Clear();
            SessionAuthenticationHelper.CreateAnonymousUser();
            return View();
        }

        // GET: Logout
        [HttpGet]
        public ActionResult Logout()
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    FormsAuthentication.SignOut();
            //}
            SessionAuthenticationHelper.Logout();
            return RedirectToAction("Index", "a_login");
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(string mailAddress, string password)
        {
            a_loginBL bl = new a_loginBL();
            var validationResult = bl.ValidateLogin(mailAddress, password, out LoginUser user);
            if (validationResult.Count > 0)
            {
                ViewBag.ValidationResult = base.ConvertToJson(validationResult);
                return View("Index");
            }
            //FormsAuthentication.SetAuthCookie(user.UserID, false);
            SessionAuthenticationHelper.CreateLoginUser(user);

            bl.InsertLoginLog(user, base.GetClientIP());

            return GotoTopPage();
        }

        // Ajax: TemporaryRegistration
        [HttpPost]
        public ActionResult TemporaryRegistration(string mailAddress)
        {
            a_loginBL bl = new a_loginBL();

            var validationResult = bl.ValidateTemporaryRegistration(mailAddress);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.InsertCertificationData(mailAddress, out string certificationCD, out DateTime effectiveDateTime))
            {
                return ErrorResult();
            }

            SendMail.SendSmtp(bl.GetTemporaryRegistrationMailInfo(mailAddress, certificationCD, effectiveDateTime));

            return OKResult();
        }

        // Ajax: VerifyMailAddress
        [HttpPost]
        public ActionResult VerifyMailAddress(string sellerKana, string birthday, string handyphone)
        {
            a_loginBL bl = new a_loginBL();

            var validationResult = bl.ValidateVerifyMailAddress(sellerKana, birthday, handyphone,
                out string mailAddress);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult(mailAddress);
        }

        // Ajax: ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(string sellerKana, string birthday, string mailAddress)
        {
            a_loginBL bl = new a_loginBL();

            var validationResult = bl.ValidateResetPassword(sellerKana, birthday, mailAddress,
                out string sellerCD, out string sellerName);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            if (!bl.UpdatePassword(sellerCD, sellerName, base.GetClientIP(), mailAddress, out string newPassword))
            {
                return ErrorResult();
            }

            SendMail.SendSmtp(bl.GetResetPasswordMailInfo(mailAddress, sellerName, newPassword));

            return OKResult();
        }

        private ActionResult GotoTopPage()
        {
            return RedirectToAction("Index", "a_index");
        }
    }
}