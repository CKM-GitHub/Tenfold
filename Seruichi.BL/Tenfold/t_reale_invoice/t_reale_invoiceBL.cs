using Models.Tenfold.t_reale_invoice;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_reale_invoice
{
    public class t_reale_invoiceBL
    {
        public Dictionary<string, string> ValidateAll(t_reale_invoiceModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckYM("StartMonth", model.StartDate);//E125
            validator.CheckYM("EndMonth", model.EndDate);//E125

            validator.CheckCompareYM("StartMonth", model.StartDate, model.EndDate);//E126
            validator.CheckCompareYM("EndMonth", model.StartDate, model.EndDate);//E126
            return validator.GetValidationResult();
        }

        public DataTable Get_D_Billing_List(t_reale_invoiceModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@Range", SqlDbType.VarChar){Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@Option", SqlDbType.TinyInt){Value = model.Option.ToStringOrNull() },

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_invoice_Select_D_Billing_By_RealECD", sqlParams);
            return dt;
        }

        public DataTable get_t_reale_CompanyInfo(t_reale_invoiceModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_CompanyCountingInfo(t_reale_invoiceModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyCountingInfo", sqlParams);

            return dt;
        }
    }
}
