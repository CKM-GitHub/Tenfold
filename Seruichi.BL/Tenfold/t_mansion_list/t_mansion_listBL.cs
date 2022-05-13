using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_mansion_list
{
    public class t_mansion_listBL
    {
        public DataTable GetM_Pref()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_get_m_pref", sqlParams);
            return dt;
        }
        public DataTable Get_Prefcd_and_CityGPCD()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_get_m_pref_and_citygpcd", sqlParams);
            return dt;
        }
        public DataTable Get_Prefcd_and_CityGPCD_and_CityCD()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_get_m_pref_and_citygpcd_and_citycd", sqlParams);
            return dt;
        }
    }
}
