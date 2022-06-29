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
        public void tenfold_daily_process_Insert_Update(string  model)
        {
            var sqlParams = new SqlParameter[]
               {
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model }
               };
            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_tenfold_daily_process_Insert_Update", false, sqlParams);
        }
    }
}
