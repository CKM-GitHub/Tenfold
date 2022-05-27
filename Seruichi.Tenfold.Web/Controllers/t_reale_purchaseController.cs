using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;
using Seruichi.BL.Tenfold.t_reale_purchase;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_purchaseController : BaseController
    {
        // GET: t_reale_purchase
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult get_t_reale_purchase_DisplayData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.chk_Purchase.ToString());
            chk_lst.Add(model.chk_Checking.ToString());
            chk_lst.Add(model.chk_Nego.ToString());
            chk_lst.Add(model.chk_Contract.ToString());
            chk_lst.Add(model.chk_SellerDeclined.ToString());
            chk_lst.Add(model.chk_BuyerDeclined.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.get_t_reale_purchase_DisplayData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_L_Log(t_reale_purchase_l_log_Model model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = "";//bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.Page = model.LogStatus;
            model.Processing = "link";
            bl.Insert_L_Log(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult Generate_M_SellerMansionCSV(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_purchase_CSVData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Get_Pills_Home(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.Get_Pills_Home(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Get_Pills_Profile(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.Get_Pills_Profile(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Get_Pills_Contact(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.Get_Pills_Contact(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}