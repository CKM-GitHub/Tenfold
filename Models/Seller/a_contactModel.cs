using System;

namespace Models
{
    public class a_contactModel : BaseModel
    {
        public int ContactSEQ { get; set; }
        public DateTime ContactTime { get; set; }
        public string ContactName { get; set; }
        public string ContactKana { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public string ContactType { get; set; }
        public string ContactAssID { get; set; }
        public string ContactSubject { get; set; }
        public string ContactIssue { get; set; }
    }
}
