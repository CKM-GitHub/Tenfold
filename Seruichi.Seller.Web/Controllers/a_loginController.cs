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
        public ActionResult SendTemporaryRegistrationMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userID, string password)
        {
            a_loginBL bl = new a_loginBL();
            var validationResult = bl.ValidateLogin(userID, password);
            if (validationResult.Count > 0)
            {
                return Json(new { isOK = false, data = validationResult });
            }

            FormsAuthentication.SetAuthCookie(userID, false);
            SessionAuthenticationHelper.CreateLoginUser(userID);
            return Json(new { isOK = true });
        }

        [HttpPost]
        public ActionResult LoginDone(string userID, string password)
        {
            return RedirectToAction("Index", "a_index");
        }
    }
}