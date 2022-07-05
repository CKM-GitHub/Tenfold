using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_temp_mes
{
    public class r_temp_mesModel : RealEstate_L_Log_Model
    {
        public string RealECD { get; set; }
        public string SEQ { get; set; }
        public string MessageSEQ { get; set; }
        public string MessageTitle { get; set; }
        public string MessageTEXT { get; set; }
        public string PermissionSetting { get; set; }
    }
}
