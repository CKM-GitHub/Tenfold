using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.a_indexModel;

namespace Models.Tenfold.t_mansion_new
{
   public class t_mansion_newModel
    {
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
        public byte StructuralKBN { get; set; }
        public string ConstYYYYMM { get; set; }
        public string Rooms { get; set; }

        public byte RightKBN { get; set; }
        public byte CurrentKBN { get; set; }
        public List<MansionStation> MansionStationList { get; set; } = new List<MansionStation>();

        public string MansionStationListJson { get; set; }
    }
}
