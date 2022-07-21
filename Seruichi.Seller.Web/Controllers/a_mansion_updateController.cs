using Models;
using Models.Seller;
using Seruichi.BL;
using Seruichi.BL.Seller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_mansion_updateController : BaseController
    {
        // GET: a_mansion_update
        public ActionResult Index()
        {
            //if (string.IsNullOrEmpty(sellerMansionID))
            //    return BadRequestResult();

            string sellerMansionID = "SEL0000104";

            a_mansion_updateBL a_bl = new a_mansion_updateBL();
            a_mansion_updateModel model =Assign_Model_Value(a_bl.Get_M_SellerMansion_Data(sellerMansionID));

            ViewBag.UpdateData = model;
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            ViewBag.CityDropDownListItems = bl.GetDropDownListItemsOfCity(model.PrefCD);
            ViewBag.TownDropDownListItems = bl.GetDropDownListItemsOfTown(model.PrefCD, model.CityCD);
            ViewBag.FloorTypeDropDownListItems = bl.GetDropDownListItemsOfMultPurpose(104);
            return View();
        }
        [HttpPost]
        public ActionResult CheckAll(a_mansion_updateModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            model.MansionStationList = ConvertJsonToObject<List<MansionStation>>(model.MansionStationListJson);

            a_mansion_updateBL bl = new a_mansion_updateBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }

            return OKResult();
        }

        public a_mansion_updateModel Assign_Model_Value(DataTable dt)
        {
            a_mansion_updateModel model = new a_mansion_updateModel();
            model.SellerMansionID = dt.Rows[0]["SellerMansionID"].ToString();
            model.MansionCD = dt.Rows[0]["MansionCD"].ToString();
            model.MansionName = dt.Rows[0]["MansionName"].ToString();
            model.PrefCD = dt.Rows[0]["PrefCD"].ToString();
            model.PrefName = dt.Rows[0]["PrefName"].ToString();
            model.CityCD = dt.Rows[0]["CityCD"].ToString();
            model.CityName = dt.Rows[0]["CityName"].ToString();
            model.TownCD = dt.Rows[0]["TownCD"].ToString();
            model.Address = dt.Rows[0]["Address"].ToString();
            model.TownName = dt.Rows[0]["TownName"].ToString();
            model.StructuralKBN = Convert.ToByte(dt.Rows[0]["StructuralKBN"].ToString());
            if (dt.Rows[0]["ConstYYYYMM"].ToString().Length == 6)
            {
                string yyyy = dt.Rows[0]["ConstYYYYMM"].ToString().Substring(0, 4);
                string mm = dt.Rows[0]["ConstYYYYMM"].ToString().Substring(dt.Rows[0]["ConstYYYYMM"].ToString().Length - 2);
                model.ConstYYYYMM = yyyy + "-" + mm;
            }
            else
                model.ConstYYYYMM = "";
            model.Rooms = dt.Rows[0]["Rooms"].ToString();
            model.LocationFloor = dt.Rows[0]["LocationFloor"].ToString();
            model.Floors = dt.Rows[0]["Floors"].ToString();
            model.RoomNumber = dt.Rows[0]["RoomNumber"].ToString();
            model.RoomArea = dt.Rows[0]["RoomArea"].ToString();
            model.BalconyKBN = Byte.Parse(dt.Rows[0]["BalconyKBN"].ToString());
            model.BalconyArea = dt.Rows[0]["BalconyArea"].ToString();
            model.Direction = Byte.Parse(dt.Rows[0]["Direction"].ToString());
            model.FloorType = dt.Rows[0]["FloorType1"].ToString();
            model.FloorType2 = dt.Rows[0]["FloorType2"].ToString();
            model.BathKBN = Byte.Parse(dt.Rows[0]["BathKBN"].ToString());
            model.RightKBN = Byte.Parse(dt.Rows[0]["RightKBN"].ToString());
            model.CurrentKBN = Byte.Parse(dt.Rows[0]["CurrentKBN"].ToString());
            model.ManagementKBN = Byte.Parse(dt.Rows[0]["ManagementKBN"].ToString());
            model.RentFee = dt.Rows[0]["RentFee"].ToString();
            model.ManagementFee = dt.Rows[0]["ManagementFee"].ToString();
            model.RepairFee = dt.Rows[0]["RepairFee"].ToString();
            model.ExtraFee = dt.Rows[0]["ExtraFee"].ToString();
            model.PropertyTax = dt.Rows[0]["PropertyTax"].ToString();
            model.DesiredTime =Byte.Parse(dt.Rows[0]["DesiredTime"].ToString());            
            return model;
        }

        [HttpPost]
        public ActionResult GetlineStationDistanceBySellerMansionID(string SellerMansionID)
        {
            a_mansion_updateBL a_bl = new a_mansion_updateBL();
            var dt = a_bl.GetLineStationDistanceBySellerMansionID(SellerMansionID);
            return OKResult(DataTableToJSON(dt));
        }

        // Ajax: UpdateSellerMansionData
        [HttpPost]
        public ActionResult UpdateSellerMansionData(a_mansion_updateModel model)
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

            a_mansion_updateBL bl = new a_mansion_updateBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            if (!bl.UpdateSellerMansionData(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            return OKResult();
        }
        
    }
}