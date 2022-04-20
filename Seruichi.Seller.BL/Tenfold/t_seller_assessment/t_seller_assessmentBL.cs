using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_assessment;

namespace Seruichi.BL.Tenfold.t_seller_assessment
{
    public class t_seller_assessmentBL
    {
        public Dictionary<string, string> ValidateAll(t_seller_assessmentModel model, List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111

            validator.CheckCheckboxLenght("Chk_Mi", lst_checkBox);//E112

            return validator.GetValidationResult();
        }

        public t_seller_assessment_header_Model GetM_SellerBy_SellerCD(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_M_Seller", sqlParams);
            t_seller_assessment_header_Model model = DataTableExtentions.ToEntity<t_seller_assessment_header_Model>(dt.Rows[0]);

            return model;
        }

        public DataTable GetM_SellerMansionList(t_seller_assessmentModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Mi", SqlDbType.TinyInt){ Value = model.Chk_Mi.ToByte(0) },
                new SqlParameter("@Chk_Kan", SqlDbType.TinyInt){ Value = model.Chk_Kan.ToByte(0) },
                new SqlParameter("@Chk_Satei", SqlDbType.TinyInt){ Value = model.Chk_Satei.ToByte(0) },
                new SqlParameter("@Chk_Kaitori", SqlDbType.TinyInt){ Value = model.Chk_Kaitori.ToByte(0) },
                new SqlParameter("@Chk_Kakunin", SqlDbType.TinyInt){ Value = model.Chk_Kakunin.ToByte(0) },
                new SqlParameter("@Chk_Kosho", SqlDbType.TinyInt){ Value = model.Chk_Kosho.ToByte(0) },
                new SqlParameter("@Chk_Seiyaku", SqlDbType.TinyInt){ Value = model.Chk_Seiyaku.ToByte(0) },
                new SqlParameter("@Chk_Urinushi", SqlDbType.TinyInt){ Value = model.Chk_Urinushi.ToByte(0) },
                new SqlParameter("@Chk_Kainushi", SqlDbType.TinyInt){ Value = model.Chk_Kainushi.ToByte(0) },
                new SqlParameter("@Chk_Sakujo", SqlDbType.TinyInt){ Value = model.Chk_Sakujo.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_M_SellerMansionData", sqlParams);
            return dt;
        }
    }
}
