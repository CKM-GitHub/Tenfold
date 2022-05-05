using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_issueslist;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_issueslist;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_issueslistController : BaseController
    {
        // GET: r_issueslist
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.ContactTypeDropDownListItems = bl.GetDropDownListItemsOfStaff_by_RealECD(base.GetOperator("RealECD"));
            return View();
        }

        [HttpPost]
        public ActionResult get_issueslist_Data(r_issueslistModel model)
        {
            r_issueslistBL bl = new r_issueslistBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.chk_New.ToString());
            chk_lst.Add(model.chk_Nego.ToString());
            chk_lst.Add(model.chk_Contract.ToString());
            chk_lst.Add(model.chk_SellerDeclined.ToString());
            chk_lst.Add(model.chk_BuyerDeclined.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            model.RealECD = base.GetOperator("RealECD");
            var dt = bl.get_issueslist_Data(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult Insert_l_log(r_issueslistModel model)
        {
            r_issueslistBL bl = new r_issueslistBL();
            model = Getlogdata(model);
            bl.Insertr_issueslist_L_Log(model);
            return OKResult();
        }

        public r_issueslistModel Getlogdata(r_issueslistModel model)
        {
            CommonBL bl = new CommonBL();
            model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = bl.GetTenstaffNamebyTenstaffcd(model.LoginID);
            model.IPAddress = base.GetClientIP();
            model.PageID = model.PageID;
            model.ProcessKBN = "link";
            model.Remarks = model.AssReqID;
            return model;
        }

        [HttpPost]
        public ActionResult generate_r_issueslist_CSV(r_issueslistModel model)
        {
            r_issueslistBL bl = new r_issueslistBL();
            var dt = bl.get_issueslist_Data(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_SellerDetails_Data(r_issueslistModel model)
        {
            r_issueslistBL bl = new r_issueslistBL();
            var dt = bl.get_SellerDetails_Data(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}