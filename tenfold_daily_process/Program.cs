using System;
using System.Collections.Generic;
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
        static System.Net.IPHostEntry client = new System.Net.IPHostEntry();

        static void Main(string[] args)
        {
            Console.Title = "Tenfold_Dialy_Process";
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
            string IPaddress = "";
            foreach (NetworkInterface network in networks)
            {
                foreach (IPAddress entry in network.GetIPProperties().DhcpServerAddresses)
                {
                    IPaddress = entry.ToString();
                }
            }
            StaticCache.SetIniInfo();
            tbl.tenfold_daily_process_Insert_Update(IPaddress, 1);
            var dtMonth = tbl.tenfold_daily_process_Select_M_Monthly();

            string masterMonth = "";
            if (dtMonth.Rows.Count > 0)
            {
                masterMonth = dtMonth.Rows[0]["MasterYYYYMM"].ToString();
            }
            string thisMonth = DateTime.Now.ToString("yyyyMM");
            if (masterMonth == thisMonth)
            {
                tbl.tenfold_daily_process_Insert_Update(IPaddress, 2);
            }
        }
    }
}
