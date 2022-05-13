using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seruichi.Common;
using Models.Tenfold.t_mansion_list;
using System.Data;
using System.Data.SqlClient;
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
        public Dictionary<string, string> ValidateAll(t_mansion_listModel model)
        {
            var sqlParams = new SqlParameter[]
            {};
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckIsHalfWidth("StartNum", model.StartAge, 15, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndNum", model.EndAge, 15, RegexFormat.Number); //E104,E105 

            validator.CheckIsHalfWidth("StartUnit", model.StartAge, 3, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndUnit", model.EndAge, 3, RegexFormat.Number); //E104,E105 

            validator.CheckCompareNum("EndNum", model.StartAge, model.EndAge);//E113
            validator.CheckCompareNum("EndUnit", model.StartUnit, model.EndUnit);//E113

            return validator.GetValidationResult();
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_get_m_pref_and_citygpcd", sqlParams);
            return dt;
        }


        public DataTable GetM_MansionList(t_mansion_listModel model)
        public DataTable Get_Prefcd_and_CityGPCD_and_CityCD()
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@StartAge", SqlDbType.Int){ Value = model.StartAge },
                new SqlParameter("@EndAge", SqlDbType.Int){ Value = model.EndAge },
                new SqlParameter("@StartUnit", SqlDbType.Int){ Value = model.StartUnit},
                new SqlParameter("@EndUnit", SqlDbType.Int){ Value = model.EndUnit },
                new SqlParameter("@Apartment", SqlDbType.VarChar){ Value =  model.Apartment.ToStringOrNull() }
             };
            {};

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_Select_M_Mansion", sqlParams);

            var dt = db.SelectDatatable("pr_t_mansion_list_get_m_pref_and_citygpcd_and_citycd", sqlParams);
            return dt;
        }
    }
}
