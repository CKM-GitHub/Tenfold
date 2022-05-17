using Seruichi.BL.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Seller;
using Seruichi.BL;
using Seruichi.Common;
using System.Data;
using Models;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_assess_dController : BaseController
    {
        
        // GET: a_assess_d
        public ActionResult Index()
        {
            if (Session["AssReqID"] == null)
                Session["AssReqID"] = "AR00000025";
            return View();
        }
        [HttpPost]
        public ActionResult GetD_Mansion_Info(a_assess_dModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            model.AssReqID =  Session["AssReqID"].ToString();
            a_assess_dBL bl = new a_assess_dBL();
            var dt = bl.GetD_Mansion_Info(model); 
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetD_Spec_Info(a_assess_dModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            model.AssReqID = Session["AssReqID"].ToString();
            a_assess_dBL bl = new a_assess_dBL();
            var dt = bl.GetD_Spec_Info(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetD_MansionRank_Info(a_assess_dModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            model.AssReqID = Session["AssReqID"].ToString();
            a_assess_dBL bl = new a_assess_dBL();
            var dt = bl.GetD_MansionRank_Info(model);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetD_AreaRank_Info(a_assess_dModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            model.AssReqID = Session["AssReqID"].ToString();
            a_assess_dBL bl = new a_assess_dBL();
            var dt = bl.GetD_AreaRank_Info(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}