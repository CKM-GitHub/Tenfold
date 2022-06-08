using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.t_reale_purchase;
using Models.Tenfold.t_reale_account;

namespace Seruichi.BL.Tenfold.t_reale_account
{
   public class t_reale_accountBL
    {
        public Dictionary<string, string> ValidateAll(t_reale_accountModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108 

            validator.CheckRequired("StartDate", model.StartDate);
            validator.CheckRequired("EndDate", model.EndDate);

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111



            validator.CheckRequired("penaltyArea", model.Ispenalty);
            validator.CheckByteCount("penaltyArea", model.Ispenalty, 500);

            return validator.GetValidationResult(); 
        }

        public DataTable get_t_reale_CompanyInfo(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_CompanyCountingInfo(t_reale_purchaseModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyCountingInfo", sqlParams);

            return dt;
        }
        //get_t_reale_account_DisplayData
        public DataSet get_t_reale_account_DisplayData(t_reale_accountModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDataSet("pr_t_reale_account_DisplayData", sqlParams);

            return dt;
        }
    }
}
