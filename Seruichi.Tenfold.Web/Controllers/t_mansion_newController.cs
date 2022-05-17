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

            model.MansionStationList = ConvertJsonToObject<List<a_indexModel.MansionStation>>(model.MansionStationListJson);

            t_mansion_newBL bl = new t_mansion_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }
    }
}