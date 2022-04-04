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
    [SessionAuthentication(Enabled = false)]
    [AllowAnonymous]
    public class t_loginController : BaseController
    {
        private Encoding encoding = Encoding.GetEncoding("Shift_JIS");
        // GET: t_login
        public ActionResult Index()
        {
            ////string[] myCookies = Request.Cookies.AllKeys;
            ////foreach (string cookie in myCookies)
            ////{
            ////    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            ////}
            SessionAuthenticationHelper.ReCreateSession();
            return View();
        }


        [HttpPost]
        public ActionResult CheckId(string TenStaffCD)
        {
            if (TenStaffCD == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            if (!validator.CheckIsidAndpsw(TenStaffCD, 10, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
        
        [HttpPost]
        public ActionResult checkPassword(string TenStaffPW)
        {
            if (TenStaffPW == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            if (!validator.CheckIsidAndpsw(TenStaffPW, 10, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
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
                return ErrorMessageResult("E207");
            }

            DataTable dt = bl.GetM_TenfoldStaff(model);
            if (dt.Rows.Count > 0)
            {
                ////HttpCookie cookie = new HttpCookie("TenStaffCD", model.TenStaffCD);
                ////Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(model.TenStaffCD, false);
                SessionAuthenticationHelper.CreateLoginUser(model.TenStaffCD);
                return OKResult();
            }
            else
            {
               return ErrorMessageResult("E207");
            }
        }
    }
}