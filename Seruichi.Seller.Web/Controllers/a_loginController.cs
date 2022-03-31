using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Web.Mvc;
using System.Web.Security;

namespace Seruichi.Seller.Web.Controllers
{
    [SessionAuthentication(Enabled = false)]
    [AllowAnonymous]
    public class a_loginController : Controller
    {
        // GET: a_login
        [HttpGet]
        public ActionResult Index()
        {
            SessionAuthenticationHelper.ReCreateSession();
            return View();
        }

        [HttpPost]
        public ActionResult SendTemporaryRegistrationMail(string mailAddress)
        {
            a_loginBL bl = new a_loginBL();
            if (!bl.CheckMailAddressAlreadyExists(mailAddress, out string errorcd))
            {
                return Json(new { isOK = false, message = StaticCache.GetMessage(errorcd) });
            }
            if (!bl.InsertCertificationData(mailAddress, out errorcd))
            {
                return Json(new { isOK = false, message = StaticCache.GetMessage(errorcd) });
            }

            SendMailInfo mailInfo = bl.GetTemporaryRegistrationMail();
            if (mailInfo != null)
            {
                SendMail.SendSmtpAsync(mailInfo);
            }
            return Json(new { isOK = true });
        }

        [HttpPost]
        public ActionResult ValidateLogin(string mailAddress, string password)
        {
            a_loginBL bl = new a_loginBL();
            var validationResult = bl.ValidateLogin(mailAddress, password, out LoginUser user);
            if (validationResult.Count > 0)
            {
                return Json(new { isOK = false, data = validationResult });
            }

            FormsAuthentication.SetAuthCookie(user.UserID, false);
            SessionAuthenticationHelper.CreateLoginUser(user);
            return Json(new { isOK = true });
        }

        [HttpPost]
        public ActionResult LoginDone()
        {
            return RedirectToAction("Index", "a_index");
        }
    }
}