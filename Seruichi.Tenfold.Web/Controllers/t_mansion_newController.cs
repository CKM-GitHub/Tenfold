using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL;
using Seruichi.Common;
using Models;
using Seruichi.BL.Tenfold.t_mansion_new;
using Models.Tenfold.t_mansion_new;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansion_newController : BaseController
    {
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            return View();
        }
        // POST: 
        [HttpPost]
        public ActionResult GotoNextPage()
        {
            return RedirectToAction("Index", "t_mansion_new");
        }


        // Ajax: CheckZipCode
        [HttpPost]
        public ActionResult CheckZipCode(t_mansion_newModel model)
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

            t_mansion_newBL bl = new t_mansion_newBL();
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
            t_mansion_newBL bl = new t_mansion_newBL();
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
        public ActionResult CheckAll(t_mansion_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.MansionStationList = ConvertJsonToObject<List<t_mansion_newModel.MansionStation>>(model.MansionStationListJson);

            t_mansion_newBL bl = new t_mansion_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }


        // Ajax: InsertSellerMansionData
        [HttpPost]
        public ActionResult InsertSellerMansionData(t_mansion_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            string addressSearch = model.PrefName + model.CityName + model.TownName + model.Address;
            t_mansion_newBL bl = new t_mansion_newBL();
            (string longi, string lati) = bl.AddressSearch(addressSearch);

            CommonBL blCmm = new CommonBL();
            model.Longitude = longi[0];
            model.Latitude = lati[0];
            model.SellerCD = base.GetOperator();
            model.SellerName = base.GetOperatorName();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.MansionStationList = ConvertJsonToObject<List<t_mansion_newModel.MansionStation>>(model.MansionStationListJson);
            model.MansionWordList = ConvertJsonToObject<List<t_mansion_newModel.MansionWord>>(model.MansionWordListJson);

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
        } // Ajax: InsertSellerMansionData


       

    }
}