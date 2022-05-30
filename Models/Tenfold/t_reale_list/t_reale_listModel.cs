using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_reale_list
{
    public class t_reale_listModel
    {
        public string PrefNameSelect { get; set; }
        public string RealEStateCom { get; set; }
        public byte EffectiveChk { get; set; }
        public byte InValidCheck { get; set; }
    }

    public class t_reale_l_log_Model
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
        public string RealeCD { get; set; }
    }
}
