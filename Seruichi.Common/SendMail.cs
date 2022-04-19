using MimeKit;
using Models;
using System;

namespace Seruichi.Common
{
    public static class SendMail
    {
        public static void SendSmtp(SendMailInfo mailInfo)
        {
            if (mailInfo == null) return;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", mailInfo.SenderAddress));

            foreach (var recipient in mailInfo.Recipients)
            {
                if (recipient.SendType == SendMailInfo.SendTypes.To)
                    message.To.Add(new MailboxAddress("", recipient.MailAddress));

                if (recipient.SendType == SendMailInfo.SendTypes.Cc)
                    message.Cc.Add(new MailboxAddress("", recipient.MailAddress));

                if (recipient.SendType == SendMailInfo.SendTypes.Bcc)
                    message.Bcc.Add(new MailboxAddress("", recipient.MailAddress));
            }

            message.Subject = mailInfo.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = mailInfo.BodyText };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // SMTPサーバが暗号化に対応していないとき
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                try
                {
                    client.Connect(mailInfo.SenderServer, mailInfo.Port, MailKit.Security.SecureSocketOptions.Auto);
                    client.Authenticate(mailInfo.SenderAccount, mailInfo.SenderPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().Error(ex);
                }
            }
        }
    }
}
