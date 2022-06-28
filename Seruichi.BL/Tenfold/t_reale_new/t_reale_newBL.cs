using Models;
using Models.Tenfold.t_reale_new;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_reale_new
{
    public class t_reale_newBL
    {
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
                return dr["CityName"].ToStringOrEmpty();
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
                return dr["TownName"].ToStringOrEmpty();
            }
            else
            {
                return "";
            }
        }

        public DataTable GetBankListByBankWord(string SearchWord)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SearchWord", SqlDbType.VarChar){ Value = SearchWord.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_new_Select_BankList_by_BankWord", sqlParams);
            return dt;
        }

        public DataTable GetBankBranchListByBankCD(string BankCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@BankCD", SqlDbType.VarChar){ Value = BankCD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_new_Select_BankBranchList_by_BankCD", sqlParams);
            return dt;
        }
        public DataTable GetBankBranchListByBranchWord(string SearchWord, string BankCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SearchWord", SqlDbType.VarChar){ Value = SearchWord.ToStringOrNull() },
                new SqlParameter("@BankCD", SqlDbType.VarChar){ Value = BankCD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_new_Select_BankBranchList_by_BranchWord", sqlParams);
            return dt;
        }

        public Dictionary<string, string> ValidateAll(t_reale_newModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckRequired("REName", model.REName);
            validator.CheckIsDoubleByte("REName", model.REName, 100);

            validator.CheckRequired("REKana", model.REKana);
            validator.CheckIsDoubleByte("REKana", model.REName, 100);

            validator.CheckRequired("President", model.President);
            validator.CheckIsDoubleByte("President", model.President, 50);

            
            validator.CheckIsHalfWidth("ZipCode1", model.ZipCode1, 3, RegexFormat.Number);
            validator.CheckIsHalfWidth("ZipCode2", model.ZipCode2, 4, RegexFormat.Number);
           
            validator.CheckSelectionRequired("PrefCD", model.PrefCD);
            
            validator.CheckRequired("CityName", model.CityName);
            validator.CheckMaxLenght("CityName", model.CityName, 50);            
            
            validator.CheckRequired("Address1", model.Address1);
            validator.CheckMaxLenght("Address1", model.Address1, 50);

            validator.CheckMaxLenght("BusinessHours", model.BusinessHours, 50);

            validator.CheckMaxLenght("CompanyHoliday", model.CompanyHoliday, 50);

            validator.CheckMaxLenght("PICName", model.PICName, 50);
            validator.CheckIsDoubleByte("PICName", model.PICName, 50);

            validator.CheckMaxLenght("PICKana", model.PICKana, 50);
            validator.CheckIsDoubleByte("PICKana", model.PICKana, 50);

            validator.CheckRequired("HousePhone", model.HousePhone);
            validator.CheckIsNumeric("HousePhone", model.HousePhone, 15,0);

            validator.CheckIsNumeric("Fax", model.Fax, 15, 0);

            validator.CheckRequired("MailAddress", model.MailAddress);
            validator.CheckIsHalfWidth("MailAddress", model.MailAddress, 100);
            validator.CheckSellerMailAddress("MailAddress", model.MailAddress);

            validator.CheckRequired("SourceBankName", model.SourceBankName);
            var dt = GetBankListByBankWord(model.SourceBankName);
            if(dt.Rows.Count == 0)
                validator.GetValidationResult().Add("SourceBankName", StaticCache.GetMessageText1("E305"));

            validator.CheckRequired("SourceBranchName", model.SourceBranchName);
            var dt1 = GetBankBranchListByBranchWord(model.SourceBranchName, model.SourceBankCD);
            if (dt1.Rows.Count == 0)
                validator.GetValidationResult().Add("SourceBranchName", StaticCache.GetMessageText1("E306"));

            validator.CheckSelectionRequired("SourceAccountType", model.SourceAccountType.ToString());
            validator.CheckMaxLenght("SourceAccountType", model.SourceAccountType.ToString(), 50);


            validator.CheckRequired("SourceAccount", model.SourceAccount);
            validator.CheckIsNumeric("SourceAccount", model.Fax, 8, 0);
           
            validator.CheckRequired("SourceAccountName", model.SourceAccountName);
            validator.CheckIsOnlyOneCharacter("SourceAccountName", model.SourceAccountName);
            validator.CheckMaxLenght("SourceAccountName", model.SourceAccountName.ToString(), 50);
            
            validator.CheckIsNumeric("LicenceNo2", model.LicenceNo2, 2, 0);

            validator.CheckIsNumeric("LicenceNo3", model.LicenceNo3, 10, 0);
           
            validator.CheckSelectionRequired("CourseCD", model.CourseCD.ToString());
          
            validator.CheckDate("JoinedDate", model.JoinedDate.ToString());
            
            validator.CheckRequired("InitialFee", model.InitialFee.ToString());
            validator.CheckIsNumeric("InitialFee", model.InitialFee.ToString(), 9, 0);

            validator.CheckByteCount("Remark", model.Remark, 1000);

            validator.CheckRequired("txtPassword", model.Password);
            validator.CheckIsOnlyOneCharacter("txtPassword", model.Password);
            validator.CheckMinLenght("txtPassword", model.Password, 8);
            validator.CheckMaxLenght("txtPassword", model.Password, 20);

            validator.CheckRequired("txtConfirmPassword", model.REPassword);
            validator.CheckComparePassword("txtConfirmPassword", model.Password, model.REPassword);

            if(model.Status == "update")
                validator.CheckSelectionRequired("NextCourseCD", model.NextCourseCD.ToString());

            ////M_Pref
            string errorcd = "";
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

        public bool Save_t_reale_new(t_reale_newModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@REName", SqlDbType.VarChar){ Value = model.REName.ToStringOrNull() },
                new SqlParameter("@REKana", SqlDbType.VarChar){ Value = model.REKana.ToStringOrNull() },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = model.TownName.ToStringOrNull() },
                new SqlParameter("@Address1", SqlDbType.VarChar){ Value = model.Address1.ToStringOrNull() },
                new SqlParameter("@HousePhone", SqlDbType.VarChar){ Value = model.HousePhone.ToStringOrNull() },
                new SqlParameter("@Fax", SqlDbType.VarChar){ Value = model.Fax.ToStringOrNull() },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = model.MailAddress.ToStringOrNull() },
                new SqlParameter("@President", SqlDbType.VarChar){ Value = model.President.ToStringOrNull() },
                new SqlParameter("@PICName", SqlDbType.VarChar){ Value = model.PICName.ToStringOrNull() },
                new SqlParameter("@PICKana", SqlDbType.VarChar){ Value = model.PICKana.ToStringOrNull() },
                new SqlParameter("@LicenceNo1", SqlDbType.VarChar){ Value = model.LicenceNo1Name.ToStringOrNull() },
                new SqlParameter("@LicenceNo2", SqlDbType.Int){ Value = model.LicenceNo2.ToInt32(0) },
                new SqlParameter("@LicenceNo3", SqlDbType.VarChar){ Value = model.LicenceNo3.ToStringOrNull() },
                new SqlParameter("@CompanyHoliday", SqlDbType.VarChar){ Value = model.CompanyHoliday.ToStringOrNull() },
                new SqlParameter("@BusinessHours", SqlDbType.VarChar){ Value = model.BusinessHours.ToStringOrNull() },
                new SqlParameter("@SourceBankCD", SqlDbType.VarChar){ Value = model.SourceBankCD.ToStringOrNull() },
                new SqlParameter("@SourceBranchCD", SqlDbType.VarChar){ Value = model.SourceBranchCD.ToStringOrNull() },
                new SqlParameter("@SourceAccountType", SqlDbType.TinyInt){ Value = model.SourceAccountType.ToByte(0) },
                new SqlParameter("@SourceAccount", SqlDbType.VarChar){ Value = model.SourceAccount.ToStringOrNull() },
                new SqlParameter("@SourceAccountName", SqlDbType.VarChar){ Value = model.SourceAccountName.ToStringOrNull() },
                new SqlParameter("@Remark", SqlDbType.VarChar){ Value = model.Remark.ToStringOrNull() },
                new SqlParameter("@JoinedDate", SqlDbType.VarChar){ Value = model.JoinedDate.ToStringOrNull() },
                new SqlParameter("@InitialFee", SqlDbType.Money){ Value = model.InitialFee.ToDecimal(0) },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull()},
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = model.Password.ToStringOrNull() },
                new SqlParameter("@CourseCD", SqlDbType.VarChar){ Value = model.CourseCD.ToStringOrNull() }
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_t_reale_new_Insert", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public DataTable Get_t_Reale_Profile_Data(string RealECD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = RealECD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_profile_Select_data_by_realecd", sqlParams);
            return dt;
        }
        public bool Update_t_reale_profile(t_reale_newModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@REName", SqlDbType.VarChar){ Value = model.REName.ToStringOrNull() },
                new SqlParameter("@REKana", SqlDbType.VarChar){ Value = model.REKana.ToStringOrNull() },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = model.TownName.ToStringOrNull() },
                new SqlParameter("@Address1", SqlDbType.VarChar){ Value = model.Address1.ToStringOrNull() },
                new SqlParameter("@HousePhone", SqlDbType.VarChar){ Value = model.HousePhone.ToStringOrNull() },
                new SqlParameter("@Fax", SqlDbType.VarChar){ Value = model.Fax.ToStringOrNull() },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = model.MailAddress.ToStringOrNull() },
                new SqlParameter("@President", SqlDbType.VarChar){ Value = model.President.ToStringOrNull() },
                new SqlParameter("@PICName", SqlDbType.VarChar){ Value = model.PICName.ToStringOrNull() },
                new SqlParameter("@PICKana", SqlDbType.VarChar){ Value = model.PICKana.ToStringOrNull() },
                new SqlParameter("@LicenceNo1", SqlDbType.VarChar){ Value = model.LicenceNo1Name.ToStringOrNull() },
                new SqlParameter("@LicenceNo2", SqlDbType.Int){ Value = model.LicenceNo2.ToInt32(0) },
                new SqlParameter("@LicenceNo3", SqlDbType.VarChar){ Value = model.LicenceNo3.ToStringOrNull() },
                new SqlParameter("@CompanyHoliday", SqlDbType.VarChar){ Value = model.CompanyHoliday.ToStringOrNull() },
                new SqlParameter("@BusinessHours", SqlDbType.VarChar){ Value = model.BusinessHours.ToStringOrNull() },
                new SqlParameter("@SourceBankCD", SqlDbType.VarChar){ Value = model.SourceBankCD.ToStringOrNull() },
                new SqlParameter("@SourceBranchCD", SqlDbType.VarChar){ Value = model.SourceBranchCD.ToStringOrNull() },
                new SqlParameter("@SourceAccountType", SqlDbType.TinyInt){ Value = model.SourceAccountType.ToByte(0) },
                new SqlParameter("@SourceAccount", SqlDbType.VarChar){ Value = model.SourceAccount.ToStringOrNull() },
                new SqlParameter("@SourceAccountName", SqlDbType.VarChar){ Value = model.SourceAccountName.ToStringOrNull() },
                new SqlParameter("@Remark", SqlDbType.VarChar){ Value = model.Remark.ToStringOrNull() },
                new SqlParameter("@JoinedDate", SqlDbType.VarChar){ Value = model.JoinedDate.ToStringOrNull() },
                new SqlParameter("@InitialFee", SqlDbType.Money){ Value = model.InitialFee.ToDecimal(0) },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull()},
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = model.Password.ToStringOrNull() },
                new SqlParameter("@CourseCD", SqlDbType.VarChar){ Value = model.CourseCD.ToStringOrNull() },
                new SqlParameter("@NextCourseCD", SqlDbType.VarChar){ Value = model.NextCourseCD.ToStringOrNull() }
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_t_reale_profile_Update", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }
        
    }
}
