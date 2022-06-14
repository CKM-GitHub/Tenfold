using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models;
using Models.Tenfold.t_seller_new;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_seller_new;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_newController : BaseController
    {
        // GET: t_seller_new
        public ActionResult Index()
        {
            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfAllPrefecture();
            return View();
        }

        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(t_seller_newModel model)
        {
            Validator validator = new Validator();
            string errorcd = "";
            if (!validator.CheckIsHalfWidth(model.ZipCode1, 3, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            if (!validator.CheckIsHalfWidth(model.ZipCode2, 4, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            t_seller_newBL bl = new t_seller_newBL();
            if (!bl.CheckZipCode(model, out errorcd, out string prefCD, out string cityName, out string townName))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult(new { prefCD, cityName, townName });
        }


    }
}