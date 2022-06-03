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
        public DataTable Get_M_Mansion_Data(string MansionCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = MansionCD.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_Select_M_Mansion", sqlParams);
            return dt;
        }
        public DataTable GetLineStationDistanceByMansionCD(string MansionCD,string PrefCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value = MansionCD.ToStringOrNull() },
                new SqlParameter("@PrefCD", SqlDbType.VarChar){ Value = PrefCD.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_select_line_by_mansioncd_and_prefcd", sqlParams);
            return dt;
        }
    }
}
