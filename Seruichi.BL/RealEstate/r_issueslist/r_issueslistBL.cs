using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_issueslist;
using System.Collections.Generic;
using Models;

namespace Seruichi.BL.RealEstate.r_issueslist
{
    public class r_issueslistBL
    {
        public Dictionary<string, string> ValidateAll(r_issueslistModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckMaxLenght("REStaffCD", model.REStaffCD, 50);//E105
            validator.CheckIsDoubleByte("REStaffCD", model.REStaffCD, 50);//E107

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("chk_New", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public DataTable get_issueslist_Data(r_issueslistModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@chk_New", SqlDbType.TinyInt){ Value = model.chk_New.ToByte(0) },
                new SqlParameter("@chk_Nego", SqlDbType.TinyInt){ Value = model.chk_Nego.ToByte(0) },
                new SqlParameter("@chk_Contract", SqlDbType.TinyInt){ Value = model.chk_Contract.ToByte(0) },
                new SqlParameter("@chk_SellerDeclined", SqlDbType.TinyInt){ Value = model.chk_SellerDeclined.ToByte(0) },
                new SqlParameter("@chk_BuyerDeclined", SqlDbType.TinyInt){ Value = model.chk_BuyerDeclined.ToByte(0) },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToString() },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_issueslist_getDisplayData", sqlParams);
            return dt;
        }
    }
}
