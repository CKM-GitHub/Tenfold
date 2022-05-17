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
        public Dictionary<string, string> ValidateAll(r_asmc_ms_reged_listModel model)
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

        public DataTable Get_Rating(string RealECD)
        {
            var sqlParams = new SqlParameter[]
            { new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_select_ForRating_By_RealECD", sqlParams);
            return dt;
        }
        public DataTable Get_DataList(r_asmc_ms_reged_listModel model,string RealECD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value =  model.MansionName.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
                new SqlParameter("@StartYear", SqlDbType.Int){ Value = model.StartYear },
                new SqlParameter("@EndYear", SqlDbType.Int){ Value = model.EndYear },
                new SqlParameter("@Rating", SqlDbType.Int){ Value = model.Radio_Rating}
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_Select_M_RECondMan", sqlParams);

            return dt;
        }
    }
}
