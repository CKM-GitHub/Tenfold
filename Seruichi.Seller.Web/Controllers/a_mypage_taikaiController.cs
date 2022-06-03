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
            a_mypage_taikaiBL bl = new a_mypage_taikaiBL();
            a_mypage_taikaiModel model = bl.Get_Count(user.UserID);
            ViewBag.PossibleTime = model.Possible_Time;
            return View();
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

        public ActionResult UpdateData(a_mypage_taikaiModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.LoginName = base.GetOperatorName();
            a_mypage_taikaiBL bl = new a_mypage_taikaiBL();
            if(!bl.Update_M_Seller_Data(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();

        }
    }
}