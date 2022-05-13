using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_mansion_list
{
    public class t_mansion_listModel
    {
        public string Apartment { get; set; }
        public string StartAge { get; set; }
        public string EndAge { get; set; }
        public string StartUnit { get; set; }
        public string EndUnit { get; set; }
    }

    public class t_mansion_list_l_log_Model
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
        public string MansionCD { get; set; }
    }
}
