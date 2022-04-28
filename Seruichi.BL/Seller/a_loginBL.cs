using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Seruichi.BL
{
    public class a_loginBL
    {
        public Dictionary<string, string> ValidateLogin(string mailAddress, string password, out LoginUser user)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            if (string.IsNullOrEmpty(mailAddress))
            {
                validator.AddValidationResult("MailAddress", "E202"); //メールアドレスが入力されていません
            }

            if (string.IsNullOrEmpty(password))
            {
                validator.AddValidationResult("Password", "E205"); //パスワードが入力されていません
            }

            if (validator.IsValid)
            {
                string hashedPassword = new PasswordHash().GeneratePasswordHash(mailAddress, password);
                user = GetLoginUser(mailAddress, hashedPassword);
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

        private LoginUser GetLoginUser(string mailAddress, string password)
        {
            LoginUser user = new LoginUser();
            var sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = password.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_by_Password", sqlParams);
            if (dt.Rows.Count == 0)
            {
                return user;
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var row = dt.AsEnumerable().AsParallel().FirstOrDefault(r =>
                    crypt.DecryptFromBase64(r.Field<string>("MailAddress"), decryptionKey) == mailAddress);

            if (row != null)
            {
                user.UserID = row["SellerCD"].ToStringOrEmpty();
                user.UserName = crypt.DecryptFromBase64(row["SellerName"].ToStringOrEmpty(), decryptionKey);
            }

            return user;
        }

        public bool InsertLoginLog(LoginUser user, string ipaddress)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = user.UserID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = user.UserName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = ipaddress.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_login_Insert_L_Login", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }





        public Dictionary<string, string> ValidateTemporaryRegistration(string mailAddress)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            string elementId = "formTemporaryRegistration_MailAddress";

            validator.CheckSellerMailAddress(elementId, mailAddress);
            if (validator.IsValid && !CheckDuplicateMailAddresses(mailAddress))
            {
                validator.AddValidationResult(elementId, "E203"); //既に登録済みのメールアドレスです
            }

            return validator.GetValidationResult();
        }

        public bool CheckDuplicateMailAddresses(string mailAddress)
        {
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_All");
            if (dt.Rows.Count == 0)
            {
                return true;
            }

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            if (dt.AsEnumerable().AsParallel()
                .Any(r => crypt.DecryptFromBase64(r.Field<string>("MailAddress"), decryptionKey) == mailAddress))
            {
                return false;
            }

            return true;
        }

        public bool InsertCertificationData(string mailAddress, out string certificationCD, out DateTime effectiveDateTime)
        {
            certificationCD = new AESCryption().GenerateRandomDataBase64(12);
            effectiveDateTime = DateTime.MinValue;

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CertificationCD", SqlDbType.VarChar){ Value = certificationCD },
                new SqlParameter("@MailAddress", SqlDbType.VarChar){ Value = new AESCryption().EncryptToBase64(mailAddress, StaticCache.GetDataCryptionKey()) },
                new SqlParameter("@EffectiveDateTime", SqlDbType.DateTime){ Direction = ParameterDirection.Output },
            };

            try
            {
                DBAccess db = new DBAccess();
                if (db.InsertUpdateDeleteData("pr_a_login_Insert_D_Certification", false, sqlParams))
                {
                    effectiveDateTime = sqlParams[2].Value.ToDateTime(DateTime.MinValue);
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

        public SendMailInfo GetTemporaryRegistrationMailInfo(string mailAddress, string CertificationCD, DateTime effectiveDateTime)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.MemberRegistration, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.MemberRegistration, mailInfo);

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





        public Dictionary<string, string> ValidateVerifyMailAddress(string sellerKana, string birthday, string handyPhone,
            out string mailAddress)
        {
            mailAddress = "";

            ValidatorAllItems validator = new ValidatorAllItems();
            string elementId = "btnVerifyMailAddress";

            //生年月日
            validator.CheckBirthday("formVerifyMailAddress_Birthday", birthday);

            if (validator.IsValid && (string.IsNullOrEmpty(sellerKana) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(handyPhone)))
            {
                validator.AddValidationResult(elementId, "E207"); //この情報での登録はありません
            }

            if (validator.IsValid && !CheckExistsSeller_OnVerifyMailAddress(sellerKana, birthday, handyPhone, out mailAddress))
            {
                validator.AddValidationResult(elementId, "E207"); //この情報での登録はありません
            }

            return validator.GetValidationResult();
        }

        public bool CheckExistsSeller_OnVerifyMailAddress(string sellerKana, string birthday, string handyPhone,
            out string mailAddress)
        {
            mailAddress = "";

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_All");

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var row = dt.AsEnumerable().AsParallel().FirstOrDefault(r =>
                    crypt.DecryptFromBase64(r.Field<string>("SellerKana"), decryptionKey) == sellerKana
                    && crypt.DecryptFromBase64(r.Field<string>("HandyPhone"), decryptionKey) == handyPhone
                    && crypt.DecryptFromBase64(r.Field<string>("Birthday"), decryptionKey).ToDateTime(DateTime.MinValue) == birthday.ToDateTime(DateTime.MaxValue));

            if (row != null)
            {
                mailAddress = crypt.DecryptFromBase64(row["MailAddress"].ToStringOrEmpty(), decryptionKey);
                return true;
            }

            return false;
        }





        public Dictionary<string, string> ValidateResetPassword(string sellerKana, string birthday, string mailAddress,
            out string sellerCD, out string sellerName)
        {
            sellerCD = "";
            sellerName = "";

            ValidatorAllItems validator = new ValidatorAllItems();
            string elementId = "btnResetPassword";

            //生年月日
            validator.CheckBirthday("formResetPassword_Birthday", birthday);

            if (validator.IsValid && (string.IsNullOrEmpty(sellerKana) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(mailAddress)))
            {
                validator.AddValidationResult(elementId, "E207"); //この情報での登録はありません
            }

            if (validator.IsValid && !CheckExistsSeller_OnResetPassword(sellerKana, birthday, mailAddress, out sellerCD, out sellerName))
            {
                validator.AddValidationResult(elementId, "E207"); //この情報での登録はありません
            }

            return validator.GetValidationResult();
        }

        private bool CheckExistsSeller_OnResetPassword(string sellerKana, string birthday, string mailAddress, 
            out string sellerCD, out string sellerName)
        {
            sellerCD = "";
            sellerName = "";

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_login_Select_M_Seller_All");

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();

            var row = dt.AsEnumerable().AsParallel().FirstOrDefault(r =>
                    crypt.DecryptFromBase64(r.Field<string>("SellerKana"), decryptionKey) == sellerKana
                    && crypt.DecryptFromBase64(r.Field<string>("MailAddress"), decryptionKey) == mailAddress
                    && crypt.DecryptFromBase64(r.Field<string>("Birthday"), decryptionKey).ToDateTime(DateTime.MinValue) == birthday.ToDateTime(DateTime.MaxValue));

            if (row != null)
            {
                sellerCD = row["SellerCD"].ToStringOrEmpty();
                sellerName = crypt.DecryptFromBase64(row["SellerName"].ToStringOrEmpty(), decryptionKey);
                return true;
            }

            return false;
        }

        public bool UpdatePassword(string sellerCD, string sellerName, string IPAddress, string mailAddress, out string newPassword)
        {
            AESCryption crypt = new AESCryption();
            newPassword = crypt.GenerateRandomDataBase64(12);

            PasswordHash pwhash = new PasswordHash();
            string hashedPassword = pwhash.GeneratePasswordHash(mailAddress, newPassword);

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = hashedPassword },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = IPAddress },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = sellerName },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_login_Update_M_Seller_Password", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public SendMailInfo GetResetPasswordMailInfo(string mailAddress, string sellerName, string newPassword)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.ResetPassword, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.ResetPassword, mailInfo);

                DateTime sysDateTime = Utilities.GetSysDateTime();

                mailInfo.Recipients.Add(new SendMailInfo.Recipient()
                {
                    MailAddress = mailAddress,
                    SendType = SendMailInfo.SendTypes.To
                });

                mailInfo.BodyText = mailInfo.Text1.Replace("@@@@Name", sellerName)
                    .Replace("@@@@Time", sysDateTime.ToString("H時m分s秒"))
                    .Replace("@@@@Password", newPassword);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error(ex);
                mailInfo = null;
            }

            return mailInfo;
        }
    }
}
