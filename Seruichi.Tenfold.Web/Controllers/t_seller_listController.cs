using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_list;
using Seruichi.BL.Tenfold.t_seller_list;
using Seruichi.BL;
using Seruichi.Common;


namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_listController : BaseController
    {
        // GET: t_seller_list
        public ActionResult Index()
        {
            t_seller_listBL bl = new t_seller_listBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListForPref();
            return View();
        }

        [HttpPost]
        public ActionResult GetM_SellerList(t_seller_listModel model)
        {
            t_seller_listBL bl = new t_seller_listBL();

            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.ValidCheck.ToString());
            chk_lst.Add(model.InValidCheck.ToString());
            chk_lst.Add(model.expectedCheck.ToString());
            chk_lst.Add(model.negtiatioinsCheck.ToString());
            chk_lst.Add(model.endCheck.ToString());
            
            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            
            var dt = bl.GetM_SellerList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Generate_CSV(t_seller_listModel model)
        {
            t_seller_listBL bl = new t_seller_listBL();
            var dt = bl.Generate_CSV(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertM_Seller_L_Log(t_seller_l_log_Model model)
        {

            t_seller_listBL bl = new t_seller_listBL();
            model = Getlogdata(model);
            bl.InsertM_Seller_L_Log(model);
            return OKResult();

        }

        public t_seller_l_log_Model Getlogdata(t_seller_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.PageID = "t_seller_list";
            model.ProcessKBN = "www.seruichi.com" + model.SellerCD;
            model.Remarks = model.SellerCD + " " + bl.GetSellerNamebySellerCD(model.SellerCD);
            return model;
        }
    }
}