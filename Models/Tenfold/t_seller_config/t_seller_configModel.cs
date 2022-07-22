using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_config
{
    public class t_seller_configModel
    {
        public string DataID { get; set; }
        public string SlotsNum { get; set; }
        public string Additionslots { get; set; }
        public string AmountBilled { get; set; }
        public string Reassessment { get; set; }
        public string Mode { get; set; }
        public string Num { get; set; }
    }

    public class l_log_Model
    {
        public string LogDateTime { get; set; }
        public byte LoginKBN { get; set; }
        public string LoginID { get; set; }
        public string RealECD { get; set; }
        public string LoginName { get; set; }
        public string IPAddress { get; set; }
        public string Page { get; set; }
        public string Processing { get; set; }
        public string Remarks { get; set; }
    }
}
