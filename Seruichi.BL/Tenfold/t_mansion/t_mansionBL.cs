using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_mansion
{
    public class t_mansionBL
    {
        public DataTable GetM_MansionData(string MansionCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@MansionCD", SqlDbType.Int){ Value = MansionCD }
             };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_Select_M_Mansion", sqlParams);
            return dt;
        }
    }
}
