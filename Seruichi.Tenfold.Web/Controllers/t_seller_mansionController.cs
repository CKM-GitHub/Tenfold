using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_mansion;
using Seruichi.BL.Tenfold.t_seller_mansion;
using Seruichi.Common;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_mansionController : BaseController
    {
        // GET: t_seller_mansion
        public ActionResult Index()
        {
            DBAccess db = new DBAccess();
            string a = "a";
            return View();
        }

        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_seller_mansionModel model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            var dt = bl.GetM_SellerMansionList(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}