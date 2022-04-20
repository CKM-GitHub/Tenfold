using Models.Tenfold.t_seller_assessment;
using Seruichi.BL.Tenfold.t_seller_assessment;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_assessmentController : BaseController
    {
        // GET: t_seller_assessment
        public ActionResult Index(string SellerCD)
        {
            SellerCD = "0000001";
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            t_seller_assessment_header_Model model = bl.GetM_SellerBy_SellerCD(SellerCD);
            ViewBag.SellerModel = model;
            return View();
        }
        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_seller_assessmentModel model)
        {
            t_seller_assessmentBL bl = new t_seller_assessmentBL();
            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.Chk_Mi.ToString());
            chk_lst.Add(model.Chk_Kan.ToString());
            chk_lst.Add(model.Chk_Satei.ToString());
            chk_lst.Add(model.Chk_Kaitori.ToString());
            chk_lst.Add(model.Chk_Kakunin.ToString());
            chk_lst.Add(model.Chk_Kosho.ToString());
            chk_lst.Add(model.Chk_Seiyaku.ToString());
            chk_lst.Add(model.Chk_Urinushi.ToString());
            chk_lst.Add(model.Chk_Kainushi.ToString());
            chk_lst.Add(model.Chk_Sakujo.ToString());

            var validationResult = bl.ValidateAll(model, chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            var dt = bl.GetM_SellerMansionList(model);
            return OKResult(DataTableToJSON(dt));
        }
    }
}