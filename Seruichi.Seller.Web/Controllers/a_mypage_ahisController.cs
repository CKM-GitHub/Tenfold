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
    public class a_mypage_ahisController : BaseController
    {
        // GET: a_mypage_ahis
        [AllowAnonymous] 
        public ActionResult Index()
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "a_login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetD_AssReqProgressList(a_mypage_ahisModel model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            a_mypage_ahisBL bl = new a_mypage_ahisBL();
            var dt = bl.GetD_AssReqProgressList(model);

            foreach (DataRow dr in dt.Rows)
                dr["AssessAmount"]=(dr["AssessAmount"].ToString()) ==  "0円" ? "": (dr["AssessAmount"].ToString()) == "0 円" ? "" : (dr["AssessAmount"].ToString());

            return OKResult(DataTableToJSON(dt));
        }
        public string GEtdata(a_mypage_ahisModel model)
        {
            a_mypage_ahisBL bl = new a_mypage_ahisBL();
            var dt = bl.GetD_AssReqProgressList(model);
            return (DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult InsertGetD_AssReqProgress_L_Log(a_mypage_ahisModel_l_log_Model model)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
            model.SellerCD = user.UserID;
            a_mypage_ahisBL bl = new a_mypage_ahisBL(); 
            model = Getlogdata(model,model.Link);
            bl.InsertD_AssReqProgress_L_Log(model);
            return OKResult();

        }
        public a_mypage_ahisModel_l_log_Model Getlogdata(a_mypage_ahisModel_l_log_Model model, string PageID)
        {
            LoginUser user = SessionAuthenticationHelper.GetUserFromSession();
             
            a_mypage_ahisBL blc = new a_mypage_ahisBL();
            var dt = blc.GetD_AssReqProgressList(new a_mypage_ahisModel() { SellerCD = user.UserID }  );
            var result = dt.AsEnumerable().Where(myRow => myRow.Field<string>("SellerCD") == model.SellerCD).CopyToDataTable() ;

            CommonBL bl = new CommonBL();
            //a_mypage_uinfoBL bl1 = new a_mypage_uinfoBL();
            //a_mypage_uinfoModel model1 = bl1.GetSellerData(user.UserID);
            model.LoginKBN = 1;
            model.LoginID = base.GetOperator();
            model.RealECD = null;
            model.LoginName = base.GetOperatorName();///result.Rows[0]["SellerName"].ToStringOrNull(); ;
            model.IPAddress = base.GetClientIP();
            model.PageID = PageID;
            model.ProcessKBN = "link";
            model.Remarks = model.SellerCD + " " + result.Rows[0]["AssReqID"].ToStringOrNull(); ;

            return model;
        }
        ///InsertGetD_AssReqProgress_L_Log
    }
}