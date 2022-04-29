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
    public class r_issueslistController : Controller
    {
        // GET: r_issueslist
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Get_DataList(r_issueslistModel model)
        //{
        //    r_issueslistBL bl = new r_issueslistBL();
        //    List<string> chk_lst = new List<string>();
        //    chk_lst.Add(model.Chk_Mi.ToString());
        //    chk_lst.Add(model.Chk_Kan.ToString());
        //    chk_lst.Add(model.Chk_Satei.ToString());
        //    chk_lst.Add(model.Chk_Kaitori.ToString());
        //    chk_lst.Add(model.Chk_Kakunin.ToString());
        //    chk_lst.Add(model.Chk_Kosho.ToString());
        //    chk_lst.Add(model.Chk_Seiyaku.ToString());
        //    chk_lst.Add(model.Chk_Urinushi.ToString());
        //    chk_lst.Add(model.Chk_Kainushi.ToString());

        //    var validationResult = bl.ValidateAll(model, chk_lst);
        //    if (validationResult.Count > 0)
        //    {
        //        return ErrorResult(validationResult);
        //    }
        //    var dt = bl.GetM_SellerMansionList(model);
        //    return OKResult(DataTableToJSON(dt));
        //}
    }
}