using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.t_assess_soudan
{
    public class t_assess_soudanModel
    {
        public string FreeWord { get; set; }
        public byte untreated { get; set; }
        public byte processing { get; set; }
        public byte solution { get; set; }
        public string Range { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string M_PIC { get; set; }
        public string type { get; set; }
        public string ConsultID { get; set; }
        public string SellerCD { get; set; }
        public string SellerMansionID { get; set; }
        public string ResponseSEQ { get; set; }
        public string comment { get; set; }

        //L_Log
        public byte LoginKBN { get; set; }
        public string LoginID { get; set; }
        public string RealECD { get; set; }
        public string LoginName { get; set; }
        public string IPAddress { get; set; }
        public string Page { get; set; }
        public string Process { get; set; }
        public string Remarks { get; set; }
    }
}
