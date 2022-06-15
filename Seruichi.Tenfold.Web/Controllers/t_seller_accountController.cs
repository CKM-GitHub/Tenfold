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
            t_seller_accountBL bl = new t_seller_accountBL();
            var dt = bl.get_t_sellerName(SellerCD);
            ViewBag.Title = "管理－売主－" + dt.Rows[0]["SellerName"].ToString();
            ViewBag.LoginID = base.GetOperator();
            return View();
        }



        [HttpPost]
        public ActionResult get_t_seller_Info(t_seller_accountModel model)
        {
            t_seller_accountBL bl = new t_seller_accountBL();
            var dt = bl.get_t_seller_Info(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult insert_M_Seller(t_seller_accountModel model)
        {
            t_seller_accountBL bl = new t_seller_accountBL();
            model.LoginID = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.LoginName = base.GetOperatorName();
            bl.insert_M_Seller(model);
            return OKResult();
        }
    }
}