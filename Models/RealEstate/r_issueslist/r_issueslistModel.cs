using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.RealEstate.r_issueslist
{
    public class r_issueslistModel : RealEstate_L_Log_Model
    {
        public byte chk_New { get; set; }
        public byte chk_Nego { get; set; }
        public byte chk_Contract { get; set; }
        public byte chk_SellerDeclined { get; set; }
        public byte chk_BuyerDeclined { get; set; }
        public string REStaffCD { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FreeWord { get; set; }
        public string AssReqID { get; set; }
        public string SellerID { get; set; }
    }
}
