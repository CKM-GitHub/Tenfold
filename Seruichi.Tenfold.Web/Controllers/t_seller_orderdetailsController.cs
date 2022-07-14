using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Seruichi.BL.Tenfold.t_seller_orderdetails;
using Models;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_orderdetailsController : BaseController
    {
        // GET: t_seller_orderdetails
        static string strSellerCD = string.Empty;
        public ActionResult Index(string SellerCD)
        {
            if (string.IsNullOrEmpty(SellerCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }
            strSellerCD = SellerCD;
            t_seller_orderdetailsBL bl = new t_seller_orderdetailsBL();
            string sellername = bl.get_t_sellerName(SellerCD);
            string possibletime = bl.get_t_sellerPossibleTime(SellerCD);
            ViewBag.Title = "管理－売主－" + sellername;
            ViewBag.LoginID = base.GetOperator();
            ViewBag.PossibleTime = possibletime;
            return View();
        }

        [HttpPost]
        public ActionResult get_t_seller_Info()
        {
            t_seller_orderdetailsBL bl = new t_seller_orderdetailsBL();
            var dt = bl.get_t_seller_Info(strSellerCD);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_D_SellerPossibleData()
        {
           
            t_seller_orderdetailsBL bl = new t_seller_orderdetailsBL();
            var dt = bl.get_D_SellerPossibleData(strSellerCD);
            return OKResult(DataTableToJSON(dt));
        }


    }
}