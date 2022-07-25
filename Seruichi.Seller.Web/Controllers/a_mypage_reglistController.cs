
using Models;
using Models.Seller;
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


      
        [HttpPost]
        public ActionResult InsertD_Assessment(a_mypage_reglistModel model)
        {
            string SellerCD = base.GetOperator();
            a_mypage_reglistBL bl = new a_mypage_reglistBL();
            model.SellerCD = base.GetOperator();
            model.LoginID = base.GetOperator();
            model.SellerName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            model.PageID = "a_mypage_reglist";
            model.ProcessKBN = "link";
            model.Remarks = model.SellerCD;
            bl.InsertD_Assessment(model);
            return OKResult();

        }

        [HttpPost]
        public ActionResult InsertL_Log(a_mypage_reglistModel model)
        {
            string SellerCD = base.GetOperator();
            a_mypage_reglistBL bl = new a_mypage_reglistBL();
            model.LoginKBN = 3;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();
            model.PageID = "a_mypage_reglist";
            model.ProcessKBN = "link";
            model.Remarks = model.SellerCD;
            bl.Insert_L_Log(model);
            return OKResult();

        }



        [HttpPost]
        public ActionResult get_t_sellerPossibleTime()
        {
            string SellerCD = base.GetOperator();
            a_mypage_reglistBL bl = new a_mypage_reglistBL();
            var dt = bl.get_t_sellerPossibleTime(SellerCD);

            return OKResult(DataTableToJSON(dt));
        }
    }
}