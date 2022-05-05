using Models;
using Seruichi.BL;
using Seruichi.Common;
using System;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_whisController : BaseController
    {
        // GET: a_mypage_whis
        [AllowAnonymous]
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }

            a_mypage_whisBL bl = new a_mypage_whisBL();
            a_mypage_whisModel model = bl.GetSellerData(user.UserID);

            // CommonBL cmmbl = new CommonBL();
           return View(model);
        }

       

        // POST: GotoNextPage
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_mypage_whis");
        }
    }
}