using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Tenfold.t_mansion_list;
using Models;

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
       
        public async Task<DataTable> Generate_CSV1(t_mansion_listModel model)
        {
            return await Task.Run(() =>
            {
                var sqlParams = new SqlParameter[]
             {
             new SqlParameter("@StartAge", SqlDbType.Int) { Value = model.StartAge },
             new SqlParameter("@EndAge", SqlDbType.Int) { Value = model.EndAge },
             new SqlParameter("@StartUnit", SqlDbType.Int) { Value = model.StartUnit },
             new SqlParameter("@EndUnit", SqlDbType.Int) { Value = model.EndUnit },
             new SqlParameter("@Apartment", SqlDbType.VarChar) { Value = model.Apartment.ToStringOrNull() },
             new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
             new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() }
             };

                DBAccess db = new DBAccess();
                var dt = db.SelectDatatable("pr_t_mansion_list_csv1_generate", sqlParams);

                return dt;
            });
        }

        public DataTable Generate_CSV2(t_mansion_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@StartAge", SqlDbType.Int){ Value = model.StartAge },
                new SqlParameter("@EndAge", SqlDbType.Int){ Value = model.EndAge },
                new SqlParameter("@StartUnit", SqlDbType.Int){ Value = model.StartUnit},
                new SqlParameter("@EndUnit", SqlDbType.Int){ Value = model.EndUnit },
                new SqlParameter("@Apartment", SqlDbType.VarChar){ Value =  model.Apartment.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
                new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_csv2_generate", sqlParams);

            return dt;
        }

        public DataTable Generate_CSV3(t_mansion_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@StartAge", SqlDbType.Int){ Value = model.StartAge },
                new SqlParameter("@EndAge", SqlDbType.Int){ Value = model.EndAge },
                new SqlParameter("@StartUnit", SqlDbType.Int){ Value = model.StartUnit},
                new SqlParameter("@EndUnit", SqlDbType.Int){ Value = model.EndUnit },
                new SqlParameter("@Apartment", SqlDbType.VarChar){ Value =  model.Apartment.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
                new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_csv3_generate", sqlParams);

            return dt;
        }

        public void InsertM_Mansion_List_L_Log(t_mansion_list_l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_mansion_list_Insert_L_Log", false, sqlParams);
        }

        public Dictionary<string, string> ValidateAll(t_mansion_listModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckIsHalfWidth("StartNum", model.StartAge,2, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndNum", model.EndAge, 2, RegexFormat.Number); //E104,E105 

            validator.CheckIsHalfWidth("StartUnit", model.StartUnit, 3, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndUnit", model.EndUnit, 3, RegexFormat.Number); //E104,E105 

            if(!string.IsNullOrWhiteSpace(model.StartAge) && !string.IsNullOrWhiteSpace(model.EndAge))
            {
                validator.CheckCompareNum("EndNum", model.StartAge, model.EndAge);//E113
            }
            if (!string.IsNullOrWhiteSpace(model.StartUnit) && !string.IsNullOrWhiteSpace(model.EndUnit))
            {
                validator.CheckCompareNum("EndUnit", model.StartUnit, model.EndUnit);//E113
            }
               
            return validator.GetValidationResult();
        }


        public DataTable GetM_MansionList(t_mansion_listModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@StartAge", SqlDbType.Int){ Value = model.StartAge },
                new SqlParameter("@EndAge", SqlDbType.Int){ Value = model.EndAge },
                new SqlParameter("@StartUnit", SqlDbType.Int){ Value = model.StartUnit},
                new SqlParameter("@EndUnit", SqlDbType.Int){ Value = model.EndUnit },
                new SqlParameter("@Apartment", SqlDbType.VarChar){ Value =  model.Apartment.ToStringOrNull() },
                new SqlParameter("@CityCD", SqlDbType.VarChar){ Value =  model.CityCD.ToStringOrNull() },
                new SqlParameter("@CityGPCD", SqlDbType.VarChar){ Value =  model.CityGPCD.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_Select_M_Mansion", sqlParams);

            return dt;
        }
    }
}
