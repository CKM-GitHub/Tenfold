using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.BL
{
    public class a_mailregisterBL
    {
        public bool CheckAndUpdateCertification(string mailAddress, string certificationCD, out string errorcd, out string sellerCD)
        {
            errorcd = "";
            sellerCD = "";
            decimal dataSeq = 0;

            a_registerBL registerBL = new a_registerBL();
            var dt = registerBL.GetCertificationData(certificationCD);
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
                    errorcd = "E906";
                    return false;
                }

                if (dr["EffectiveDateTime"].ToDateTime(DateTime.MinValue) < Utilities.GetSysDateTime())
                {
                    errorcd = "E907";
                    return false;
                }

                if (string.IsNullOrEmpty(dr["SellerCD"].ToStringOrEmpty()))
                {
                    errorcd = "E904";
                    return false;
                }

                dataSeq = dr["DataSEQ"].ToDecimal(0);
                sellerCD = dr["SellerCD"].ToStringOrEmpty();
            }

            registerBL.UpdateCertificationData(dataSeq);
            return true;
        }

        public string GetMailAddress(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mailregister_Select_M_Seller_by_SellerCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var dr = dt.Rows[0];
            var encryptedMailAddress = dr["MailAddress"].ToStringOrEmpty();
            return crypt.DecryptFromBase64(encryptedMailAddress, decryptionKey);
        }




        public Dictionary<string, string> ValidateAll(a_mailregisterModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //メールアドレス
            validator.CheckSellerMailAddress("NewMailAddress", model.NewMailAddress);
            if (!new a_loginBL().CheckDuplicateMailAddresses(model.NewMailAddress))
            {
                validator.AddValidationResult("NewMailAddress", "E203"); //既に登録済みのメールアドレスです
            }

            //パスワード
            validator.CheckRequired("Password", model.Password);
            validator.CheckIsHalfWidth("Password", model.Password, 20);
            if (validator.IsValid 
                && !new a_mypage_uinfoBL().CheckPasswordMatch(model.SellerCD, model.MailAddress, model.Password))
            {
                validator.AddValidationResult("Password", "E207");
            }

            return validator.GetValidationResult();
        }

        public string GetSellerName(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mailregister_Select_M_Seller_by_SellerCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var dr = dt.Rows[0];
            var encryptedSellerName = dr["SellerName"].ToStringOrEmpty();
            return crypt.DecryptFromBase64(encryptedSellerName, decryptionKey);
        }

        public bool UpdateMailAddress(a_mailregisterModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(model.NewMailAddress, model.Password);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.NewMailAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = hashedPassword },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.SellerName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_mailregister_Update_M_Seller_MailAddress", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

    }
}
