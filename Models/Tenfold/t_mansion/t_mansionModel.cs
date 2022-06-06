using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_mansion
{
    public class t_mansionModel: BaseModel
    {
        public class MansionStation
        {
            public int RowNo { get; set; }
            public string LineCD { get; set; }
            public string StationCD { get; set; }
            public string Distance { get; set; }
        }
        public class MansionWord
        {
            public int RowNo { get; set; }
            public string WordSEQ { get; set; }
            public string Word { get; set; }
        }
        public string MansionCD { get; set; }
        public string MansionName { get; set; }
        public string PrefCD { get; set; }
        public string PrefName { get; set; }

        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public string TownCD { get; set; }
        public string TownName { get; set; }
        public string Address { get; set; }
        public int StructuralKBN { get; set; }
        public string ConstYYYYMM { get; set; }
        public string Rooms { get; set; }
        public string Floors { get; set; }
        public int RightKBN { get; set; }
        public string Remark { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
        public string Noti { get; set; }
        public string Katakana { get; set; }
        public string Katakana1 { get; set; }
        public string Hirakana { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string Other3 { get; set; }
        public string Other4 { get; set; }
        public string Other5 { get; set; }
        public string Other6 { get; set; }
        public List<MansionStation> MansionStationList { get; set; } = new List<MansionStation>();
        public string MansionStationListJson { get; set; }

        public List<MansionWord> MansionWordList { get; set; } = new List<MansionWord>();
        public string MansionWordListJson { get; set; }
    }
}
