using Models.RealEstate.r_invoice;
using Seruichi.BL.RealEstate.r_invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_invoiceController : BaseController
    {
        // GET: r_invoice
        public ActionResult Index()
        {
            return View();
        }

        ////[HttpPost]
        ////public ActionResult Get_D_Billing_List(r_invoiceModel model)
        ////{
        ////    r_invoiceBL bl = new r_invoiceBL();
        ////    var validationResult = bl.ValidateAll(model);
        ////    if (validationResult.Count > 0)
        ////    {
        ////        return ErrorResult(validationResult);
        ////    }
        ////    var dt = bl.Get_D_Billing_List(model);
        ////    return OKResult(DataTableToJSON(dt));
        ////}
    }
}