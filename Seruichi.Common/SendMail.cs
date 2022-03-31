using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using System;
using MimeKit;

namespace Seruichi.Common
{
    public static class SendMail
    {
        public static async void SendSmtpAsync(SendMailInfo mailInfo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", mailInfo.SenderAddress));

            foreach (var recipient in mailInfo.Recipients)
            {
                if (recipient.SendType == SendTypeEnum.To)
                    message.To.Add(new MailboxAddress("", recipient.Address));

                if (recipient.SendType == SendTypeEnum.Cc)
                    message.Cc.Add(new MailboxAddress("", recipient.Address));

                if (recipient.SendType == SendTypeEnum.Bcc)
                    message.Bcc.Add(new MailboxAddress("", recipient.Address));
            }

            message.Subject = mailInfo.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = mailInfo.BodyText };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // SMTPサーバが暗号化に対応していないとき
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                try
                {
                    await client.ConnectAsync(mailInfo.SenderServer);
                    await client.AuthenticateAsync(mailInfo.SenderAccount, mailInfo.SenderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().Error(ex);
                }
            }
        }
    }
}
