using Models.Tenfold.t_seller_assessment;
using Seruichi.Common;
using System;
using Seruichi.BL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.Tenfold.t_seller_assessment;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_assessmentController : BaseController
    {
        static string strSellerCD = string.Empty;
        static string strSellerMansionID = string.Empty;
        // GET: t_seller_assessment
        public ActionResult Index(string SellerCD)
        {
            //SellerCD = "S000000017";
            if (!String.IsNullOrWhiteSpace(SellerCD))
            {
                ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer;
                strSellerCD = SellerCD;
                t_seller_assessmentBL bl = new t_seller_assessmentBL();
                t_seller_assessment_header_Model model = bl.GetM_SellerBy_SellerCD(strSellerCD);
                ViewBag.SellerModel = model;
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_seller_assessmentModel model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.Chk_Mi.ToString());
            chk_lst.Add(model.Chk_Kan.ToString());
            chk_lst.Add(model.Chk_Satei.ToString());
            chk_lst.Add(model.Chk_Kaitori.ToString());
            chk_lst.Add(model.Chk_Kakunin.ToString());
            chk_lst.Add(model.Chk_Kosho.ToString());
            chk_lst.Add(model.Chk_Seiyaku.ToString());
            chk_lst.Add(model.Chk_Urinushi.ToString());
            chk_lst.Add(model.Chk_Kainushi.ToString());
            chk_lst.Add(model.Chk_Sakujo.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            var dt = bl.GetM_SellerMansionList(model,strSellerCD);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_l_log(t_seller_assessment_l_log_Model model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            model = Getlogdata(model);
            bl.InsertM_SellerMansion_L_Log(model);
            return OKResult();
        }

        public t_seller_assessment_l_log_Model Getlogdata(t_seller_assessment_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.Page = model.LogStatus;
            model.Processing = "link";
            //if (model.LogStatus == "t_mansion_detail")
            //    model.Remarks = model.LogId;
            if (model.LogStatus == "t_reale_purchase")
                model.Remarks = model.LogId;
            if (model.LogStatus == "t_seller_assessment_detail")
                model.Remarks = model.LogId;
            return model;
        }

        [HttpPost]
        public ActionResult Generate_M_SellerMansionCSV(t_seller_assessmentModel model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Generate_M_SellerMansionCSV(model, strSellerCD);
            return OKResult(DataTableToJSON(dt));
        }
        
        //---------ポップアップ----------//
        [HttpPost]
        public ActionResult Get_PopupFor_Home(t_seller_assessment_Popup_Model model)
        {
            strSellerMansionID = model.SellerMansionID;
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Get_PopupFor_Home(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult Get_PopupFor_ResultType_1(t_seller_assessment_Popup_Model model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Get_PopupFor_ResultType_1(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult Get_PopupFor_ResultType_2(t_seller_assessment_Popup_Model model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Get_PopupFor_ResultType_2(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult Get_PopupFor_Detail(t_seller_assessment_Popup_Model model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Get_PopupFor_Detail(strSellerMansionID);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Get_PopupFor_Seller(t_seller_assessment_Popup_Model model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            var dt = bl.Get_PopupFor_Seller(strSellerCD);
            return OKResult(DataTableToJSON(dt));
        }
    }
}