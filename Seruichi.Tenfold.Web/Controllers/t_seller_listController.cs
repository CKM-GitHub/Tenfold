using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Tenfold.t_seller_list;
using Seruichi.BL.Tenfold.t_seller_list;
using Seruichi.Common;


namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_listController : BaseController
    {
        // GET: t_seller_list
        public ActionResult Index()
        {
            t_seller_listBL bl = new t_seller_listBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListForPref();
            return View();
        }

        //[HttpPost]
        //public ActionResult CheckSellerName(string SellerName)
        //{
        //    if (SellerName == null)
        //    {
        //        return BadRequestResult();
        //    }

        //    //Validator validator = new Validator();
        //    //string errorcd = "";
        //    //if (!validator.CheckIsHalfWidth(SellerName, 10, out errorcd))
        //    //{
        //    //    return ErrorMessageResult(errorcd);
        //    //}
        //    return OKResult();
        //}

        [HttpPost]
        public ActionResult GetM_SellerList(t_seller_listModel model)
        {
            t_seller_listBL bl = new t_seller_listBL();
            var dt = bl.GetM_SellerList(model);
            return OKResult(DataTableToJSON(dt));
        }


    }
}