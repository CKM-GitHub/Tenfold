using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_auto_mes;
using Seruichi.BL.RealEstate.r_template;
using Models.RealEstate.r_auto_mes;
using Models.RealEstate.r_template;
using System.Data;
using System.Threading.Tasks;
using Seruichi.BL;
using Seruichi.Common;
namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_templateController : BaseController
    {
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            //var queryString = GetFromQueryString<r_templateModel>();
            //if (queryString.permission_setting != null && !String.IsNullOrEmpty(queryString.permission_setting))
            //    Session["permission_setting"] = queryString.permission_setting;

            //if (String.IsNullOrEmpty(queryString.permission_setting) || Session["AssReqID"] == null)
            //    Session["permission_setting"] = "1";
            ViewBag.permission_setting = user.PermissionSetting.ToByte(0);// Session["permission_setting"].ToString();
            return View(); 
        }
        [HttpPost]
        public ActionResult Get_templateData(r_templateModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_templateBL bl = new r_templateBL();
            //model.RealECD = user.RealECD;
            //model.LoginID = base.GetOperator("UserID");
            //model.LoginName = base.GetOperator("UserName");
            //model.IPAddress = base.GetClientIP(); 
            var ds = bl.Get_templateData();
            var json = DataTableToJSON(ds.Tables[0]) + "Ʈ" + DataTableToJSON(ds.Tables[1]) + "Ʈ" + DataTableToJSON(ds.Tables[2])+ "Ʈ" + DataTableToJSON(ds.Tables[3]);
            return OKResult(json);
        }

        [HttpPost]
        public ActionResult DeleteData(r_templateModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            model.RealECD = user.RealECD;
            model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            model.Page = "r_template";
            model.Processing = "Delete";
            model.Remarks = "TemplateNo： " + model.TemplateNo;
            r_templateBL bl = new r_templateBL();
            if (user.PermissionSetting.ToByte(0).ToString() == "1")
            {
                bl.Delete_R_templateDate(model);
                bl.Insert_L_Log(model);
            }
            return OKResult();
        }
         
    }
}