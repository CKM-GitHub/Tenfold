using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_config;
using Seruichi.BL.Tenfold.t_seller_config;
using Seruichi.BL;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_configController : BaseController
    {
        // GET: t_seller_config
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectFromMultipurpose(t_seller_configModel model)
        {
            t_seller_configBL bl = new t_seller_configBL();
            var dt = bl.SelectFromMultipurpose(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertUpdateToMultipurpose(t_seller_configModel model)
        {
            t_seller_configBL bl = new t_seller_configBL();
            bl.InsertUpdateToMultipurpose(model);
            return OKResult();

        }
        
        [HttpPost]
        public ActionResult InsertL_Log(l_log_Model model)
        {
            t_seller_configBL bl = new t_seller_configBL();
            model = Getlogdata(model);
            bl.Insert_L_Log(model);
            return OKResult();

        }

        public l_log_Model Getlogdata(l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.Page = "t_seller_config";
            model.Processing = "Update";
            model.Remarks = "data update/insert";
            return model;
        }
    }
}