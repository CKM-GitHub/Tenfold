using System;
using System.Collections.Generic;
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

        public Dictionary<string, string> ValidateAll(t_mansion_listModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckIsHalfWidth("StartNum", model.StartAge, 15, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndNum", model.EndAge, 15, RegexFormat.Number); //E104,E105 

            validator.CheckIsHalfWidth("StartUnit", model.StartAge, 3, RegexFormat.Number); //E104,E105 
            validator.CheckIsHalfWidth("EndUnit", model.EndAge, 3, RegexFormat.Number); //E104,E105 

            validator.CheckCompareNum("EndNum", model.StartAge, model.EndAge);//E113
            validator.CheckCompareNum("EndUnit", model.StartUnit, model.EndUnit);//E113

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
                new SqlParameter("@Apartment", SqlDbType.VarChar){ Value =  model.Apartment.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_mansion_list_Select_M_Mansion", sqlParams);

            return dt;
        }
    }
}
