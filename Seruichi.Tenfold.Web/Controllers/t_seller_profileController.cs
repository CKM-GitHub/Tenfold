using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models;
using Models.Tenfold.t_seller_profile;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_seller_profile;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_profileController : BaseController
    {
        // GET: t_seller_profile
        public ActionResult Index(string SellerCD)
        {
            if (string.IsNullOrEmpty(SellerCD))
            {
                return RedirectToAction("BadRequest", "Error");
            }

            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfAllPrefecture();
            return View();
        }

        [HttpPost]
        public ActionResult get_t_seller_Info(t_seller_profileModel model)
        {
            t_seller_profileBL bl = new t_seller_profileBL();
            var dt = bl.get_t_seller_Info(model);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult get_t_seller_profile_DisplayData(t_seller_profileModel model)
        {
            t_seller_profileBL bl = new t_seller_profileBL();
            var dt = bl.get_t_seller_profile_DisplayData(model);
            return OKResult(DataTableToJSON(dt));
        }

        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(t_seller_profileModel model)
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

            t_seller_profileBL bl = new t_seller_profileBL();
            if (!bl.CheckZipCode(model, out errorcd, out string prefCD, out string cityName, out string townName))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult(new { prefCD, cityName, townName });
        }

        [HttpPost]
        public ActionResult modify_SellerData(t_seller_profileModel model)
        {
            t_seller_profileBL bl = new t_seller_profileBL();
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

        [HttpPost]
        public ActionResult modify_SellerPW(t_seller_profileModel model)
        {
            t_seller_profileBL bl = new t_seller_profileBL();
            model.LoginID = base.GetOperator();
            model.LoginName = base.GetOperatorName();
            model.IPAddress = base.GetClientIP();

            var validationResult = bl.ValidatePW(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            bl.modify_SellerPW(model);
            return OKResult();
        }
    }
}