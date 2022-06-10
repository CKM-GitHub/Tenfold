using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_admin
{
    public class t_adminModel:BaseModel
    {
        public string TenStaffCD { get; set; }
        public string TenStaffPW { get; set; }
        public string TenStaffName { get; set; }
        public string InvalidFLG { get; set; }         
        public string LoginName { get; set; }
        public string AdminPassword { get; set; }
        public string AdminConfirmPassword { get; set; }
        public List<Update_t_adminModel> lst_AdminModel { get; set; } = new List<Update_t_adminModel>(); 
        public string Processing { get; set; }
        public string Remark { get; set; }
    }
    public class Update_t_adminModel
    {
        public string TenStaffCD { get; set; }
        public string TenStaffPW { get; set; }
        public string TenStaffName { get; set; }
        public string InvalidFLG { get; set; }
        public string DeleteFLG { get; set; }
    }
}
