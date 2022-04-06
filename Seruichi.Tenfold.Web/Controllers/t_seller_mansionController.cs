using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_mansion;
using Seruichi.BL.Tenfold.t_seller_mansion;
using Seruichi.Common;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_mansionController : BaseController
    {
        // GET: t_seller_mansion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetM_SellerMansionList(t_seller_mansionModel model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();

            List<string> chk_lst = new List<string>();
            chk_lst.Add(model.Chk_Blue.ToString());
            chk_lst.Add(model.Chk_Sky.ToString());
            chk_lst.Add(model.Chk_Orange.ToString());
            chk_lst.Add(model.Chk_Green.ToString());
            chk_lst.Add(model.Chk_Brown.ToString());
            chk_lst.Add(model.Chk_Dark_Sky.ToString());
            chk_lst.Add(model.Chk_Gray.ToString());
            chk_lst.Add(model.Chk_Black.ToString());
            chk_lst.Add(model.Chk_Pink.ToString());

            var validationResult = bl.ValidateAll(model,chk_lst);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            var dt = bl.GetM_SellerMansionList(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult InsertM_SellerMansion_L_Log(t_seller_mansion_l_log_Model model)
        {
            t_seller_mansionBL bl = new t_seller_mansionBL();
            bl.InsertM_SellerMansion_L_Log(model);
            return OKResult();
        }
    }
}