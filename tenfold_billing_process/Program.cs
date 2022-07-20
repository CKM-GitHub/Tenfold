using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seruichi.BL.Tenfold.tenfold_billing_process;
using System.Data;
using System.Net;
using Seruichi.Common;

namespace tenfold_billing_process
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tenfold_Billing_Process";

            tenfold_billing_processBL bl = new tenfold_billing_processBL();
            string BillingYYYYMM = "";
            string IPaddress = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPaddress = ip.ToString();
                }
            }
            StaticCache.SetIniInfo();
            DataTable dt = new DataTable();
            dt = bl.Tenfold_Console_Process();
            if (dt.Rows.Count > 0)
            {
               BillingYYYYMM = dt.Rows[0]["BillingYYYYMM"].ToString();
            }
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            var TargetYYYYMM = year + month;

            if(BillingYYYYMM != TargetYYYYMM)
            {
               
                if (!bl.Tenfold_Billing_Process_Insert_Update(IPaddress))
                {
                    Console.WriteLine("Insert Update Unsuccess");
                    Console.ReadLine();
                }
            }
        }
    }
}
