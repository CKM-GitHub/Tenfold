using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.RealEstate.r_staff;
using Models.RealEstate.r_login;
using Models.RealEstate.r_staff;
using System.Data;

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
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            if(!bl.Get_select_M_REStaff(user.RealECD,model.REStaffCD))
            {
                return OKResult();
            }
            else
            {
                return ErrorMessageResult("E314");
            }
        }
    }
}