using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Seruichi.BL
{
    public class a_mypage_uinfoBL
    {
        public a_mypage_uinfoModel GetSellerData(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_uinfo_Select_M_Seller_by_SellerCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return new a_mypage_uinfoModel() { SellerCD = sellerCD };
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var dr = dt.Rows[0];
            var model = new a_mypage_uinfoModel()
            {
                SellerCD = sellerCD,
                SellerName = crypt.DecryptFromBase64(dr["SellerName"].ToStringOrEmpty(), decryptionKey),
                SellerKana = crypt.DecryptFromBase64(dr["SellerKana"].ToStringOrEmpty(), decryptionKey),
                Birthday = crypt.DecryptFromBase64(dr["Birthday"].ToStringOrEmpty(), decryptionKey),
                ZipCode1 = dr["ZipCode1"].ToStringOrEmpty(),
                ZipCode2 = dr["ZipCode2"].ToStringOrEmpty(),
                PrefCD = dr["PrefCD"].ToStringOrEmpty(),
                PrefName = dr["PrefName"].ToStringOrEmpty(),
                CityName = crypt.DecryptFromBase64(dr["CityName"].ToStringOrEmpty(), decryptionKey),
                TownName = crypt.DecryptFromBase64(dr["TownName"].ToStringOrEmpty(), decryptionKey),
                Address1 = crypt.DecryptFromBase64(dr["Address1"].ToStringOrEmpty(), decryptionKey),
                Address2 = crypt.DecryptFromBase64(dr["Address2"].ToStringOrEmpty(), decryptionKey),
                HandyPhone = crypt.DecryptFromBase64(dr["HandyPhone"].ToStringOrEmpty(), decryptionKey),
                HousePhone = crypt.DecryptFromBase64(dr["HousePhone"].ToStringOrEmpty(), decryptionKey),
                Fax = crypt.DecryptFromBase64(dr["Fax"].ToStringOrEmpty(), decryptionKey),
                MailAddress = crypt.DecryptFromBase64(dr["MailAddress"].ToStringOrEmpty(), decryptionKey),
            };
            model.Birthday = model.Birthday.ToDateTime().ToDateString(DateTimeFormat.yyyyMMdd);

            return model;
        }

        public Dictionary<string, string> ValidateAll(a_mypage_uinfoModel model)
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
            //建物名
            validator.CheckIsDoubleByte("Address2", model.Address2, 100);
            //携帯番号
            validator.CheckRequired("HandyPhone", model.HandyPhone);
            validator.CheckIsHalfWidth("HandyPhone", model.HandyPhone, 15, RegexFormat.Number);
            //電話番号
            validator.CheckIsHalfWidth("HousePhone", model.HousePhone, 15, RegexFormat.Number);
            //FAX番号
            validator.CheckIsHalfWidth("Fax", model.Fax, 15, RegexFormat.Number);

            if (validator.IsValid)
            {
                //郵便番号
                if (!string.IsNullOrEmpty(model.ZipCode1) || !string.IsNullOrEmpty(model.ZipCode2))
                {
                    if (!CheckZipCode(model.ZipCode1, model.ZipCode2, out string errorcd, out string prefCD, out string cityName, out string townName))
                    {
                        validator.AddValidationResult("ZipCode1", errorcd);
                    }
                }
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
            var dt = db.SelectDatatable("pr_a_mypage_uinfo_Select_M_Address_by_ZipCode", sqlParams);
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

        public bool UpdateSellerData(a_mypage_uinfoModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            //yyyy-MM-dd
            model.Birthday = model.Birthday.ToDateTime().ToDateString(DateTimeFormat.yyyy_MM_dd);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
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
                return db.InsertUpdateDeleteData("pr_a_mypage_uinfo_Update_M_Seller", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }





        public Dictionary<string, string> ValidateChangeMailAddress(a_mypage_uinfo_emailModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            string elementId = "";

            //新しいメールアドレス
            elementId = "formChangeMailAddress_NewMailAddress";
            validator.CheckSellerMailAddress(elementId, model.NewMailAddress);
            if (validator.IsValid && !new a_loginBL().CheckDuplicateMailAddresses(model.NewMailAddress))
            {
                validator.AddValidationResult(elementId, "E203"); //既に登録済みのメールアドレスです
            }

            ////パスワード
            //elementId = "formChangeMailAddress_Password";
            //validator.CheckRequired(elementId, model.Password);
            //validator.CheckIsHalfWidth(elementId, model.Password, 20);
            //if (validator.IsValid && !CheckPasswordMatch(model.SellerCD, model.MailAddress, model.Password))
            //{
            //    validator.AddValidationResult(elementId, "E207");
            //}

            return validator.GetValidationResult();
        }

        public bool InsertCertificationData(a_mypage_uinfo_emailModel model, out string certificationCD, out DateTime effectiveDateTime)
        {
            certificationCD = new AESCryption().GenerateRandomDataBase64(12);
            effectiveDateTime = DateTime.MinValue;

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CertificationCD", SqlDbType.VarChar){ Value = certificationCD },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = new AESCryption().EncryptToBase64(model.NewMailAddress, StaticCache.GetDataCryptionKey()) },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@EffectiveDateTime", SqlDbType.DateTime){ Direction = ParameterDirection.Output },
            };

            try
            {
                DBAccess db = new DBAccess();
                if (db.InsertUpdateDeleteData("pr_a_mypage_uinfo_Insert_D_Certification", false, sqlParams))
                {
                    effectiveDateTime = sqlParams[3].Value.ToDateTime(DateTime.MinValue);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public SendMailInfo GetChangeMailAddressMailInfo(string mailAddress, string CertificationCD, DateTime effectiveDateTime)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.ChangeMailAddress, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.ChangeMailAddress, mailInfo);

                mailInfo.Recipients.Add(new SendMailInfo.Recipient()
                {
                    MailAddress = mailAddress,
                    SendType = SendMailInfo.SendTypes.To
                });

                string url = string.Format("{0}?mail={1}&setupid={2}",
                    mailInfo.Text2, HttpUtility.UrlEncode(mailAddress), HttpUtility.UrlEncode(CertificationCD));

                mailInfo.BodyText = mailInfo.Text1.Replace("@@@@MailAddress", mailAddress)
                    .Replace("@@@@URL", url)
                    .Replace("@@@@validity", effectiveDateTime.ToString(DateTimeFormat.yyyyMdHmsJP));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error(ex);
                mailInfo = null;
            }

            return mailInfo;
        }





        public Dictionary<string, string> ValidateChangePassword(a_mypage_uinfo_passwordModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            string elementId = "";

            //現在のパスワード
            elementId = "formChangePassword_Password";
            validator.CheckRequired(elementId, model.Password);
            validator.CheckIsHalfWidth(elementId, model.Password, 20);
            //新しいパスワード
            elementId = "formChangePassword_NewPassword";
            validator.CheckRequired(elementId, model.NewPassword);
            validator.CheckIsHalfWidth(elementId, model.NewPassword, 20);
            //新しいパスワード（確認）
            elementId = "formChangePassword_ConfirmNewPassword";
            validator.CheckRequired(elementId, model.ConfirmNewPassword);
            if (!string.IsNullOrEmpty(model.ConfirmNewPassword) && model.NewPassword != model.ConfirmNewPassword)
            {
                validator.AddValidationResult(elementId, "E109");
            }

            if (validator.IsValid)
            {
                //現在のパスワード
                if (!CheckPasswordMatch(model.SellerCD, model.MailAddress, model.Password))
                {
                    validator.AddValidationResult("formChangePassword_Password", "E207");
                }
                //新しいパスワード
                if (model.NewPassword.Length < 8 || model.NewPassword.Length > 20)
                {
                    validator.AddValidationResult("formChangePassword_NewPassword", "E105");
                }
            }

            return validator.GetValidationResult();
        }

        public bool UpdatePassword(a_mypage_uinfo_passwordModel model, out string msgid)
        {
            msgid = "";

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(model.MailAddress, model.NewPassword);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = hashedPassword.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_mypage_uinfo_Update_M_Seller_Password", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }





        public bool CheckPasswordMatch(string sellerCD, string mailAddress, string password)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_uinfo_Select_M_Seller_Password_by_SellerCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(mailAddress, password);

            var dr = dt.Rows[0];
            return dr["Password"].ToStringOrEmpty() == hashedPassword;
        }
    }
}
