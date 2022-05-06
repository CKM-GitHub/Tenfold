using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seruichi.BL;
using Seruichi.Common;
using System.Web.Mvc;
using Models;
using Seruichi.BL.Seller;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_plusController : BaseController
    {
        // GET: a_mypage_plus
        [AllowAnonymous]
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }
            a_mypage_plusBL bl = new a_mypage_plusBL();
            a_mypage_plusModel model = bl.Get_Possible_Time(user.UserID);
            ViewBag.PossibleTime = model.Possible_Time;
            return View();
        }
        [HttpPost]
        public ActionResult Get_Calculate_extra_charge(a_mypage_plusModel model)
        {
            a_mypage_plusBL bl = new a_mypage_plusBL();
            var dt = bl.Get_Calculate_extra_charge(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult Insert_D_SellerPossible_OK(a_mypage_plusModel model)
        {

            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            a_mypage_plusBL bl = new a_mypage_plusBL();
            if (!bl.Insert_D_SellerPossible_OK(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
        [HttpPost]
        public ActionResult Insert_D_SellerPossible_NG(a_mypage_plusModel model)
        {

            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            a_mypage_plusBL bl = new a_mypage_plusBL();
            if (!bl.Insert_D_SellerPossible_NG(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
    }
}