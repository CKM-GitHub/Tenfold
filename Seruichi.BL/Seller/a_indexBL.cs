using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;

namespace Seruichi.BL
{
    public class a_indexBL
    {
        private CommonBL commonBL = new CommonBL();

        public Dictionary<string, string> ValidateAll(a_indexModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //郵便番号
            validator.CheckIsHalfWidth("ZipCode1", model.ZipCode1, 3, RegexFormat.Number);
            validator.CheckIsHalfWidth("ZipCode2", model.ZipCode2, 4, RegexFormat.Number);
            //都道府県
            validator.CheckSelectionRequired("PrefCD", model.PrefCD);
            //マンション名
            validator.CheckRequired("MansionName", model.MansionName);
            validator.CheckIsDoubleByte("MansionName", model.MansionName, 50);
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
            validator.CheckRequiredNumber("ExtraFee", model.ExtraFee, false);
            validator.CheckIsNumeric("ExtraFee", model.ExtraFee, 9, 0);
            //固定資産税
            validator.CheckRequiredNumber("PropertyTax", model.PropertyTax, false);
            validator.CheckIsNumeric("PropertyTax", model.PropertyTax, 9, 0);

            string errorcd = "";

            //M_Pref
            if (!string.IsNullOrEmpty(model.ZipCode1) || !string.IsNullOrEmpty(model.ZipCode2))
            {
                if (!CheckPrefecturesByZipCode(model.ZipCode1, model.ZipCode2, out errorcd, out string outPrefCD))
                {
                    if (!string.IsNullOrEmpty(model.ZipCode1))
                        validator.AddValidationResult("ZipCode1", errorcd);


                    if (!string.IsNullOrEmpty(model.ZipCode2))
                        validator.AddValidationResult("ZipCode2", errorcd);
                }
            }

            //M_Counter
            if (!commonBL.CheckExistsCounterMaster(CounterKey.MansionID, out errorcd))
            {
                validator.AddValidationResult("btnShowConfirmation", errorcd);
            }

            return validator.GetValidationResult();
        }

        public bool CheckPrefecturesByZipCode(string zipCode1, string zipCode2, out string errorcd, out string outPrefCD)
        {
            errorcd = "";
            outPrefCD = "";

            var sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = zipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = zipCode2.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_index_Select_Prefectures_by_ZipCode", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E103"; //入力された値が正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["RegionCD"].ToStringOrEmpty()))
            {
                errorcd = "E201"; //査定サービスの対象外地域です
                return false;
            }

            outPrefCD = dr["PrefCD"].ToStringOrEmpty();
            return true;
        }

        public string GetCityCDByZipCode(string zipCode1, string zipCode2)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = zipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = zipCode2.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_index_Select_Cities_by_ZipCode", sqlParams);
            if (dt.Rows.Count == 1)
            {
                var dr = dt.Rows[0];
                return dr["CityCD"].ToStringOrEmpty();
            }
            else
            {
                return "";
            }
        }

        public string GetTownCDByZipCode(string zipCode1, string zipCode2)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = zipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = zipCode2.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_index_Select_Towns_by_ZipCode", sqlParams);
            if (dt.Rows.Count == 1)
            {
                var dr = dt.Rows[0];
                return dr["TownCD"].ToStringOrEmpty();
            }
            else
            {
                return "";
            }
        }

        public DataTable GetMansionListByMansionWord(string prefCD, string searchWord)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = prefCD.ToStringOrNull() },
                new SqlParameter("@MansionWord", SqlDbType.VarChar){ Value = searchWord.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_index_Select_MansionList_by_MansionWord", sqlParams);
            return dt;
        }

        public DataTable GetMansionData(string mansionCD)
        {
            var sqlParam = new SqlParameter("@MansionCD", SqlDbType.VarChar) { Value = mansionCD.ToStringOrNull() };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_index_Select_MansionData", sqlParam);
            return dt;
        }

        public bool InsertSellerMansionData(a_indexModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = null },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = model.MansionCD.ToStringOrNull() },
                new SqlParameter("@LatestRequestDate", SqlDbType.DateTime){ Value = model.LatestRequestDate.ToDateTime() },
                new SqlParameter("@HoldingStatus", SqlDbType.TinyInt){ Value = model.HoldingStatus.ToByte(0) },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value = model.CityCD.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownCD", SqlDbType.VarChar){ Value = model.TownCD.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = model.TownName.ToStringOrNull() },
                new SqlParameter("@Address", SqlDbType.VarChar){ Value = model.Address.ToStringOrNull() },
                new SqlParameter("@StructuralKBN", SqlDbType.TinyInt){ Value = model.StructuralKBN.ToByte(0) },
                new SqlParameter("@Floors", SqlDbType.Int){ Value = model.Floors.ToInt32(0) },
                new SqlParameter("@ConstYYYYMM", SqlDbType.Int){ Value = model.ConstYYYYMM.Replace("/", "").ToInt32(0) },
                new SqlParameter("@Rooms", SqlDbType.Int){ Value = model.Rooms.ToInt32(0) },
                new SqlParameter("@LocationFloor", SqlDbType.Int){ Value = model.LocationFloor.ToInt32(0) },
                new SqlParameter("@RoomNumber", SqlDbType.VarChar){ Value = model.RoomNumber.ToStringOrNull() },
                new SqlParameter("@RoomArea", SqlDbType.Decimal){ Value = model.RoomArea.ToDecimal(0) },
                new SqlParameter("@BalconyKBN", SqlDbType.TinyInt){ Value = model.BalconyKBN.ToByte(0) },
                new SqlParameter("@BalconyArea", SqlDbType.Decimal){ Value = model.BalconyArea.ToDecimal(0) },
                new SqlParameter("@Direction", SqlDbType.TinyInt){ Value = model.Direction.ToByte(0) },
                new SqlParameter("@FloorType", SqlDbType.Int){ Value = model.FloorType.ToInt32(0) },
                new SqlParameter("@BathKBN", SqlDbType.TinyInt){ Value = model.BathKBN.ToByte(0) },
                new SqlParameter("@RightKBN", SqlDbType.TinyInt){ Value = model.RightKBN.ToByte(0) },
                new SqlParameter("@CurrentKBN", SqlDbType.TinyInt){ Value = model.CurrentKBN.ToByte(0) },
                new SqlParameter("@ManagementKBN", SqlDbType.TinyInt){ Value = model.ManagementKBN.ToByte(0) },
                new SqlParameter("@RentFee", SqlDbType.Money){ Value = model.RentFee.ToDecimal(0) },
                new SqlParameter("@ManagementFee", SqlDbType.Money){ Value = model.ManagementFee.ToDecimal(0) },
                new SqlParameter("@RepairFee", SqlDbType.Money){ Value = model.RepairFee.ToDecimal(0) },
                new SqlParameter("@ExtraFee", SqlDbType.Money){ Value = model.ExtraFee.ToDecimal(0) },
                new SqlParameter("@PropertyTax", SqlDbType.Money){ Value = model.PropertyTax.ToDecimal(0) },
                new SqlParameter("@DesiredTime", SqlDbType.TinyInt){ Value = model.DesiredTime.ToByte(0) },
                new SqlParameter("@Remark", SqlDbType.VarChar){ Value = model.Remark.ToStringOrNull() },
                new SqlParameter("@Longitude", SqlDbType.Decimal){ Value = model.Longitude.ToDecimal(0) },
                new SqlParameter("@Latitude", SqlDbType.Decimal){ Value = model.Latitude.ToDecimal(0) },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
                new SqlParameter("@MansionStationTable", SqlDbType.Structured) { TypeName = "dbo.T_MansionStation", Value = model.MansionStationList.ToDataTable() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_index_Insert_SellerMansionData", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }
    }
}
