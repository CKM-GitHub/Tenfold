using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SendMailInfo
    {
        public enum SendTypes : int
        {
            None = 0,
            To = 1,
            Cc = 2,
            Bcc = 3
        }

        public class Recipient
        {
            public string MailAddress { get; set; }
            public SendMailInfo.SendTypes SendType { get; set; }
        }

        public string SenderAddress { get; set; }
        public string SenderServer { get; set; }
        public string SenderAccount { get; set; }
        public string SenderPassword { get; set; }

        public int Port { get; set; } = 587;

        public List<Recipient> Recipients { get; set; }

        public string Subject { get; set; }
        public string BodyText { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }

    }
}
