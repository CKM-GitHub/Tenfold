using Models;
using Models.RealEstate.r_contact;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.RealEstate.r_contact
{
    public class r_contactBL
    {
        public Dictionary<string, string> ValidateAll(r_contactModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //名前
            validator.CheckRequired("ContactName", model.ContactName);
            validator.CheckIsDoubleByte("ContactName", model.ContactName, 50);
            //名前カナ
            validator.CheckRequired("ContactKana", model.ContactKana);
            validator.CheckIsDoubleByteKana("ContactKana", model.ContactKana, 50);
            //メールアドレス
            validator.CheckRequired("ContactAddress", model.ContactAddress);
            validator.CheckIsHalfWidth("ContactAddress", model.ContactAddress, 100);
            validator.CheckSellerMailAddress("ContactAddress", model.ContactAddress);
            //電話番号
            validator.CheckRequired("ContactPhone", model.ContactPhone);
            validator.CheckIsHalfWidth("ContactPhone", model.ContactPhone, 15, RegexFormat.Number);
            //お問い合わせ種類
            validator.CheckSelectionRequired("ContactTypeCD", model.ContactTypeCD);
            //査定ID
            validator.CheckIsHalfWidth("ContactAssID", model.ContactAssID, 10);
            //件名
            validator.CheckRequired("ContactSubject", model.ContactSubject);
            validator.CheckByteCount("ContactSubject", model.ContactSubject, 50);
            //お問い合わせ内容
            validator.CheckRequired("ContactIssue", model.ContactIssue);
            validator.CheckByteCount("ContactIssue", model.ContactIssue, 1000);

            return validator.GetValidationResult();
        }
        public bool InsertContactData(r_contactModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ContactTime", SqlDbType.DateTime){ Value = model.ContactTime },
                new SqlParameter("@ContactName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactKana", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactKana, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactPhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactPhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactType", SqlDbType.VarChar){ Value = model.ContactType.ToStringOrNull() },
                new SqlParameter("@ContactAssID", SqlDbType.VarChar){ Value = model.ContactAssID.ToStringOrNull() },
                new SqlParameter("@ContactSubject", SqlDbType.VarChar){ Value = model.ContactSubject.ToStringOrNull() },
                new SqlParameter("@ContactIssue", SqlDbType.VarChar){ Value = model.ContactIssue.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.REStaffName.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_contact_Insert_D_Contact", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public SendMailInfo GetContactTenfoldMailInfo(r_contactModel model)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.RealEstate_ContactTenfold, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.RealEstate_ContactTenfold, mailInfo);

                mailInfo.Subject = mailInfo.Subject.Replace("@@@@Title", model.ContactSubject);

                mailInfo.BodyText = mailInfo.Text1
                    .Replace("@@@@Time", model.ContactTime.ToString(DateTimeFormat.yyyyMdHmsJP))
                    .Replace("@@@@Name", model.ContactName)
                    .Replace("@@@@Kana", model.ContactKana)
                    .Replace("@@@@Address", model.ContactAddress)
                    .Replace("@@@@Phone", model.ContactPhone)
                    .Replace("@@@@AssessID", model.ContactAssID)
                    .Replace("@@@@Subject", model.ContactType)
                    + Environment.NewLine
                    + Environment.NewLine
                    + model.ContactIssue;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error(ex);
                mailInfo = null;
            }

            return mailInfo;
        }

        public SendMailInfo GetContactPersonMailInfo(r_contactModel model)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.RealEstate_ContactPerson, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.RealEstate_ContactPerson, mailInfo);

                mailInfo.Recipients.Add(new SendMailInfo.Recipient()
                {
                    MailAddress = model.ContactAddress,
                    SendType = SendMailInfo.SendTypes.To
                });

                mailInfo.BodyText = mailInfo.Text1
                    .Replace("@@@@Title", model.ContactSubject)
                    .Replace("@@@@Time", model.ContactTime.ToString(DateTimeFormat.yyyyMdHmsJP))
                    .Replace("@@@@Name", model.ContactName)
                    .Replace("@@@@Kana", model.ContactKana)
                    .Replace("@@@@Address", model.ContactAddress)
                    .Replace("@@@@Phone", model.ContactPhone)
                    .Replace("@@@@AssessID", model.ContactAssID)
                    .Replace("@@@@Subject", model.ContactType)
                    + Environment.NewLine
                    + Environment.NewLine
                    + model.ContactIssue;
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
