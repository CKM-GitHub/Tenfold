using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Seruichi.BL.Tenfold.t_reale_list;
using Models.Tenfold.t_reale_list;
using Seruichi.BL;
using System.Threading.Tasks;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_listController : BaseController
    {
        // GET: t_reale_list
        public ActionResult Index()
        {
            t_reale_listBL bl = new t_reale_listBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListForPref();
            return View();
        }

        [HttpPost]
        public ActionResult getM_RealList(t_reale_listModel model)
        {
            t_reale_listBL bl = new t_reale_listBL();

            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.EffectiveChk.ToString());
            chk_lst.Add(model.InValidCheck.ToString());
            
            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.getM_RealList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public async Task<ActionResult> Generate_CSV(t_reale_listModel model)
        {
            t_reale_listBL bl = new t_reale_listBL();
            var dt = await bl.Generate_CSV(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertM_Reale_L_Log(t_reale_l_log_Model model)
        {

            t_reale_listBL bl = new t_reale_listBL();
            model = Getlogdata(model);
            bl.InsertM_Seller_L_Log(model);
            return OKResult();

        }

        public t_reale_l_log_Model Getlogdata(t_reale_l_log_Model model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.PageID = "t_reale_list";
            model.ProcessKBN = "link";
            //model.Remarks = model.RealeCD + " " + bl.GetSellerNamebySellerCD(model.RealeCD);
            return model;
        }
    }
}