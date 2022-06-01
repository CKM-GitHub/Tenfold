using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.RealEstate.r_staff;
using Models.RealEstate.r_login;
using Models.RealEstate.r_staff;
using System.Data;
using Seruichi.BL;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_staffController : BaseController
    {
        // GET: r_staff
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            r_staffBL bl = new r_staffBL();
            List<r_staffModel> StaffList = new List<r_staffModel>();
            DataTable dt = bl.Get_M_REStaff_By_RealECD_IsAdmin(user.RealECD,user.REStaffCD);
            StaffList = (from DataRow dr in dt.Rows
                                select new r_staffModel()
                                {
                                    RealECD   = dr["RealECD"].ToString(),
                                    REFaceImage = dr["REFaceImage"].ToString(),
                                    REStaffCD = dr["REStaffCD"].ToString(),
                                    REStaffName = dr["REStaffName"].ToString(),
                                    REIntroduction = dr["REIntroduction"].ToString(),
                                    REPassword = dr["REPassword"].ToString(),
                                    PermissionChat = dr["PermissionChat"].ToString(),
                                    PermissionSetting = dr["PermissionSetting"].ToString(),
                                    PermissionPlan = dr["PermissionPlan"].ToString(),
                                    PermissionInvoice = dr["PermissionInvoice"].ToString()
                                }).ToList();

            ViewBag.StaffInfo = StaffList;
            ViewBag.IsAdmin = user.REStaffCD;
            return View();
        }
        [HttpPost]
        public ActionResult Get_select_M_REStaff(r_staffModel model)
        {
            string errorcd = "";
            model.RealECD = base.GetOperator("RealECD");
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            if(!bl.Get_select_M_REStaff(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
                
            }
            return OKResult();
        }

        [HttpPost]
        public ActionResult Save_M_REStaff(r_staffModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            model = Getlogdata(model);

            if (!bl.Save_M_REStaff(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }

        public r_staffModel Getlogdata(r_staffModel model)
        {
            CommonBL bl = new CommonBL();
            //model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            //model.PageID = "r_staff";
            //model.ProcessKBN = "INSERT/UPDATE";
            //model.Remarks = "INS=" + " " + model.REStaffCD + " " + model.MansionName;
            return model;
        }
    }
}