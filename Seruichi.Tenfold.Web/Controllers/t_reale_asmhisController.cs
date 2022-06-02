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
    public class t_reale_asmhisController : BaseController
    {
        // GET: t_reale_asmhis
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
    }
}