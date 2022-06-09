using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_reale_account
{
    public class t_reale_accountModel
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
        public string AssReqID { get; set; }
        public string SellerCD { get; set; }
        public string SellerMansionID { get; set; }
        public byte Chk_Area { get; set; }
        public byte Chk_Mansion { set; get; }
        public byte Chk_SendCustomer { get; set; }
        public byte Chk_Top5 { get; set; }
        public byte Chk_Top5Out { get; set; }
        public byte Chk_NonMemberSeller { get; set; }
        public string Ispenalty { get; set; }
        public byte penaltyFlg { get; set; }
        public byte testFlg { get; set; }
        public string Memo { get; set; }
        public string TenStaffCD { get; set; }
        public string IPAddress { get; set; }


    }

    public class t_reale_asmhis_l_log_Model
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
