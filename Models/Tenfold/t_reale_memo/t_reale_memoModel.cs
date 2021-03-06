using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_reale_memo
{
    public class t_reale_memoModel
    {
        public string RealECD { get; set; }
        public string REMemoSEQ { get; set; }
        public string ParentSEQ { get; set; }
        public string ParentChildKBN { get; set; }
        public string MemoText { get; set; }
        public string Type { get; set; }
        public byte LoginKBN { get; set; }
        public string LoginID { get; set; }
        public string LoginName { get; set; }
        public string IPAddress { get; set; }
        public string Page { get; set; }
        public string Processing { get; set; }
        public string Remarks { get; set; }
    }
}
