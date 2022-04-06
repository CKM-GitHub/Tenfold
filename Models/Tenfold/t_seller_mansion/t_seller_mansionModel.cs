using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_mansion
{
    public class t_seller_mansionModel
    {
        public byte Chk_Blue { get; set; }
        public byte Chk_Sky { get; set; }
        public byte Chk_Orange { get; set; }
        public byte Chk_Green { get; set; }
        public byte Chk_Brown { get; set; }
        public byte Chk_Dark_Sky { get; set; }
        public byte Chk_Gray { get; set; }
        public byte Chk_Black { get; set; }
        public byte Chk_Pink { get; set; }
        public string MansionName { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //public Nullable<DateTime> StartDate { get; set; }
        //public Nullable<DateTime> EndDate { get; set; }

    }

    public class t_seller_mansion_l_log_Model
    {
         public byte LoginKBN { get; set; }
         public string LoginID { get; set; }
         public string RealECD { get; set; }
         public string LoginName { get; set; }
         public string IPAddress { get; set; }
         public string Page { get; set; }
         public string Processing { get; set; }
         public string Remarks { get; set; }
    }
   
}
