using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seruichi.Common;
using System.Web.Mvc;
using Models.Tenfold.Login;
using System.Text;
using System.Data;
using Seruichi.BL.Tenfold.Login;
using System.Web.Security;

namespace Seruichi.Tenfold.Web.Controllers
{
    [IgnoreVerificationToken]
    [AllowAnonymous]
    public class t_loginController : BaseController
    {
        // GET: t_login
        public ActionResult Login()
        {
            Session.Clear();
            SessionAuthenticationHelper.CreateAnonymousUser();

            return View();
        }
        public ActionResult Logout()
        {
            // Session.Clear();
            SessionAuthenticationHelper.Logout();
            return RedirectToAction("Login", "t_login");
        }
        [HttpPost]
        public ActionResult select_M_TenfoldStaff(t_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            t_loginBL bl = new t_loginBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            DataTable dt = bl.GetM_TenfoldStaff(model);
            if (dt.Rows.Count > 0)
            {
                model.TenStaffName = dt.Rows[0]["TenStaffName"].ToString();
               // FormsAuthentication.SetAuthCookie(model.TenStaffCD, false);
                SessionAuthenticationHelper.CreateLoginUser(model);
                return OKResult(new { UserID = model.TenStaffCD, UserName = model.TenStaffName });
            }
            else
            {
               return ErrorMessageResult("E207");
            }
        }
    }
}