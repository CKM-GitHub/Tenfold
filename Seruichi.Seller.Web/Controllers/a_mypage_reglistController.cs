
using Seruichi.BL;
using Seruichi.BL.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mypage_reglistController : BaseController
    {
        // GET: a_mypage_reglist
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult get_displaydata()
        {
            string SellerCD = base.GetOperator();
            a_mypage_reglistBL bl = new a_mypage_reglistBL();

            var dt = bl.get_displaydata(SellerCD);

            return OKResult(DataTableToJSON(dt));
        }
    }
}