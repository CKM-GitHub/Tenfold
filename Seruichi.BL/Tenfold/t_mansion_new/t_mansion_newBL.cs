using Models;
using Models.Tenfold.t_mansion_new;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_mansion_new
{
   public  class t_mansion_newBL
    {
        private CommonBL commonBL = new CommonBL();

        public Dictionary<string, string> ValidateAll(t_mansion_newModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //郵便番号
            validator.CheckIsHalfWidth("ZipCode1", model.ZipCode1, 3, RegexFormat.Number);
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
            validator.CheckSelectionRequired("StructuralKBN", model.StructuralKBN);
            ////築年月
            validator.CheckRequired("ConstYYYYMM", model.ConstYYYYMM);
            validator.CheckYMDate("ConstYYYYMM", model.ConstYYYYMM);
            ////総戸数
            validator.CheckRequiredNumber("Rooms", model.Rooms, true);
            validator.CheckIsNumeric("Rooms", model.Rooms, 3, 0);
            //階建て
            validator.CheckRequiredNumber("Floors", model.Floors, true);
            validator.CheckIsNumeric("Floors", model.Floors, 3, 0);

            validator.CheckSelectionRequired("RightKBN", model.RightKBN);

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

            ////M_Counter
            if (!commonBL.CheckExistsCounterMaster(CounterKey.MansionCD, out errorcd))
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
        public bool InsertSellerMansionData(t_mansion_newModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value = model.MansionName.ToStringOrNull() },
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = model.MansionCD.ToStringOrNull() },
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
                new SqlParameter("@MansionWord", SqlDbType.VarChar){ Value = model.Noti.ToStringOrNull() },
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
                return db.InsertUpdateDeleteData("pr_t_mansion_new_InsertMansionData", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }
         public (string, string) AddressSearch(string address)
        {
            string postUrl = "https://msearch.gsi.go.jp/address-search/AddressSearch?q=" + address;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    var response1 = reader.ReadToEnd();
                    if (response1 == "" || response1 == "[]")
                    {
                        return ("0", "0");
                    }
                    else
                    {
                        string[] xx = response1.Split(':');
                        string[] yy = xx[2].Split(',');
                        string longti = yy[0].Replace("[", "");
                        string lati = yy[1].Replace("]", "");
                        return (longti, lati);
                    }
                }
            }
            catch (Exception ex)
            {
                return ("0", "0");
            }
            finally
            {
                response.Close();
            }
        }
    }
}
