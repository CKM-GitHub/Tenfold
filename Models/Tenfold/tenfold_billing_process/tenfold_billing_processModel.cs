using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tenfold.tenfold_billing_process
{
    public class tenfold_billing_processModel:BaseModel
    {
        public string BillingYYYYMM { get; set; }
        public string TargetDate { get; set; }
    }
}
