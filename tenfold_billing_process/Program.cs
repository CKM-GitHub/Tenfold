using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seruichi.BL.Tenfold.tenfold_billing_process;
using System.Data;
using Models.Tenfold.tenfold_billing_process;

namespace tenfold_billing_process
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tenfold_Billing_Process";
            tenfold_billing_processBL bl = new tenfold_billing_processBL();
            tenfold_billing_processModel model = new tenfold_billing_processModel();
            DataTable dt = new DataTable();
            dt = bl.Tenfold_Console_Process();
            if (dt.Rows.Count > 0)
            {
               model.BillingYYYYMM = dt.Rows[0]["BillingYYYYMM"].ToString();
            }
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            var TargetYYYYMM = year + month;

            if(model.BillingYYYYMM != TargetYYYYMM)
            {
                model.TargetDate = DateTime.Now.ToString();
            }
        }
    }
}
