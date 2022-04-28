using System.Collections.Generic;

namespace Models
{
    public class a_indexModel : BaseModel
    {
        public class MansionStation
        {
            public int RowNo { get; set; }
            public string LineCD { get; set; }
            public string StationCD { get; set; }
            public string Distance { get; set; }
        }

        public string SellerCD { get; set; }
        public string MansionName { get; set; }
        public string MansionCD { get; set; }
        public string LatestRequestDate { get; set; }
        public byte HoldingStatus { get; set; }
        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public string TownCD { get; set; }
        public string TownName { get; set; }
        public string Address { get; set; }
        public byte StructuralKBN { get; set; }
        public string Floors { get; set; }
        public string ConstYYYYMM { get; set; }
        public string Rooms { get; set; }
        public string LocationFloor { get; set; }
        public string RoomNumber { get; set; }
        public string RoomArea { get; set; }
        public byte BalconyKBN { get; set; }
        public string BalconyArea { get; set; }
        public byte Direction { get; set; }
        public string FloorType { get; set; }
        public byte BathKBN { get; set; }
        public byte RightKBN { get; set; }
        public byte CurrentKBN { get; set; }
        public byte ManagementKBN { get; set; }
        public string RentFee { get; set; }
        public string ManagementFee { get; set; }
        public string RepairFee { get; set; }
        public string ExtraFee { get; set; }
        public string PropertyTax { get; set; }
        public byte DesiredTime { get; set; }
        public string Remark { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string SellerName { get; set; }

        public List<MansionStation> MansionStationList { get; set; } = new List<MansionStation>();
        public string MansionStationListJson { get; set; }
    }
}
