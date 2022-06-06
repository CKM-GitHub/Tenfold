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
        // GET: t_seller_account
        public ActionResult Index(String SellerCD)
        {
            SellerCD = "S000000017";
            if (!String.IsNullOrWhiteSpace(SellerCD))
            {
                ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer;
                strSellerCD = SellerCD;
                t_seller_accountBL bl = new t_seller_accountBL();
                var dt = bl.GetM_SellerBy_SellerCD(strSellerCD);
                ViewBag.TestFLG = dt.Rows[0]["TestFLG"].ToString() == "1" ? "checked" : "unchecked";
                ViewBag.InvalidFLG = dt.Rows[0]["InvalidFLG"].ToString() == "1" ? "checked" : "unchecked";
            }
            return View();
        }
    }
}