using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_dashboard
{
    public class r_dashboardModel
    {
        public string RealECD { get; set; }
        public string REStaffCD { get; set; }
        public string REPassword { get; set; }
        public string REStaffName { get; set; }
        public byte PermissionChat { get; set; }
        public byte PermissionSetting { get; set; }
        public byte PermissionPlan { get; set; }
        public byte PermissionInvoice { get; set; }
    }
}
