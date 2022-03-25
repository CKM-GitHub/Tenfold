﻿using Models;
using Seruichi.Common;
using Seruichi.BL;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_indexController : BaseController
    {
        // GET: a_index
        //[SessionFilter(false)]
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            return View();
        }


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
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.MansionStationList = ConvertJsonToObject<List<a_indexModel.MansionStation>>(model.MansionStationListJson);

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