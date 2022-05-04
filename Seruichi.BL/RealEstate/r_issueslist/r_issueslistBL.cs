using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_issueslist;
using System.Collections.Generic;
using Models;
using System.Linq;

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
            var e = dt.AsEnumerable();
            if (!string.IsNullOrEmpty(model.REStaffCD))
            {
                var query = e.Where(dr => dr.Field<string>("マンション名").Contains(model.REStaffCD));
                if (query.Any())
                {
                    int i = 0;
                    foreach (var row in query)
                    {
                        i++;
                        row["NO"] = i;
                    }
                    return query.CopyToDataTable();
                }
                else
                {
                    DataTable newTable = dt.Clone();
                    return newTable;
                }
            }
            return dt;
        }

        public void Insertr_issueslist_L_Log(r_issueslistModel model)
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
