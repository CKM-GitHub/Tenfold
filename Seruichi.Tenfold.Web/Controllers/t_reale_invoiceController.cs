using Models.Tenfold.t_reale_invoice;
using Seruichi.BL.Tenfold.t_reale_invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_invoiceController : BaseController
    {
        // GET: t_reale_invoice
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
        public ActionResult get_t_reale_CompanyInfo(t_reale_invoiceModel model)
        {
            t_reale_invoiceBL bl = new t_reale_invoiceBL();
            var dt = bl.get_t_reale_CompanyInfo(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_reale_CompanyCountingInfo(t_reale_invoiceModel model)
        {
            t_reale_invoiceBL bl = new t_reale_invoiceBL();
            var dt = bl.get_t_reale_CompanyCountingInfo(model);
            return OKResult(DataTableToJSON(dt));
        }


        [HttpPost]
        public ActionResult Get_D_Billing_List(t_reale_invoiceModel model)
        {
            t_reale_invoiceBL bl = new t_reale_invoiceBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            var dt = bl.Get_D_Billing_List(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}