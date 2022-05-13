using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion_list;
using Models.Tenfold.t_mansion_list;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansion_listController : BaseController
    {
        // GET: t_mansion_list
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetM_MansionList(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();

            List<string> chk_lst = new List<string>();
            
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.GetM_MansionList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Generate_CSV1(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();
            var dt = bl.Generate_CSV1(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult Generate_CSV2(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();
            var dt = bl.Generate_CSV2(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Generate_CSV3(t_mansion_listModel model)
        {
            t_mansion_listBL bl = new t_mansion_listBL();
            var dt = bl.Generate_CSV3(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertM_Seller_L_Log(t_mansion_list_l_log_Model model)
        {

            t_mansion_listBL bl = new t_mansion_listBL();
            model = Getlogdata(model);
            bl.InsertM_Mansion_List_L_Log(model);
            return OKResult();

        }

        public t_mansion_list_l_log_Model Getlogdata(t_mansion_list_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.PageID = "t_mansion_list";
            model.ProcessKBN = "link";
            model.Remarks = model.MansionCD + " " + bl.GetSellerNamebySellerCD(model.MansionCD);
            return model;
        }
    }
}