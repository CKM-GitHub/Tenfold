using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_assess_list
{
    public class t_assess_listModel
    {
        public byte Chk_Shinki { get; set; }
        public byte Chk_Kosho { get; set; }
        public byte Chk_Seiyaku { get; set; }
        public byte Chk_Urinushi { get; set; }
        public byte Chk_Kainushi { get; set; }
        public byte Chk_Hide { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class t_assess_list_l_log_Model
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
        public string LogRemarks { get; set; }
    }
}
