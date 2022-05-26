using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Seller
{
    public class a_assess_dModel :BaseModel
    {
        public string SellerCD { get; set; }
        public string AssReqID { get; set; }
        public string AssID { get; set; }
        public string AssSEQ { get; set; }
        public string ProgressKBN { get; set; }
        public string RealECD { get; set; }
        public string SellerMansionID { get; set; }
        public string AssessAmount { get; set; }
        //public string IpAddress { get; set; }
        public string SellerName{ get; set; }
        public string AssessType1{ get; set; }
        public string AssessType2{ get; set; }
        public string ConditionSEQ{ get; set; } 
    }
    public class a_assess_dModel_l_log_Model
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
        public string SellerCD { get; set; }
        public string Link { get; set; }
    }
}
