using Models.Tenfold.t_assess_list;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_assess_list;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_assess_listController : BaseController
    {
        // GET: t_assess_list
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_assess_listModel model)
        {
            t_assess_listBL bl = new t_assess_listBL();
            List<string> chk_lst = new List<string>(); 
            chk_lst.Add(model.Chk_Shinki.ToString());
            chk_lst.Add(model.Chk_Kosho.ToString());
            chk_lst.Add(model.Chk_Seiyaku.ToString());
            chk_lst.Add(model.Chk_Urinushi.ToString());
            chk_lst.Add(model.Chk_Kainushi.ToString());
            chk_lst.Add(model.Chk_Hide.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            var dt = bl.GetM_SellerMansionList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_l_log(t_assess_list_l_log_Model model)
        {
            t_assess_listBL bl = new t_assess_listBL();
            model = Getlogdata(model);
            bl.InsertM_SellerMansion_L_Log(model);
            return OKResult();
        }

        public t_assess_list_l_log_Model Getlogdata(t_assess_list_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.Page = "t_assess_list";//model.LogStatus;
            model.Processing = "link";
            if (model.LogStatus == "t_chat")
                model.Remarks = model.LogId;
            if (model.LogStatus == "t_seller_assessment")
                model.Remarks = model.LogId+" "+model.LogRemarks;
            if (model.LogStatus == "t_reale_purchase")
                model.Remarks = model.LogId + " " + model.LogRemarks;
            return model;
        }

        [HttpPost]
        public ActionResult Generate_M_SellerMansionCSV(t_assess_listModel model)
        {
            t_assess_listBL bl = new t_assess_listBL();
            var dt = bl.Generate_M_SellerMansionCSV(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}