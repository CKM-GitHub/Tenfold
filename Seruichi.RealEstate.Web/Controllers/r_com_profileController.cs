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
            return View();
        }

        public r_com_profileModel GetLogin_Info()
        {
            r_com_profileModel model = new r_com_profileModel();
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.PermissionChat = Convert.ToByte(base.GetOperator("PermissionChat"));
            model.PermissionSetting = Convert.ToByte(base.GetOperator("PermissionSetting"));
            model.PermissionPlan = Convert.ToByte(base.GetOperator("PermissionPlan"));
            model.PermissionInvoice = Convert.ToByte(base.GetOperator("PermissionInvoice"));
            return model;
        }
    }
}