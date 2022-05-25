using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_asmc_ms_list_sh
{
    public class r_asmc_ms_list_shModel : RealEstate_L_Log_Model
    {
        public string MansionName { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string StartDistance { get; set; }
        public string EndDistance { get; set; }
        public string StartRooms { get; set; }
        public string EndRooms { get; set; }
        public string CityGPCD { get; set; }
        public string CityCD { get; set; }
        public string Unregistered { get; set; }
        public string Priority { get; set; }
        public string Radio_Priority { get; set; }
        public string MansionCD { get; set; }
    }

    public class M_Pref
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
    }

    public class M_Pref_And_CityGPCD
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityGPCD { get; set; }
        public string CityGPName { get; set; }
        public string AddressLevel { get; set; }
    }
    public class M_Pref_And_CityGPCD_And_CityCD
    {
        public string PrefCD { get; set; }
        public string PrefName { get; set; }
        public string CityGPCD { get; set; }
        public string CityGPName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
    }
}
