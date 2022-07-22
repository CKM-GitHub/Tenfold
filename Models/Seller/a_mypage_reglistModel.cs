using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Seller
{
  public  class a_mypage_reglistModel:BaseModel
    {
        
        public string AssReqID { get; set; }
        public string ReqDateTime { get; set; }
        public string LoginFLG { get; set; }
        public string AssKBN { get; set; }
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
        public string SellerMansionID { get; set; }
        public string MansionName { get; set; }
        //public string MansionCD { get; set; }
        //public string ZipCode1 { get; set; }
        //public string ZipCode2 { get; set; }
        //public string PrefCD { get; set; }
        //public string PrefName { get; set; }
        //public string CityCD { get; set; }
        //public string CityName { get; set; }
        //public string TownCD { get; set; }
        //public string TownName { get; set; }
        //public string Address { get; set; }
        //public byte StructuralKBN { get; set; }
        //public string Floors { get; set; }
        //public string ConstYYYYMM { get; set; }
        //public string Rooms { get; set; }
        //public string LocationFloor { get; set; }
        //public string RoomNumber { get; set; }
        //public string RoomArea { get; set; }
        //public byte BalconyKBN { get; set; }
        //public string BalconyArea { get; set; }
        //public byte Direction { get; set; }
        //public string FloorType { get; set; }
        //public byte BathKBN { get; set; }
        //public byte RightKBN { get; set; }
        //public byte CurrentKBN { get; set; }
        //public byte ManagementKBN { get; set; }
        //public string RentFee { get; set; }
        //public string ManagementFee { get; set; }
        //public string RepairFee { get; set; }
        //public string ExtraFee { get; set; }
        //public string PropertyTax { get; set; }
        //public byte DesiredTime { get; set; }
        //public string AuthenticateNo { get; set; }
        //public decimal AuthCIndexDateTime { get; set; }
        //public decimal AuthenticateDateTime { get; set; }

        public string EasyAssDateTime { get; set; }
        public string DeepAssDateTime { get; set; }
        public byte PurchReqDateTime { get; set; }
        public string ConfirmDateTime { get; set; }
        public decimal IntroDateTime { get; set; }

        public string REConfirmDateTime { get; set; }
        public string SellerTermDateTime { get; set; }
        public byte BuyerTermDateTime { get; set; }
        public string EndDateTime { get; set; }
        public decimal EndStatus { get; set; }
        public string EndStatusS { get; set; }
        public string EndStatusRE { get; set; }
        public byte AssessType1 { get; set; }
        public string AssessType2 { get; set; }
        public string ConditionSEQ { get; set; }
        public byte AssessAmount { get; set; }
        public string AssID { get; set; }
        public byte AssSEQ { get; set; }
        public string SimpleAssReqID { get; set; }
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
