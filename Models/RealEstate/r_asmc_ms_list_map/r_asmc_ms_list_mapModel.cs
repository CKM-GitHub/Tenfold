namespace Models.RealEstate.r_asmc_ms_list_map
{
    public class r_asmc_ms_list_mapModel
    {
        public string RealECD { get; set; }
        public string MansionName { get; set; }
        public string CityCDList { get; set; }
        public string TownCDList { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string StartDistance { get; set; }
        public string EndDistance { get; set; }
        public string StartRooms { get; set; }
        public string EndRooms { get; set; }
        public byte Unregistered { get; set; }
        public int Priority { get; set; }
    }

    public class M_Pref_CityGP_City_Town
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityGPCD { get; set; }
        public string CityGPName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public string TownCD { get; set; }
        public string TownName { get; set; }
        public byte CitySelected { get; set; }
        public byte TownSelected { get; set; }
    }
}
