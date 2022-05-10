using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Models;
using Models.RealEstate.r_login;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_login;
using Seruichi.Common;

namespace Seruichi.RealEstate.Web.Controllers
{
    [AllowAnonymous]
    public class r_loginController : BaseController
    {
        // GET: r_login
        public ActionResult Login()
        {
            Session.Clear();
            SessionAuthenticationHelper.CreateAnonymousUser();
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult checkRealECD(r_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            if (!validator.CheckNullOrEmpty("RealECD", model.RealECD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            r_loginBL bl = new r_loginBL();
            if (!bl.checkRealECD(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult checkREStaffCD(r_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            string rePassword = "";
            if (!validator.CheckNullOrEmpty("RealECD", model.RealECD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!validator.CheckNullOrEmpty("REStaffCD", model.REStaffCD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            r_loginBL bl = new r_loginBL();
            if (!bl.checkREStaffCD(model, out errorcd, out rePassword))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult checkREPassword(r_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            string rePassword = "";
            if (!validator.CheckNullOrEmpty("RealECD", model.RealECD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!validator.CheckNullOrEmpty("REStaffCD", model.REStaffCD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            r_loginBL bl = new r_loginBL();
            if (!bl.checkREStaffCD(model, out errorcd, out rePassword))
            {
                return ErrorMessageResult("E109");
            }
            else
            {
                if(model.REPassword== rePassword)
                {

                }
                else
                {
                    return ErrorMessageResult("E109");
                }
            }
            return OKResult();
        }
        [HttpPost]
        public ActionResult select_M_RealEstateStaff(r_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            Validator validator = new Validator();
            string errorcd = "";
            string rePassword = "";
            if (!validator.CheckNullOrEmpty("RealECD", model.RealECD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!validator.CheckNullOrEmpty("REStaffCD", model.REStaffCD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!validator.CheckNullOrEmpty("REPassword", model.REPassword, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            r_loginBL bl = new r_loginBL();
            if (!bl.checkRealECD(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!bl.checkREStaffCD(model, out errorcd, out rePassword))
            {
                return ErrorMessageResult(errorcd);
            }
            else
            {
                if (model.REPassword == rePassword)
                {
                    model.REStaffName = bl.GetREStaffNamebyREStaffCD(model.REStaffCD);
                    //FormsAuthentication.SetAuthCookie(model.REStaffCD, false);

                    SessionAuthenticationHelper.CreateLoginUser(model);                    
                    bl.Insert_L_Login(model, base.GetClientIP());
                    return OKResult(new { RealECD = model.RealECD, REStaffCD = model.REStaffCD, PermissionChat = model.PermissionChat, PermissionSetting= model.PermissionSetting, PermissionPlan= model.PermissionPlan, PermissionInvoice= model.PermissionInvoice });
                }
                else
                {
                    return ErrorMessageResult("E109");
                }
            }
        }

        public ActionResult Logout()
        {
            //Session.Clear();
            SessionAuthenticationHelper.Logout();
            return RedirectToAction("Login", "r_login");
        }
    }
}