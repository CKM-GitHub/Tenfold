using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models;
using Models.Tenfold.t_seller_profile;

namespace Seruichi.BL.Tenfold.t_seller_profile
{
    public class t_seller_profileBL
    {
        public Dictionary<string, string> ValidateAll(t_seller_profileModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //名前
            validator.CheckRequired("SellerName", model.SellerName);
            validator.CheckIsDoubleByte("SellerName", model.SellerName, 50);
            //名前カナ
            validator.CheckRequired("SellerKana", model.SellerKana);
            validator.CheckIsDoubleByteKana("SellerKana", model.SellerKana, 50);
            //生年月日
            validator.CheckRequired("Birthday", model.Birthday);
            validator.CheckBirthday("Birthday", model.Birthday);
            //郵便番号
            validator.CheckIsHalfWidth("ZipCode1", model.ZipCode1, 3, RegexFormat.Number);
            validator.CheckIsHalfWidth("ZipCode2", model.ZipCode2, 4, RegexFormat.Number);
            //都道府県
            validator.CheckSelectionRequired("PrefCD", model.PrefCD);
            //市区町村
            validator.CheckRequired("CityName", model.CityName);
            validator.CheckIsDoubleByte("CityName", model.CityName, 100);
            //町域
            validator.CheckRequired("TownName", model.TownName);
            validator.CheckIsDoubleByte("TownName", model.TownName, 100);
            //番地
            validator.CheckRequired("Address1", model.Address1);
            validator.CheckIsDoubleByte("Address1", model.Address1, 100);
            //携帯番号
            validator.CheckRequired("HandyPhone", model.HandyPhone);
            validator.CheckIsHalfWidth("HandyPhone", model.HandyPhone, 15, RegexFormat.Number);
            //電話番号
            validator.CheckIsHalfWidth("HousePhone", model.HousePhone, 15, RegexFormat.Number);
            //FAX番号
            validator.CheckIsHalfWidth("Fax", model.Fax, 15, RegexFormat.Number);
            //メールアドレス
            validator.CheckSellerMailAddress("MailAddress", model.MailAddress);
            if (!CheckDuplicateMailAddresses(model.SellerCD, model.MailAddress))
            {
                validator.AddValidationResult("MailAddress", "E203"); //既に登録済みのメールアドレスです
            }

            if (validator.IsValid)
            {
                //郵便番号
                if (!string.IsNullOrEmpty(model.ZipCode1) || !string.IsNullOrEmpty(model.ZipCode2))
                {
                    if (!CheckZipCode(model, out string errorcd, out string prefCD, out string cityName, out string townName))
                    {
                        validator.AddValidationResult("ZipCode1", errorcd);
                    }
                }
            }

            return validator.GetValidationResult();
        }

        public bool CheckDuplicateMailAddresses(string sellerCD, string mailAddress)
        {
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_All");
            if (dt.Rows.Count == 0)
            {
                return true;
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            foreach(DataRow row in dt.Rows)
            {
                if (crypt.DecryptFromBase64(row.Field<string>("MailAddress"), decryptionKey) == mailAddress && row["SellerCD"].ToString() != sellerCD)
                    return false;
            }

            return true;
        }

        public Dictionary<string, string> ValidatePW(t_seller_profileModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //パスワード
            validator.CheckRequired("Password", model.Password);
            validator.CheckIsHalfWidth("Password", model.Password, 20);
            //パスワード（確認）
            validator.CheckRequired("ConfirmPassword", model.ConfirmPassword);
            if (!string.IsNullOrEmpty(model.ConfirmPassword) && model.Password != model.ConfirmPassword)
            {
                validator.AddValidationResult("ConfirmPassword", "E109");
            }

            if (validator.IsValid)
            {
                //パスワード
                if (model.Password.Length < 8 || model.Password.Length > 20)
                {
                    validator.AddValidationResult("Password", "E105");
                }
            }

            return validator.GetValidationResult();
        }

        public DataTable get_t_seller_Info(t_seller_profileModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@Type", SqlDbType.TinyInt){ Value = 1 }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_SellerInfo", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            foreach (DataRow row in dt.Rows)
            {
                row["SellerKana"] = crypt.DecryptFromBase64(row.Field<string>("SellerKana"), decryptionKey);
                row["SellerName"] = crypt.DecryptFromBase64(row.Field<string>("SellerName"), decryptionKey);
                row["TownName"] = crypt.DecryptFromBase64(row.Field<string>("TownName"), decryptionKey);
                row["Address1"] = crypt.DecryptFromBase64(row.Field<string>("Address1"), decryptionKey);
                row["Address2"] = crypt.DecryptFromBase64(row.Field<string>("Address2"), decryptionKey);
                row["HousePhone"] = crypt.DecryptFromBase64(row.Field<string>("HousePhone"), decryptionKey);
                row["HandyPhone"] = crypt.DecryptFromBase64(row.Field<string>("HandyPhone"), decryptionKey);
                row["MailAddress"] = crypt.DecryptFromBase64(row.Field<string>("MailAddress"), decryptionKey);
            }
            return dt;
        }

        public DataTable get_t_seller_profile_DisplayData(t_seller_profileModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_profile_getDisplayData", sqlParams);

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();
            foreach(DataRow row in dt.Rows)
            {
                row["SellerName"] = crypt.DecryptFromBase64(row.Field<string>("SellerName"), cryptionKey);
                row["SellerKana"] = crypt.DecryptFromBase64(row.Field<string>("SellerKana"), cryptionKey);
                row["Birthday"] = crypt.DecryptFromBase64(row.Field<string>("Birthday"), cryptionKey);
                row["Password"] = crypt.DecryptFromBase64(row.Field<string>("Password"), cryptionKey);
                row["TownName"] = crypt.DecryptFromBase64(row.Field<string>("TownName"), cryptionKey);
                row["Address1"] = crypt.DecryptFromBase64(row.Field<string>("Address1"), cryptionKey);
                row["HandyPhone"] = crypt.DecryptFromBase64(row.Field<string>("HandyPhone"), cryptionKey);
                row["HousePhone"] = crypt.DecryptFromBase64(row.Field<string>("HousePhone"), cryptionKey);
                row["Fax"] = crypt.DecryptFromBase64(row.Field<string>("Fax"), cryptionKey);
                row["MailAddress"] = crypt.DecryptFromBase64(row.Field<string>("MailAddress"), cryptionKey);
            }
            return dt;
        }

        public bool CheckZipCode(t_seller_profileModel model, out string errorcd, out string prefCD, out string cityName, out string townName)
        {
            errorcd = "";
            prefCD = "";
            cityName = "";
            townName = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_new_Select_M_Address_by_ZipCode", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E103"; //入力された値が正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            prefCD = dr["PrefCD"].ToStringOrEmpty();
            cityName = dr["CityName"].ToStringOrEmpty();
            if (dt.Rows.Count == 1)
            {
                townName = dr["TownName"].ToStringOrEmpty();
            }

            return true;
        }

        public void modify_SellerData(t_seller_profileModel model)
        {
            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@SellerKana", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerKana, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Birthday", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Birthday, cryptionKey).ToStringOrNull() },
                new SqlParameter("@MemoText", SqlDbType.VarChar){ Value = model.MemoText.ToStringOrNull() },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.TownName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Address1", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Address1, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HousePhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HousePhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HandyPhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HandyPhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Fax", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Fax, cryptionKey).ToStringOrNull() },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.MailAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
            };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_profile_modify_SellerData", false, sqlParams);
        }

        public void modify_SellerPW(t_seller_profileModel model)
        {
            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Password, cryptionKey).ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
            };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_profile_modify_SellerPW", false, sqlParams);
        }

    }
}
