using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_login
{
    public class r_loginModel
    {
        public string RealECD { get; set; }
        public string REStaffCD { get; set; }
        public string REPassword { get; set; }
    }
    public class r_login_l_log_Model
    {
        public string LogDateTime { get; set; }
        public byte LoginKBN { get; set; }
        public string LoginID { get; set; }
        public string RealECD { get; set; }
        public string LoginName { get; set; }
        public string IPAddress { get; set; }
        public string PageID { get; set; }
        public string ProcessKBN { get; set; }
        public string Remarks { get; set; }
    }
}
