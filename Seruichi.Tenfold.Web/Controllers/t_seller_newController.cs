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

        [HttpPost]
        public ActionResult CheckAll(t_seller_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            t_seller_newBL bl = new t_seller_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            return OKResult();
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

        [HttpPost]
        public ActionResult checkMailAddress(t_seller_newModel model)
        {
            t_seller_newBL bl = new t_seller_newBL();
            if (!bl.checkMailAddress(model))
                return ErrorMessageResult("E203");

            return OKResult();
        }

        [HttpPost]
        public ActionResult modify_SellerData(t_seller_newModel model)
        {
            t_seller_newBL bl = new t_seller_newBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();

            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            bl.modify_SellerData(model);
            return OKResult();
        }

    }
}