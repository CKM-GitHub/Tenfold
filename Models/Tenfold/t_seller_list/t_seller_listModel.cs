using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_list
{
    public class t_seller_listModel
    {
       
        public string location { get; set; }
        public string MansionName { get; set; }
        public string Range { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public byte AccFlg { get; set; }
        public byte StatusFlg { get; set; }

    }
}
