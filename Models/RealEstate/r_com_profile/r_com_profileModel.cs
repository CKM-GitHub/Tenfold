using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_com_profile
{
    public class r_com_profileModel : RealEstate_L_Log_Model
    {
        public byte PermissionChat { get; set; }
        public byte PermissionSetting { get; set; }
        public byte PermissionPlan { get; set; }
        public byte PermissionInvoice { get; set; }
    }
}
