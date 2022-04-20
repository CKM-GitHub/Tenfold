using Models;
using Seruichi.BL;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_uinfoController : BaseController
    {
        // GET: a_mypage_uinfo
        [AllowAnonymous]
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }

            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfPrefecture();
            return View(new a_mypage_uinfoModel() { MailAddress = "" });
        }
    }
}