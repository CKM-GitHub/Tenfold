using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_staff
{
    public class r_staffModel:RealEstate_L_Log_Model
    {
       
        public string REFaceImage { get; set; }
        public string REStaffCD { get; set; }
        public string REStaffName { get; set; }
        public string REIntroduction { get; set; }
        public string REPassword { get; set; }
        public string REStaffEmail { get; set; }
        public string PermissionChat { get; set; }
        public string PermissionSetting { get; set; }
        public string PermissionPlan { get; set; }
        public string PermissionInvoice { get; set; }
        public List<Update_r_staffModel> lst_StaffModel { get; set; } = new List<Update_r_staffModel>();
    }

    public class Update_r_staffModel
    {
        public string RealECD { get; set; }
        public string REFaceImage { get; set; }
        public string REStaffCD { get; set; }
        public string REStaffName { get; set; }
        public string REIntroduction { get; set; }
        public string REPassword { get; set; }
        public string REStaffEmail { get; set; }
        public string PermissionChat { get; set; }
        public string PermissionSetting { get; set; }
        public string PermissionPlan { get; set; }
        public string PermissionInvoice { get; set; }
    }


}
