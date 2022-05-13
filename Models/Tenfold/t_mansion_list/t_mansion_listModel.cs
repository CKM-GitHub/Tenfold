using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_mansion_list
{
    public class t_mansion_listModel
    {
         
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
}
