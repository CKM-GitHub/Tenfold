using Models;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion_new;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

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
            a_indexBL bl = new a_indexBL();
            var dt = bl.GetMansionListByMansionWord(prefCD, searchWord);
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

            model.MansionStationList = ConvertJsonToObject<List<a_indexModel.MansionStation>>(model.MansionStationListJson);

            a_indexBL bl = new a_indexBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }
    }
   
}