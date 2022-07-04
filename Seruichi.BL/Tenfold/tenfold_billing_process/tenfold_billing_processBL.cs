using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Seruichi.Common;
using System.Data;

namespace Seruichi.BL.Tenfold.tenfold_billing_process
{
    public class tenfold_billing_processBL
    {
        public DataTable Tenfold_Console_Process()
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
