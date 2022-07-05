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
       
        public bool Tenfold_Daily_Process_Insert_Update(string Ipaddress,int type)
        {
            
            var sqlParams = new SqlParameter[]
               {
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value =  Ipaddress}
               };
           
            try
            {
                DBAccess db = new DBAccess();
                if (type == 1)
                {
                    return db.InsertUpdateDeleteData("pr_tenfold_daily_process_Update_M_RealEstate", false, sqlParams);
                }
                else
                {
                    return db.InsertUpdateDeleteData("pr_tenfold_daily_process_Insert_Update", false, sqlParams);
                }
                 
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public DataTable Tenfold_Daily_Process_Select_M_Monthly()
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
