using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;

namespace Seruichi.BL.RealEstate.r_auto_mes
{
    public class r_auto_mesBL
    {
        
        public DataTable Get_M_REMessage_By_RealECD(string RealECD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD",SqlDbType.VarChar) { Value = RealECD },
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_auto_mes_Select_M_REMessage_By_RealECD", sqlParams);
            return dt;
        }
    }
}
