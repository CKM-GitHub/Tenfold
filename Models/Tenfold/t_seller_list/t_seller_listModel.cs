using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_list
{
    public class t_seller_listModel
    {
       
        public string PrefNameSelect { get; set; }
        public string SellerName { get; set; }
        public string RangeSelect { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public byte ValidCheck { get; set; }
        public byte InValidCheck { get; set; }
        public byte expectedCheck { get; set; }
        public byte negtiatioinsCheck { get; set; }
        public byte endCheck { get; set; }

    }
}
