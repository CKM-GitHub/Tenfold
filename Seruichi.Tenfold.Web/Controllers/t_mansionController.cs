using Models;
using Models.Tenfold.t_mansion;
using Seruichi.BL;
using Seruichi.BL.Tenfold.t_mansion;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansionController : BaseController
    {
        // GET: t_mansion
        public ActionResult Index(string MansionCD)
        {
            if (string.IsNullOrEmpty(MansionCD))
                return BadRequestResult();

            t_mansionBL t_bl = new t_mansionBL();
            t_mansionModel model =Assign_Model_Value(t_bl.Get_M_Mansion_Data(MansionCD));

            ViewBag.MansionModel = model;
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            ViewBag.CityDropDownListItems = bl.GetDropDownListItemsOfCity(model.PrefCD);
            ViewBag.TownDropDownListItems = bl.GetDropDownListItemsOfTown(model.PrefCD, model.CityCD);
            return View();
        }
        public t_mansionModel Assign_Model_Value(DataTable dt)
        {
            t_mansionModel model = new t_mansionModel();
            model.MansionCD = dt.Rows[0]["MansionCD"].ToString();
            model.MansionName = dt.Rows[0]["MansionName"].ToString();
            model.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString();
            model.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString();
            model.PrefCD = dt.Rows[0]["PrefCD"].ToString();
            model.PrefName = dt.Rows[0]["PrefName"].ToString();
            model.CityCD = dt.Rows[0]["CityCD"].ToString();
            model.CityName = dt.Rows[0]["CityName"].ToString();
            model.TownCD = dt.Rows[0]["TownCD"].ToString();
            model.TownName = dt.Rows[0]["TownName"].ToString();
            model.Address = dt.Rows[0]["Address"].ToString();
            model.RightKBN = dt.Rows[0]["RightKBN"].ToString() == "1" ? 1 : 2;
            model.StructuralKBN = dt.Rows[0]["StructuralKBN"].ToString() == "1" ?  1 : 2;
            string yyyy = dt.Rows[0]["ConstYYYYMM"].ToString().Substring(0, 4);
            string mm = dt.Rows[0]["ConstYYYYMM"].ToString().Substring(dt.Rows[0]["ConstYYYYMM"].ToString().Length - 2);
            model.ConstYYYYMM = yyyy + "-" + mm;
            model.Rooms = dt.Rows[0]["Rooms"].ToString();
            model.Floors = dt.Rows[0]["Floors"].ToString();
            model.Remark = dt.Rows[0]["Remark"].ToString();
            CommonBL bl = new CommonBL();
            model.InsertOperatorName = bl.GetTenstaffNamebyTenstaffcd(dt.Rows[0]["InsertOperator"].ToString());
            model.UpdateOperatorName = string.IsNullOrEmpty(dt.Rows[0]["UpdateOperator"].ToString())? "" : bl.GetTenstaffNamebyTenstaffcd(dt.Rows[0]["UpdateOperator"].ToString());
            model.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
            model.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();
            return model;
        }
        [HttpPost]
        public ActionResult GetLineStationDistanceByMansionCD(string MansionCD)
        {
            t_mansionBL t_bl = new t_mansionBL();
            var dt = t_bl.GetLineStationDistanceByMansionCD(MansionCD);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult GetMansionWordByMansionCD(string MansionCD)
        {
            t_mansionBL t_bl = new t_mansionBL();
            var dt = t_bl.GetMansionWordByMansionCD(MansionCD);
            return OKResult(DataTableToJSON(dt));
        }

        [HttpPost]
        public ActionResult CheckAll(t_mansionModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.MansionStationList = ConvertJsonToObject<List<t_mansionModel.MansionStation>>(model.MansionStationListJson);

            t_mansionBL bl = new t_mansionBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        [HttpPost]
        public ActionResult UpdateMansionData(t_mansionModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            string addressSearch = model.PrefName + model.CityName + model.TownName + model.Address;
            t_mansionBL bl = new t_mansionBL();
            CommonBL blCmm = new CommonBL();
            var longitude_latitude = blCmm.GetLongitudeAndLatitude(model.PrefName, model.CityName, model.TownName, model.Address);
            model.Longitude = longitude_latitude[0];
            model.Latitude = longitude_latitude[1];
            model.SellerCD = base.GetOperator();
            model.SellerName = base.GetOperatorName();
            model.Operator = base.GetOperator();
            model.IPAddress = base.GetClientIP();
            model.MansionStationList = ConvertJsonToObject<List<t_mansionModel.MansionStation>>(model.MansionStationListJson);
            model.MansionWordList = ConvertJsonToObject<List<t_mansionModel.MansionWord>>(model.MansionWordListJson);

            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.UpdateMansionData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }

        [HttpPost]
        public ActionResult CheckZipCode(t_mansionModel model)
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
    }
}