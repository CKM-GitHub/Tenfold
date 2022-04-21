using System.Collections.Generic;

namespace Models
{
    public class a_mypage_uinfoModel : BaseModel
    {
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
        public string SellerKana { get; set; }
        public string Birthday { get; set; }
        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityName { get; set; }
        public string TownName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string HandyPhone { get; set; }
        public string HousePhone { get; set; }
        public string Fax { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
