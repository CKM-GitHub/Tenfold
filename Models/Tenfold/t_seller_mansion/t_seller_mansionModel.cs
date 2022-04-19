using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_seller_mansion
{
    public class t_seller_mansionModel
    {
        public byte Chk_Mi { get; set; }
        public byte Chk_Kan { get; set; }
        public byte Chk_Satei { get; set; }
        public byte Chk_Kaitori { get; set; }
        public byte Chk_Kakunin { get; set; }
        public byte Chk_Kosho { get; set; }
        public byte Chk_Seiyaku { get; set; }
        public byte Chk_Urinushi { get; set; }
        public byte Chk_Kainushi { get; set; }
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
         public string LogId { get; set; }
        public string LogStatus { get; set; }
    }
   
}
