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
                new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() },
                new SqlParameter("@StartYear", SqlDbType.Int){ Value = model.StartYear },
                new SqlParameter("@EndYear", SqlDbType.Int){ Value = model.EndYear },
                new SqlParameter("@Rating", SqlDbType.Int){ Value = model.Radio_Rating}
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_asmc_ms_reged_list_Select_M_RECondMan", sqlParams);

            return dt;
        }

        public void Insert_r_asmc_ms_reged_list_L_Log(r_asmc_ms_reged_listModel model)
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

        public void Update_M_RECondMan(r_asmc_ms_reged_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@MansionCD", SqlDbType.VarChar){ Value =  model.MansionCD.ToStringOrNull() },
             };
            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_asmc_ms_reged_list_Update_M_RECondMan", false, sqlParams);
        }
    }
}
