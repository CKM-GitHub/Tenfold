using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Seruichi.Common;

namespace Seruichi.BL
{
    public class tenfold_daily_processBL
    {
       
        public void tenfold_daily_process_Insert_Update(string Ipaddress,int type)
        {
            
            var sqlParams = new SqlParameter[]
               {
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value =  Ipaddress}
               };
            DBAccess db = new DBAccess();
            if (type ==1)
            {
                db.InsertUpdateDeleteData("tenfold_daily_process_Update_M_RealEstate", false, sqlParams);
            }
            else
            {
                db.InsertUpdateDeleteData("pr_tenfold_daily_process_Insert_Update", false, sqlParams);
            }
           
        }

        public DataTable tenfold_daily_process_Select_M_Monthly()
        {

            var sqlParams = new SqlParameter[]
               {
                
               };
            DBAccess db = new DBAccess();

            var dt = db.SelectDatatable("pr_tenfold_console_process", sqlParams);
            return dt;
            
        }
    }
}
