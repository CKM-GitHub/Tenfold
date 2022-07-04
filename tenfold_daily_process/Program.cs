using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Seruichi.BL;
using Seruichi.Common;

namespace tenfold_daily_process
{
    class Program
    {
        static tenfold_daily_processBL tbl = new tenfold_daily_processBL();

        static void Main(string[] args)
        {
            Console.Title = "Tenfold_Dialy_Process";
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
            if(!tbl.Tenfold_Daily_Process_Insert_Update(IPaddress, 1))
            {
                Console.WriteLine("Daily Update Unsuccess");
                Console.ReadLine();
            }
            DataTable dtMonth = tbl.Tenfold_Daily_Process_Select_M_Monthly();
            if (dtMonth.Rows.Count > 0)
            {
                if (dtMonth.Rows[0]["MasterYYYYMM"].ToString() != DateTime.Now.ToString("yyyyMM"))
                {
                    if(!tbl.Tenfold_Daily_Process_Insert_Update(IPaddress, 2))
                    {
                        Console.WriteLine("Monthly Update Unsuccess");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
