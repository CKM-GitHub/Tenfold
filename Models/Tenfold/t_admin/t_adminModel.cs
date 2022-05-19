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
    }
}
