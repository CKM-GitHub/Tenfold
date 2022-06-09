using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;
using Models.Tenfold.t_reale_account;
using Seruichi.BL.Tenfold.t_reale_purchase;
using Seruichi.BL.Tenfold.t_reale_asmhis;
using Seruichi.BL.Tenfold.t_reale_account;
using System.Data;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_accountController : BaseController
    {
        // GET: t_reale_account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult get_t_reale_CompanyInfo(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_CompanyInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_reale_CompanyCountingInfo(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_CompanyCountingInfo(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]

        public ActionResult get_t_reale_account_DisplayData(t_reale_accountModel model)
        {
            t_reale_accountBL bl = new t_reale_accountBL();
            //List<string> chk_lst = new List<string>();
            //var validationResult = bl.ValidateAll(model, chk_lst);
            //if (validationResult.Count > 0)
            //{
            //    return ErrorResult(validationResult);
            //}

            var dt = bl.get_t_reale_account_DisplayData(model);
            var json = DataTableToJSON(dt.Tables[0]) + "Ʈ" + DataTableToJSON(dt.Tables[1]);
            return OKResult(json);
        }
        [HttpPost]

        public ActionResult get_t_reale_account_ManipulateData(t_reale_accountModel model)
        {
            t_reale_accountBL bl = new t_reale_accountBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
           
            model.TenStaffCD = base.GetOperator();  
            model.IPAddress = base.GetClientIP();
            bl.get_t_reale_account_ManipulateData(model); 
            return OKResult();
        }
        [HttpPost]
        public ActionResult Insert_L_Log(t_reale_purchase_l_log_Model model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            
            bl.Insert_L_Log(model);
            return OKResult();
        }
    }
}