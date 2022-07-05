using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.BL
{
    public class a_registerBL
    {
        public bool CheckAndUpdateCertification(string mailAddress, string certificationCD, out string errorcd)
        {
            errorcd = "";
            decimal dataSeq = 0;

            var dt = GetCertificationData(certificationCD);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E901";
                return false;
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var encryptedMailAddress = dr["MailAddress"].ToStringOrEmpty();
                if (crypt.DecryptFromBase64(encryptedMailAddress, decryptionKey) != mailAddress)
                {
                    errorcd = "E901";
                    return false;
                }

                if (!dr.IsNull("AccessDateTime"))
                {
                    errorcd = "E902";
                    return false;
                }

                if (dr["EffectiveDateTime"].ToDateTime(DateTime.MinValue) < Utilities.GetSysDateTime())
                {
                    errorcd = "E903";
                    return false;
                }

                if (!string.IsNullOrEmpty(dr["SellerCD"].ToStringOrEmpty()))
                {
                    errorcd = "E905";
                    return false;
                }

                dataSeq = dr["DataSEQ"].ToDecimal(0);
            }

            UpdateCertificationData(dataSeq);
            return true;
        }

        public DataTable GetCertificationData(string certificationCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CertificationCD", SqlDbType.VarChar){ Value = certificationCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            return db.SelectDatatable("pr_a_register_Select_D_Certification_by_CertificationCD", sqlParams);
        }

        public bool UpdateCertificationData(decimal dataSeq)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@DataSEQ", SqlDbType.Decimal){ Value = dataSeq },
            };

            try
            {
                DBAccess db = new DBAccess();
                return (db.InsertUpdateDeleteData("pr_register_Update_D_Certification", false, sqlParams));
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }





        public Dictionary<string, string> ValidateAll(a_registerModel model)
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
            if (!validator.IsContains("ZipCode1") && !validator.IsContains("ZipCode2"))
            {
                if (!string.IsNullOrEmpty(model.ZipCode1) || !string.IsNullOrEmpty(model.ZipCode2))
                {
                    if (!CheckZipCode(model.ZipCode1, model.ZipCode2, out string errorcd, out string prefCD, out string cityName, out string townName))
                    {
                        validator.AddValidationResult("ZipCode1", errorcd);
                    }
                }
            }
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
            //建物名
            validator.CheckIsDoubleByte("Address2", model.Address2, 100);
            //携帯番号
            validator.CheckRequired("HandyPhone", model.HandyPhone);
            validator.CheckIsHalfWidth("HandyPhone", model.HandyPhone, 15, RegexFormat.Number);
            //電話番号
            validator.CheckIsHalfWidth("HousePhone", model.HousePhone, 15, RegexFormat.Number);
            //FAX番号
            validator.CheckIsHalfWidth("Fax", model.Fax, 15, RegexFormat.Number);
            //メールアドレス
            validator.CheckSellerMailAddress("MailAddress", model.MailAddress);
            if (!validator.IsContains("MailAddress"))
            {
                if (!new a_loginBL().CheckDuplicateMailAddresses(model.MailAddress))
                {
                    validator.AddValidationResult("MailAddress", "E203"); //既に登録済みのメールアドレスです
                }
            }
            //パスワード
            validator.CheckRequired("Password", model.Password);
            validator.CheckIsHalfWidth("Password", model.Password, 20);
            if (!validator.IsContains("Password"))
            {
                if (model.Password.Length < 8)
                {
                    validator.AddValidationResult("Password", "E110");
                }
            }
            //パスワード（確認）
            validator.CheckRequired("ConfirmPassword", model.ConfirmPassword);
            if (!string.IsNullOrEmpty(model.ConfirmPassword) && model.Password != model.ConfirmPassword)
            {
                validator.AddValidationResult("ConfirmPassword", "E109");
            }

            return validator.GetValidationResult();
        }

        public bool CheckZipCode(string zipCode1, string zipCode2, out string errorcd, out string prefCD, out string cityName, out string townName)
        {
            errorcd = "";
            prefCD = "";
            cityName = "";
            townName = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = zipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = zipCode2.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_register_Select_M_Address_by_ZipCode", sqlParams);
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

        public bool InsertSellerData(a_registerModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(model.MailAddress, model.Password);

            //yyyy-MM-dd
            model.Birthday = model.Birthday.ToDateTime().ToDateString(DateTimeFormat.yyyy_MM_dd);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = null },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.MailAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = hashedPassword.ToStringOrNull() },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@SellerKana", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.SellerKana, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Birthday", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Birthday, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ZipCode1", SqlDbType.VarChar){ Value = model.ZipCode1.ToStringOrNull() },
                new SqlParameter("@ZipCode2", SqlDbType.VarChar){ Value = model.ZipCode2.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = model.PrefCD.ToStringOrNull() },
                new SqlParameter("@PrefName", SqlDbType.VarChar){ Value = model.PrefName.ToStringOrNull() },
                new SqlParameter("@CityName", SqlDbType.VarChar){ Value = model.CityName.ToStringOrNull() },
                new SqlParameter("@TownName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.TownName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Address1", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Address1, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Address2", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Address2, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HandyPhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HandyPhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@HousePhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.HousePhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Fax", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.Fax, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_register_Insert_M_Seller", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }
    }
}
