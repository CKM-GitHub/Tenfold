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
        public ActionResult Index()
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
            if (!bl.checkRealECD(model.RealECD, out errorcd))
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

            r_loginBL bl = new r_loginBL();
            if (!bl.checkREStaffCD(model.RealECD, model.REStaffCD, out errorcd, out rePassword))
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
            if (!bl.checkREStaffCD(model.RealECD, model.REStaffCD, out errorcd, out rePassword))
            {
                return ErrorMessageResult(errorcd);
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
            if (!bl.checkRealECD(model.RealECD, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            if (!bl.checkREStaffCD(model.RealECD, model.REStaffCD, out errorcd, out rePassword))
            {
                return ErrorMessageResult(errorcd);
            }
            else
            {
                if (model.REPassword == rePassword)
                {
                    FormsAuthentication.SetAuthCookie(model.REStaffCD, false);
                    SessionAuthenticationHelper.CreateLoginUser(model.REStaffCD);
                    r_login_l_log_Model model1 = Getlogdata(model);
                    bl.Insert_L_Login(model1);
                    return OKResult();
                }
                else
                {
                    return ErrorMessageResult("E109");
                }
            }
        }
        public r_login_l_log_Model Getlogdata(r_loginModel model1)
        {
            r_loginBL bl = new r_loginBL();
            r_login_l_log_Model model=new r_login_l_log_Model();
            model.LoginKBN = 2;
            model.LoginID = model1.REStaffCD;
            model.RealECD = model1.RealECD;
            model.LoginName = bl.GetREStaffNamebyREStaffCD(model1.REStaffCD);
            model.IPAddress = base.GetClientIP(); 
            model.PageID = "r_login";
            model.ProcessKBN = "Insert";
            model.Remarks = model.LoginName;
            return model;
        }
    }
}