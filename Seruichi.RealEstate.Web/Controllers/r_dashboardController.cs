using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_dashboardController : Controller
    {
        static string strRealECD = string.Empty;
        static string strREStaffCD = string.Empty;
        static string strPermissionChat = string.Empty;
        static string strPermissionSetting = string.Empty;
        static string strPermissionPlan = string.Empty;
        static string strPermissionInvoice = string.Empty;

        // GET: r_dashboard
        public ActionResult Index(string RealECD,string REStaffCD,string PermissionChat, string PermissionSetting, string PermissionPlan, string PermissionInvoice)
        {
            strRealECD = RealECD;strRealECD = REStaffCD;
            strPermissionChat = PermissionChat;
            strPermissionSetting = PermissionSetting;
            strPermissionPlan = PermissionPlan;
            strPermissionInvoice = PermissionInvoice;
                return View();
        }


    }
}