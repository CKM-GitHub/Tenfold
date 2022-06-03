using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Seruichi.Common;
using Models;
using Seruichi.BL.Seller;
using System.Data;


namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_taikaiController : BaseController
    {
        // GET: a_mypage_taikai
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