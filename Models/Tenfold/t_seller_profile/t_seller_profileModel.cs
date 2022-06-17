using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_profile
{
    public class t_seller_profileModel
    {
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
        public string SellerKana { get; set; }
        public string Birthday { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string MemoText { get; set; }
        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityName { get; set; }
        public string TownName { get; set; }
        public string Address1 { get; set; }
        public string HandyPhone { get; set; }
        public string HousePhone { get; set; }
        public string Fax { get; set; }
        public string MailAddress { get; set; }

        //L_Log
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
