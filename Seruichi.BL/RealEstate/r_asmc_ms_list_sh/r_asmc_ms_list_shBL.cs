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

        public DataTable get_DisplayData(r_asmc_ms_list_shModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() },
                new SqlParameter("@MansionName", SqlDbType.VarChar){ Value =  model.MansionName.ToStringOrNull() },
                new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
                new SqlParameter("@StartYear", SqlDbType.Int){ Value = model.StartYear },
                new SqlParameter("@EndYear", SqlDbType.Int){ Value = model.EndYear },
                new SqlParameter("@StartDistance", SqlDbType.Int){ Value = model.StartDistance },
                new SqlParameter("@EndDistance", SqlDbType.Int){ Value = model.EndDistance },
                new SqlParameter("@StartRooms", SqlDbType.Int){ Value = model.StartRooms },
                new SqlParameter("@EndRooms", SqlDbType.Int){ Value = model.EndRooms },
                new SqlParameter("@Unregistered", SqlDbType.Int){ Value = model.Unregistered},
                new SqlParameter("@Priority", SqlDbType.Int){ Value = model.Priority},
                new SqlParameter("@Radio_Priority", SqlDbType.Int){ Value = model.Radio_Priority}
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_list_sh_get_DisplayData", sqlParams);

            return dt;
        }

        public void Insert_r_asmc_ms_list_sh_L_Log(r_asmc_ms_list_shModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_RealEstate_Insert_L_Log", false, sqlParams);
        }

    }
}
