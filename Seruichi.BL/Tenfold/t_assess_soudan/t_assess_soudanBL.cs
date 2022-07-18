using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models;
using Models.Tenfold.t_assess_soudan;
using Seruichi.Common;

namespace Seruichi.BL.Tenfold.t_assess_soudan
{
    public class t_assess_soudanBL
    {
        public Dictionary<string, string> ValidateAll(t_assess_soudanModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("untreated", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public DataTable get_t_assess_soudan_DisplayData(t_assess_soudanModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@FreeWord", SqlDbType.VarChar){ Value = model.FreeWord.ToStringOrNull() },
                new SqlParameter("@untreated", SqlDbType.TinyInt){ Value = model.untreated.ToByte(0) },
                new SqlParameter("@processing", SqlDbType.TinyInt){ Value = model.processing.ToByte(0) },
                new SqlParameter("@solution", SqlDbType.TinyInt){ Value = model.solution.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@M_PIC", SqlDbType.VarChar){ Value = model.M_PIC.ToStringOrNull() },
                new SqlParameter("@type", SqlDbType.VarChar){ Value = model.type.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_soudan_get_DisplayData", sqlParams);

            return dt;
        }

        public DataTable get_t_assess_soudan_CSVData(t_assess_soudanModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@FreeWord", SqlDbType.VarChar){ Value = model.FreeWord.ToStringOrNull() },
                new SqlParameter("@untreated", SqlDbType.TinyInt){ Value = model.untreated.ToByte(0) },
                new SqlParameter("@processing", SqlDbType.TinyInt){ Value = model.processing.ToByte(0) },
                new SqlParameter("@solution", SqlDbType.TinyInt){ Value = model.solution.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@M_PIC", SqlDbType.VarChar){ Value = model.M_PIC.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_soudan_get_CSVData", sqlParams);

            return dt;
        }

        public DataTable get_Modal_infotrainData(t_assess_soudanModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID.ToStringOrNull() },
                new SqlParameter("@ConsultID", SqlDbType.VarChar){ Value = model.ConsultID.ToStringOrNull() }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_soudan_get_Modal_infotrainData", sqlParams);
            return dt;
        }
    }
}
