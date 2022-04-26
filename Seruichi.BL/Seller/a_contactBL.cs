﻿using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.BL
{
    public class a_contactBL
    {
        public Dictionary<string, string> ValidateAll(a_contactModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();


            return validator.GetValidationResult();
        }

        public bool InsertContactData(a_contactModel model, out string msgid)
        {
            msgid = "";

            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = null },
                new SqlParameter("@ContactTime", SqlDbType.DateTime){ Value = model.ContactTime },
                new SqlParameter("@ContactName", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactName, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactKana", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactKana, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactAddress", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactAddress, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactPhone", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.ContactPhone, cryptionKey).ToStringOrNull() },
                new SqlParameter("@ContactType", SqlDbType.VarChar){ Value = model.ContactType.ToStringOrNull() },
                new SqlParameter("@ContactAssID", SqlDbType.VarChar){ Value = model.ContactAssID.ToStringOrNull() },
                new SqlParameter("@ContactSubject", SqlDbType.VarChar){ Value = model.ContactSubject.ToStringOrNull() },
                new SqlParameter("@ContactIssue", SqlDbType.VarChar){ Value = model.ContactIssue.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() },
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_contact_Insert_D_Contact", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public SendMailInfo GetContactTenfoldMailInfo(a_contactModel model)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.ContactTenfold, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.ContactTenfold, mailInfo);

                mailInfo.Recipients.Add(new SendMailInfo.Recipient()
                {
                    MailAddress = model.ContactAddress,
                    SendType = SendMailInfo.SendTypes.To
                });

                mailInfo.Subject = mailInfo.Subject.Replace("@@@@Title", model.ContactSubject);

                mailInfo.BodyText = mailInfo.Text1
                    .Replace("@@@@Time", model.ContactTime.ToString(DateTimeFormat.yyyyMdHmsJP))
                    .Replace("@@@@Name", model.ContactName)
                    .Replace("@@@@Kana", model.ContactKana)
                    .Replace("@@@@Address", model.ContactAddress)
                    .Replace("@@@@Phone", model.ContactPhone)
                    .Replace("@@@@AssessID", model.ContactAssID)
                    .Replace("@@@@Subject", model.ContactType)
                    + model.ContactIssue;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error(ex);
                mailInfo = null;
            }

            return mailInfo;
        }

        public SendMailInfo GetContactPersonMailInfo(a_contactModel model)
        {
            SendMailInfo mailInfo = new SendMailInfo();
            try
            {
                CommonBL cmnBL = new CommonBL();
                cmnBL.GetMailSender(mailInfo);
                cmnBL.GetMailRecipients(MailKBN.ContactPerson, mailInfo);
                cmnBL.GetMailTitleAndText(MailKBN.ContactPerson, mailInfo);

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
