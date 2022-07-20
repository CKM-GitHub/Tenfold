using Models;
using Models.Seller;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Seller
{
    public class a_mansion_updateBL
    {
        private CommonBL commonBL = new CommonBL();
        public Dictionary<string, string> ValidateAll(a_mansion_updateModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

           
            //都道府県
            validator.CheckSelectionRequired("PrefCD", model.PrefCD);
            //マンション名
            validator.CheckRequired("MansionName", model.MansionName);
            validator.CheckIsDoubleByte("MansionName", model.MansionName, 100);
            //市区町村
            validator.CheckSelectionRequired("CityCD", model.CityCD);
            //町域
            validator.CheckSelectionRequired("TownCD", model.TownCD);
            //住所
            validator.CheckRequired("Address", model.Address);
            validator.CheckIsDoubleByte("Address", model.Address, 50);
            //交通アクセス
            if (model.MansionStationList.Count == 0)
            {
                validator.CheckSelectionRequired("LineCD_1", "");
            }
            else
            {
                foreach (var item in model.MansionStationList)
                {
                    validator.CheckSelectionRequired("StationCD_1", item.StationCD);
                    validator.CheckRequiredNumber("Distance_1", item.Distance, true);
                    validator.CheckIsNumeric("Distance_1", item.Distance, 2, 0);
                }
            }
            //建物構造
            validator.CheckSelectionRequired("StructuralKBN", model.StructuralKBN);
            //築年月
            validator.CheckRequired("ConstYYYYMM", model.ConstYYYYMM);
            validator.CheckYMDate("ConstYYYYMM", model.ConstYYYYMM);
            //総戸数
            validator.CheckRequiredNumber("Rooms", model.Rooms, true);
            validator.CheckIsNumeric("Rooms", model.Rooms, 3, 0);
            //階
            validator.CheckRequiredNumber("LocationFloor", model.LocationFloor, true);
            validator.CheckIsNumeric("LocationFloor", model.LocationFloor, 2, 0);
            //階建て
            validator.CheckRequiredNumber("Floors", model.Floors, true);
            validator.CheckIsNumeric("Floors", model.Floors, 2, 0);
            //部屋番号
            validator.CheckRequired("RoomNumber", model.RoomNumber);
            validator.CheckIsHalfWidth("RoomNumber", model.RoomNumber, 5, RegexFormat.NumAlphaLowUp);
            //専有面積
            validator.CheckRequiredNumber("RoomArea", model.RoomArea, true);
            validator.CheckIsNumeric("RoomArea", model.RoomArea, 3, 2);
            //バルコニー区分
            validator.CheckSelectionRequired("BalconyKBN", model.BalconyKBN);
            //バルコニー面積
            if (model.BalconyKBN == 1)
            {
                validator.CheckRequiredNumber("BalconyArea", model.BalconyArea, true);
                validator.CheckIsNumeric("BalconyArea", model.BalconyArea, 3, 2);
            }
            else if (model.BalconyKBN == 2)
            {
                model.BalconyArea = "";
            }
            //主採光
            validator.CheckSelectionRequired("Direction", model.Direction);
            //間取り
            validator.CheckRequiredNumber("FloorType", model.FloorType, true);
            validator.CheckIsNumeric("FloorType", model.FloorType, 2, 0);
            validator.CheckSelectionRequired("FloorType2", model.FloorType2);
            //バス・トイレ
            validator.CheckSelectionRequired("BathKBN", model.BathKBN);
            //土地・権利
            validator.CheckSelectionRequired("RightKBN", model.RightKBN);
            //現況
            validator.CheckSelectionRequired("CurrentKBN", model.CurrentKBN);
            //管理方式
            validator.CheckSelectionRequired("ManagementKBN", model.ManagementKBN);
            //月額家賃
            validator.CheckRequiredNumber("RentFee", model.RentFee, false);
            validator.CheckIsNumeric("RentFee", model.RentFee, 9, 0);
            //管理費
            validator.CheckRequiredNumber("ManagementFee", model.ManagementFee, false);
            validator.CheckIsNumeric("ManagementFee", model.ManagementFee, 9, 0);
            //修繕積立金
            validator.CheckRequiredNumber("RepairFee", model.RepairFee, false);
            validator.CheckIsNumeric("RepairFee", model.RepairFee, 9, 0);
            //その他費用
            //validator.CheckRequiredNumber("ExtraFee", model.ExtraFee, false);
            validator.CheckIsNumeric("ExtraFee", model.ExtraFee, 9, 0);
            //固定資産税
            validator.CheckRequiredNumber("PropertyTax", model.PropertyTax, false);
            validator.CheckIsNumeric("PropertyTax", model.PropertyTax, 9, 0);
            //売却希望時期
            validator.CheckSelectionRequired("DesiredTime", model.DesiredTime);

            if (validator.IsValid)
            {
                foreach (var item in model.MansionStationList)
                {
                    if (!string.IsNullOrEmpty(item.StationCD))
                    {
                        if (model.MansionStationList.Any(r => r.LineCD == item.LineCD && r.StationCD == item.StationCD && r.RowNo < item.RowNo))
                        {
                            validator.AddValidationResult("StationCD_" + item.RowNo.ToStringOrEmpty(), "E210");
                        }
                    }
                }
                if (Utilities.GetSysDateTime().ToString("yyyyMM").ToInt32(0) < model.ConstYYYYMM.ToInt32(0))
                {
                    validator.AddValidationResult("ConstYYYYMM", "E208");
                }
                if (model.Floors.ToInt32(0) < model.LocationFloor.ToInt32(0))
                {
                    validator.AddValidationResult("LocationFloor", "E209");
                }
            }

            string errorcd = "";
           

            //M_Counter
            if (!commonBL.CheckExistsCounterMaster(CounterKey.MansionID, out errorcd))
            {
                validator.AddValidationResult("btnShowConfirmation", errorcd);
            }

            return validator.GetValidationResult();
        }

        

        public DataTable Get_M_SellerMansion_Data(string id)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = id.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mansion_update_Select_M_SellerMansion_Data", sqlParams);
            return dt;
        }
        public DataTable GetLineStationDistanceBySellerMansionID(string id)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = id.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mansion_update_Select_M_SellerMansion_Data", sqlParams);
            return dt;
        }
    }
}
