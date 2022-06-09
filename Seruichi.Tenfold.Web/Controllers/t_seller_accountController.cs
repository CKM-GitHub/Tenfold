using Models.Tenfold.t_seller_account;
using Seruichi.Common;
using System;
using Seruichi.BL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.Tenfold.t_seller_account;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_accountController : BaseController
    {
        static string strSellerCD = string.Empty;
        static string strSellerMansionID = string.Empty;
        // GET: t_seller_account
        public ActionResult Index(String SellerCD)
        {


           
            return View();
        }

        [HttpPost]
        public ActionResult get_t_seller_Info(t_seller_accountModel model)
        {
           
            
            t_seller_accountBL bl = new t_seller_accountBL();
            var dt = bl.GetM_SellerBy_SellerCD(model);
            return OKResult(DataTableToJSON(dt));
        }


    }


}