using Models;
using Models.Tenfold.t_reale_new;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_reale_new;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_newController : BaseController
    {
        // GET: t_reale_new
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            ViewBag.MultPurposeDropDownListItems = bl.GetDropDownListItemsOfMultPurpose(110);
            ViewBag.CourseDropDownListItems = bl.GetDropDownListItemsOfCourse();
            return View();
        }

        [HttpPost]
        public ActionResult CheckZipCode(t_reale_newModel model)
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

            t_reale_newBL bl = new t_reale_newBL();
            if (!bl.CheckPrefecturesByZipCode(model.ZipCode1, model.ZipCode2, out errorcd, out outPrefCD))
            {
                return ErrorMessageResult(errorcd);
            }

            string outCityCD = bl.GetCityCDByZipCode(model.ZipCode1, model.ZipCode2);
            string outTownCD = bl.GetTownCDByZipCode(model.ZipCode1, model.ZipCode2);

            return OKResult(new { PrefCD = outPrefCD, CityCD = outCityCD, TownCD = outTownCD });
        }

        [HttpPost]
        public ActionResult GetBankListByBankWord(string searchWord)
        {
            if (string.IsNullOrEmpty(searchWord))
            {
                return BadRequestResult();
            }
            t_reale_newBL bl = new t_reale_newBL();
            var dt = bl.GetBankListByBankWord(searchWord);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetBankBranchListByBankCD(string bankcd)
        {
            if (string.IsNullOrEmpty(bankcd))
            {
                return BadRequestResult();
            }
            t_reale_newBL bl = new t_reale_newBL();
            var dt = bl.GetBankBranchListByBankCD(bankcd);
            return OKResult(DataTableToJSON(dt));
        }
        [HttpPost]
        public ActionResult GetBankBranchListByBranchWord(string searchWord,string bankCD)
        {
            if (string.IsNullOrEmpty(searchWord) && string.IsNullOrEmpty(bankCD))
            {
                return BadRequestResult();
            }
            t_reale_newBL bl = new t_reale_newBL();
            var dt = bl.GetBankBranchListByBranchWord(searchWord.Trim(), bankCD);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult CheckAll(t_reale_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            t_reale_newBL bl = new t_reale_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            return OKResult();
        }
        [HttpPost]
        public ActionResult Save_t_reale_new(t_reale_newModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.LoginName = base.GetOperatorName();

            t_reale_newBL bl = new t_reale_newBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.Save_t_reale_new(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }

    }
}