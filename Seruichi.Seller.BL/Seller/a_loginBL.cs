using Models;
using Seruichi.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.BL
{
    public class a_loginBL
    {
        public Dictionary<string, string> ValidateLogin(string mailAddress, string password, out LoginUser user)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            if (string.IsNullOrEmpty(mailAddress))
            {
                validator.AddValidationResult("UserID", "E202"); //メールアドレスが入力されていません
            }
            if (string.IsNullOrEmpty(password))
            {
                validator.AddValidationResult("Password", "E205"); //パスワードが入力されていません
            }

            if (validator.IsValid)
            {
                user = GetSellerLoginUser(mailAddress, password);
                if (string.IsNullOrEmpty(user.UserID))
                {
                    validator.AddValidationResult("Password", "E206"); //メールアドレスとパスワードの組合せが正しくありません
                }
            }
            else
            {
                user = new LoginUser();
            }
            
            return validator.GetValidationResult();
        }

        public LoginUser GetSellerLoginUser(string mailAddress, string password)
        {
            LoginUser user = new LoginUser();
            var sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = password.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_SellerByPassword", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return user;
            }

            AESCryption crypt = new AESCryption();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var encryptedMailAddress = dr["MailAddress"].ToStringOrEmpty();

                if (string.IsNullOrEmpty(encryptedMailAddress))
                {
                    return user;
                }

                if (crypt.DecryptFromBase64(encryptedMailAddress, StaticCache.GetDataCryptionKey()) == mailAddress)
                {
                    user.UserID = dr["SellerCD"].ToStringOrEmpty();
                    break;
                }
            }

            return user;
        }

        public bool CheckMailAddressAlreadyExists(string mailAddress, out string errorcd)
        {
            errorcd = "";

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_AllMailAddress");
            if (dt.Rows.Count == 0)
            {
                return true;
            }

            AESCryption crypt = new AESCryption();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var encryptedMailAddress = dr["MailAddress"].ToStringOrEmpty();

                if (!string.IsNullOrEmpty(encryptedMailAddress))
                {
                    if (crypt.DecryptFromBase64(encryptedMailAddress, StaticCache.GetDataCryptionKey()) == mailAddress)
                    {
                        errorcd = "E203";
                        return false;
                    }
                }
            }

            return true;
        }

        public bool InsertCertificationData(string mailAddress, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CertificationCD", SqlDbType.VarChar){ Value = new AESCryption().GenerateRandomDataBase64(12) },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = mailAddress },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_login_Insert_D_Certification", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public SendMailInfo GetTemporaryRegistrationMail()
        {
            SendMailInfo mailInfo = new SendMailInfo();

            return mailInfo;
        }
    }
}
