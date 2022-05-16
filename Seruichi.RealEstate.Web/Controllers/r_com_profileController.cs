using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_com_profile;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_com_profile;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_com_profileController : BaseController
    {
        // GET: r_com_profile
        public ActionResult Index()
        {
            r_com_profileModel model = new r_com_profileModel();
            model = getLogin_Info(model);
            return View(model);
        }

        public r_com_profileModel getLogin_Info(r_com_profileModel model)
        {
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.PermissionChat = Convert.ToByte(base.GetOperator("PermissionChat"));
            model.PermissionSetting = Convert.ToByte(base.GetOperator("PermissionSetting"));
            model.PermissionPlan = Convert.ToByte(base.GetOperator("PermissionPlan"));
            model.PermissionInvoice = Convert.ToByte(base.GetOperator("PermissionInvoice"));
            return model;
        }

        public ActionResult get_r_com_profileData(r_com_profileModel model)
        {
            r_com_profileBL bl = new r_com_profileBL();
            var dt = bl.get_r_com_profileData(model);
            return OKResult(DataTableToJSON(dt));
        }

        public ActionResult update_r_com_profileData(r_com_profileModel model)
        {
            r_com_profileBL bl = new r_com_profileBL();
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.IPAddress = base.GetClientIP();
            return OKResult(bl.update_r_com_profileData(model));
        }

        public ActionResult Insert_l_log(r_com_profileModel model)
        {
            r_com_profileBL bl = new r_com_profileBL();
            model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            model.PageID = model.PageID;
            model.ProcessKBN = model.ProcessKBN;
            model.Remarks = model.Remarks;
            bl.Insertr_com_profile_L_Log(model);
            return OKResult();
        }
    }
}