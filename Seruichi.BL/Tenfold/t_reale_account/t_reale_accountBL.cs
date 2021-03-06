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
        public Dictionary<string, string> ValidateAll(t_reale_accountModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            if (model.penaltyFlg.ToByte(0).ToString() == "1")
            {
                validator.CheckDate("StartDate", model.StartDate);//E108
                validator.CheckDate("EndDate", model.EndDate);//E108  
                validator.CheckRequired("StartDate", model.StartDate);
                validator.CheckRequired("EndDate", model.EndDate); 
                validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
                validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111 
                validator.CheckRequired("penaltyArea", model.Memo);
                validator.CheckByteCount("penaltyArea", model.Memo, 500);
            }

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
        public void get_t_reale_account_ManipulateData(t_reale_accountModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@PFlg", SqlDbType.TinyInt){ Value = model.penaltyFlg.ToByte(0) },
                new SqlParameter("@TFlg", SqlDbType.TinyInt){ Value = model.testFlg.ToByte(0) },
                new SqlParameter("@StartDate", SqlDbType.Date){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.Date){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@Memo", SqlDbType.NVarChar){ Value = model.Memo.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull() },
                new SqlParameter("@IP", SqlDbType.VarChar){ Value = model.IPAddress.ToStringOrNull() } ,
                new SqlParameter("@Isfake", SqlDbType.TinyInt){ Value = model.Isfake.ToByte(0) } ,
                new SqlParameter("@SEQPenalty", SqlDbType.Int){ Value = model.SEQPenalty }
        };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_reale_account_InsertUpdateData_new", false, sqlParams);
        }


    }
}
