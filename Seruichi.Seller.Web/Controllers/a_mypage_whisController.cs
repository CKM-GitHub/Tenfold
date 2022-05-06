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

            
            return View();
        }
        [HttpPost]
        public ActionResult GetD_SellerPossibleData(a_mypage_whisModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            a_mypage_whisBL bl = new a_mypage_whisBL();

            var dt = bl.GetD_SellerPossibleData(user.UserID);

            return OKResult(DataTableToJSON(dt));
        }

    }
}