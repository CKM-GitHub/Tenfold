using System;
using System.Collections.Generic;

namespace Models.RealEstate.r_asmc_address
{
    public class r_asmc_address_Region
    {
        public string RegionCD { get; set; }
        public string RegionName { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class r_asmc_addressModel
    {
        public IEnumerable<r_asmc_address_Pref> Prefectures { get; set; } = new List<r_asmc_address_Pref>();
    }

    public class r_asmc_address_Pref
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public IEnumerable<r_asmc_address_CityGroup> CityGroups { get; set; } = new List<r_asmc_address_CityGroup>();
    }

    public class r_asmc_address_CityGroup
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityGroupCD { get; set; }
        public string CityGroupName { get; set; }
        public IEnumerable<r_asmc_address_City> Cities { get; set; } = new List<r_asmc_address_City>();
        public int CitiesCount { get; set; }
    }

    public class r_asmc_address_City
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityGroupCD { get; set; }
        public string CityGroupName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public int MansionCount { get; set; }
        public int RealEstateCount { get; set; }
        public int DisplayOrder { get; set; }
        public IEnumerable<r_asmc_address_Town> Towns { get; set; } = new List<r_asmc_address_Town>();
        public int TownsCount { get; set; }
        public int ValidFLG { get; set; }
        public int ExpirationFlag { get; set; }
    }

    public class r_asmc_address_Town
    {
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public string TownCD { get; set; }
        public string TownName { get; set; }
        public string TownKana { get; set; }
        public int MansionCount { get; set; }
        public int RealEstateCount { get; set; }
        public int DisplayOrder { get; set; }
        public int ValidFLG { get; set; }
        public int ExpirationFlag { get; set; }
    }

    public class r_asmc_addressDetailModel
    {
        public IEnumerable<r_asmc_address_City> Cities { get; set; } = new List<r_asmc_address_City>();
        public int Settings1 { get; set; }
        public int Settings2 { get; set; }
    }

}
