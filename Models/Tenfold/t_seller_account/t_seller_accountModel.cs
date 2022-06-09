using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_account
{


    public class t_seller_accountModel
    {

        public string SellerName { get; set; }
        public string InvalidFLG { get; set; }
        public string SellerKana { get; set; }
        public string SellerCD { get; set; }
        public string Address { get; set; }
        public string TownName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Mobile_Phone { get; set; }
        public string MailAddress { get; set; }
        public string BillingAmt { get; set; }
        public int Assessment_constant { get; set; }
        public int Num_contracts { get; set; }
        public int Num_seller_declines { get; set; }
        public int Num_buyer_declines { get; set; }
        public string InsertDateTime { get; set; }
        public string DeepAssDateTime { get; set; }
        public string PurchReqDateTime { get; set; }
        public string LoginDateTime { get; set; }
        public byte Chk_Mi { get; set; }
        public byte Chk_Kan { get; set; }
        public byte Chk_Satei { get; set; }
        public byte Chk_Kaitori { get; set; }
        public byte Chk_Kakunin { get; set; }
        public byte Chk_Kosho { get; set; }
        public byte Chk_Seiyaku { get; set; }
        public byte Chk_Urinushi { get; set; }
        public byte Chk_Kainushi { get; set; }
        public byte Chk_Sakujo { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }



    

}
