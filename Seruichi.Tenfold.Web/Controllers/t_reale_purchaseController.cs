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
        public ActionResult Index(string RealECD)
        {
            if (string.IsNullOrEmpty(RealECD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer;
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
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            bl.Insert_L_Log(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult get_t_reale_purchase_CSVData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_t_reale_purchase_CSVData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_Modal_HomeData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_Modal_HomeData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_Modal_ProfileData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_Modal_ProfileData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_Modal_ContactData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_Modal_ContactData(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_Modal_DetailData(t_reale_purchaseModel model)
        {
            t_reale_purchaseBL bl = new t_reale_purchaseBL();
            var dt = bl.get_Modal_DetailData(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}