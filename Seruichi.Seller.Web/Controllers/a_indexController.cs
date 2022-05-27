using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    [AllowAnonymous]
    public class a_indexController : BaseController
    {
        // GET: a_index
        [HttpGet]
        public ActionResult Index()
        {
            if (SessionAuthenticationHelper.GetUserFromSession() == null)
            {
                Session.Clear();
                SessionAuthenticationHelper.CreateAnonymousUser();
            }

            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            ViewBag.FloorTypeDropDownListItems = bl.GetDropDownListItemsOfMultPurpose(104);
            return View();
        }

        // POST: 
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "a_index");
        }

        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(a_indexModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            string outPrefCD = "";

            if (!validator.CheckIsHalfWidth(model.ZipCode1, 3, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            if (!validator.CheckIsHalfWidth(model.ZipCode2, 4, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            a_indexBL bl = new a_indexBL();
            if (!bl.CheckPrefecturesByZipCode(model.ZipCode1, model.ZipCode2, out errorcd, out outPrefCD))
            {
                return ErrorMessageResult(errorcd);
            }

            string outCityCD = bl.GetCityCDByZipCode(model.ZipCode1, model.ZipCode2);
            string outTownCD = bl.GetTownCDByZipCode(model.ZipCode1, model.ZipCode2);

            return OKResult(new { PrefCD = outPrefCD, CityCD = outCityCD, TownCD = outTownCD });
        }

        // Ajax: GetMansionListByMansionWord
        [HttpPost]
        public ActionResult GetMansionListByMansionWord(string prefCD, string searchWord)
        {
            if (string.IsNullOrEmpty(searchWord))
            {
                return BadRequestResult();
            }
            a_indexBL bl = new a_indexBL();
            var dt = bl.GetMansionListByMansionWord(prefCD, searchWord);
            return OKResult(DataTableToJSON(dt));
        }

        // Ajax: GetMansionData
        [HttpPost]
        public ActionResult GetMansionData(string mansionCD)
        {
            if (string.IsNullOrEmpty(mansionCD))
            {
                return BadRequestResult();
            }
            a_indexBL bl = new a_indexBL();
            var dt = bl.GetMansionData(mansionCD);
            return OKResult(DataTableToJSON(dt));
        }

        // Ajax: CheckAll
        [HttpPost]
        public ActionResult CheckAll(a_indexModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.MansionStationList = ConvertJsonToObject<List<MansionStation>>(model.MansionStationListJson);

            a_indexBL bl = new a_indexBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        // Ajax: InsertSellerMansionData
        [HttpPost]
        public ActionResult InsertSellerMansionData(a_indexModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            CommonBL blCmm = new CommonBL();
            var longitude_latitude = blCmm.GetLongitudeAndLatitude(model.PrefName, model.CityName, model.TownName, model.Address);
            model.Longitude = longitude_latitude[0];
            model.Latitude = longitude_latitude[1];
            model.SellerCD = base.GetOperator();
            model.SellerName = base.GetOperatorName();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.MansionStationList = ConvertJsonToObject<List<MansionStation>>(model.MansionStationListJson);

            a_indexBL bl = new a_indexBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.InsertSellerMansionData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
    }
}