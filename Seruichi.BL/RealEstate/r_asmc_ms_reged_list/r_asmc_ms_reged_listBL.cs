using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.RealEstate.r_asmc_ms_reged_list;
using Models;

namespace Seruichi.BL.RealEstate.r_asmc_ms_reged_list
{
    public class r_asmc_ms_reged_listBL
    {

        public DataTable GetM_Pref()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_get_m_pref", sqlParams);
            return dt;
        }
        public DataTable Get_Prefcd_and_CityGPCD()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_get_m_pref_and_citygpcd", sqlParams);
            return dt;
        }

        public DataTable Get_Prefcd_and_CityGPCD_and_CityCD()
        {
            var sqlParams = new SqlParameter[]
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_get_m_pref_and_citygpcd_and_citycd", sqlParams);
            return dt;
        }

        public DataTable Get_Rating(string RealECD)
        {
            var sqlParams = new SqlParameter[]
            { new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_select_ForRating_By_RealECD", sqlParams);
            return dt;
        }
    }
}
