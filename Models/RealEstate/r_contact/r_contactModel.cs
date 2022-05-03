using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_contact
{
    public class r_contactModel:BaseModel
    {
        public int ContactSEQ { get; set; }
        public DateTime ContactTime { get; set; }
        public string ContactName { get; set; }
        public string ContactKana { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public string ContactType { get; set; }
        public string ContactTypeCD { get; set; }
        public string ContactAssID { get; set; }
        public string ContactSubject { get; set; }
        public string ContactIssue { get; set; }
        public string REStaffName { get; set; }
    }
}
