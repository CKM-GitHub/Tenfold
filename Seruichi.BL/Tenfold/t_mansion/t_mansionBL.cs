using Models.Tenfold.t_mansion;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Models;


namespace Seruichi.BL.Tenfold.t_mansion
{
    public class t_mansionBL
    {
        CommonBL commonBL = new CommonBL();
        public DataTable Get_M_Mansion_Data(string MansionCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = MansionCD.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_Select_M_Mansion", sqlParams);
            return dt;
        }

        public DataTable GetLineStationDistanceByMansionCD(string MansionCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = MansionCD.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_select_line_station_distance_by_mansioncd", sqlParams);
            return dt;
        }


        public DataTable GetMansionWordByMansionCD(string MansionCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = MansionCD.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_select_mansionword_by_mansioncd", sqlParams);
            return dt;            
        }

        public Dictionary<string, string> ValidateAll(t_mansionModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //郵便番号
            validator.CheckIsHalfWidth("ZipCode1", model.ZipCode1, 3,RegexFormat.Number);
            validator.CheckIsHalfWidth("ZipCode2", model.ZipCode2, 4, RegexFormat.Number);
            //都道府県
            validator.CheckSelectionRequired("PrefCD", model.PrefCD);
            ////マンション名
            validator.CheckRequired("MansionName", model.MansionName);
            validator.CheckIsDoubleByte("MansionName", model.MansionName, 100);
            ////市区町村
            validator.CheckSelectionRequired("CityCD", model.CityCD);
            ////町域
            validator.CheckSelectionRequired("TownCD", model.TownCD);
            ////住所
            validator.CheckRequired("Address", model.Address);
            validator.CheckIsDoubleByte("Address", model.Address, 50);
            ////交通アクセス
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
            ////建物構造
            validator.CheckSelectionRequired("StructuralKBN", model.StructuralKBN.ToString());
            ////築年月
            validator.CheckRequired("ConstYYYYMM", model.ConstYYYYMM);
            validator.CheckYMDate("ConstYYYYMM", model.ConstYYYYMM);
            ////総戸数
            validator.CheckRequiredNumber("Rooms", model.Rooms, true);
            validator.CheckIsNumeric("Rooms", model.Rooms, 3, 0);
            //階建て
            validator.CheckRequiredNumber("Floors", model.Floors, true);
            validator.CheckIsNumeric("Floors", model.Floors, 3, 0);

            validator.CheckSelectionRequired("RightKBN", model.RightKBN.ToString());

            validator.CheckRequired("Noti", model.Noti);
            validator.CheckIsDoubleByte("Noti", model.Noti, 50);

            validator.CheckRequired("Katakana", model.Katakana);
            validator.CheckIsDoubleByte("Katakana", model.Katakana, 50);

            validator.CheckRequired("Katakana1", model.Katakana1);
            validator.CheckIsHalfWidth("Katakana1", model.Katakana1, 50);

            validator.CheckRequired("Hirakana", model.Hirakana);
            validator.CheckIsDoubleByte("Hirakana", model.Hirakana, 50);


            validator.CheckByteCount("Remark", model.Remark, 1000);


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

            }

            string errorcd = "";

            ////M_Pref
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

        public bool UpdateMansionData(t_mansionModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = model.MansionCD.ToStringOrNull() },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },                
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
                new SqlParameter("@ConstYYYYMM", SqlDbType.Int){ Value = model.ConstYYYYMM.Replace("/", "").ToInt32(0) },
                new SqlParameter("@Rooms", SqlDbType.Int){ Value = model.Rooms.ToInt32(0) },
                new SqlParameter("@RightKBN", SqlDbType.TinyInt){ Value = model.RightKBN.ToByte(0) },
                new SqlParameter("@Floors", SqlDbType.Int){ Value = model.Floors.ToInt32(0) },
                new SqlParameter("@Remark", SqlDbType.VarChar){ Value = model.Remark.ToStringOrNull() },
                new SqlParameter("@Longitude", SqlDbType.Decimal){ Value = model.Longitude.ToDecimal(0) },
                new SqlParameter("@Latitude", SqlDbType.Decimal){ Value = model.Latitude.ToDecimal(0) },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
                new SqlParameter("@MansionStationTable", SqlDbType.Structured) { TypeName = "dbo.T_MansionStation", Value = model.MansionStationList.ToDataTable() },
                new SqlParameter("@MansionWordTable", SqlDbType.Structured) {TypeName = "dbo.T_MansionWord", Value = model.MansionWordList.ToDataTable() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_t_mansion_Update_M_Mansion", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }
    }
}
