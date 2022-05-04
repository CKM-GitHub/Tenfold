using Seruichi.BL.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Seller;
using Seruichi.BL;
using Seruichi.Common;
using Models;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_ahisController : BaseController
    {
        // GET: a_mypage_ahis
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
        public ActionResult GetD_AssReqProgressList(a_mypage_ahisModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            a_mypage_ahisBL bl = new a_mypage_ahisBL();
            var dt = bl.GetD_AssReqProgressList(model);
            return OKResult(DataTableToJSON(dt));
        }
        public string GEtdata(a_mypage_ahisModel model)
        {
            a_mypage_ahisBL bl = new a_mypage_ahisBL();
            var dt = bl.GetD_AssReqProgressList(model);
            return (DataTableToJSON(dt));
        }
    }
}