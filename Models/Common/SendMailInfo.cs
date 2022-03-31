using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum SendTypeEnum : int
    {
        None = 0,
        To = 1,
        Cc = 2,
        Bcc = 3
    }

    public class Recipient
    {
        public string Address { get; set; }
        public SendTypeEnum SendType { get; set; }
    }

    public class SendMailInfo
    {
        public string SenderAddress { get; set; }
        public string SenderServer { get; set; }
        public string SenderAccount { get; set; }
        public string SenderPassword { get; set; }

        public List<Recipient> Recipients { get; set; }

        public string Subject { get; set; }
        public string BodyText { get; set; }

    }
}
