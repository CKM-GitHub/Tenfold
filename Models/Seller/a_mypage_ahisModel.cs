using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Seller
{
    public class a_mypage_ahisModel : BaseModel
    {
        public int No { get; set; }
        public string ID { get; set; }
        public string Staus { get; set; }
        public string MansionName { get; set; }
        public string Address { get; set; }
        public string REName { get; set; }
        public int AssessAmount { get; set; }
        public string DeepAssDateTime { get; set; }
        public string AssReqID { get; set; }
        public string SellerMansionID { get; set; }
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
    }
}
