using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_asmc_ms_list_sh;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Models;

namespace Seruichi.BL.RealEstate.r_asmc_ms_list_sh
{
    public class r_asmc_ms_list_shBL
    {
        public Dictionary<string, string> ValidateAll(r_asmc_ms_list_shModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();
            validator.CheckIsHalfWidth("StartYear", model.StartYear, 15, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndYear", model.EndYear, 15, RegexFormat.Number); //E104,E105 

            validator.CheckCompareNum("EndYear", model.StartYear, model.EndYear);//E113
            return validator.GetValidationResult();
        }

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
    }
}
