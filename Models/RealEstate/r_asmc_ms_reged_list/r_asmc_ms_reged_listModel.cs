using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_asmc_ms_reged_list
{
    public class r_asmc_ms_reged_listModel : RealEstate_L_Log_Model
    {
        public string MansionName { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string CityCD { get; set; }
        public string Radio_Rating { get; set; }
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

    public class RatingModel
    {
       public string Rating { get; set; }
    }

}
