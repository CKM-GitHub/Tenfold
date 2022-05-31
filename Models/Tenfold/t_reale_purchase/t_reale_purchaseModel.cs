using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_reale_purchase
{
    public class t_reale_purchaseModel
    {
        public string RealECD { get; set; }
        public string REName { get; set; }
        public byte chk_Purchase { get; set; }
        public byte chk_Checking { get; set; }
        public byte chk_Nego { get; set; }
        public byte chk_Contract { get; set; }
        public byte chk_SellerDeclined { get; set; }
        public byte chk_BuyerDeclined { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SellerCD { get; set; }
        public string SellerMansionID { get; set; }
    }

    public class t_reale_purchase_l_log_Model
    {
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
