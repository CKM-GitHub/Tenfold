using Models.RealEstate.r_login;
using Models.RealEstate.r_temp_mes;
using Seruichi.BL.RealEstate.r_temp_mes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_temp_mesController : Controller
    {
        // GET: r_temp_mes
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            r_temp_mesBL bl = new r_temp_mesBL();
            List<r_temp_mesModel> TempMsgList = new List<r_temp_mesModel>();
            DataTable dt = bl.Get_M_REMessage_By_RealECD(user.RealECD);
            TempMsgList = (from DataRow dr in dt.Rows
                         select new r_temp_mesModel()
                         {
                             RealECD = dr["RealECD"].ToString(),
                             SEQ = dr["SEQ"].ToString(),
                             MessageSEQ = dr["MessageSEQ"].ToString(),
                             MessageTitle = dr["MessageTitle"].ToString(),
                             MessageTEXT = dr["MessageTEXT"].ToString()
                         }).ToList();

            ViewBag.TempMsg = TempMsgList;
            ViewBag.IsSetting = user.PermissionSetting;
            return View();
        }
    }
}