using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_plusController : BaseController
    {
        // GET: a_mypage_plus
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }

            return View();
        }
    }
}