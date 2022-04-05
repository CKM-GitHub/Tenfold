using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_mansion;
namespace Seruichi.BL.Tenfold.t_seller_mansion
{
    public class t_seller_mansionBL
    {
        public Dictionary<string, string> ValidateAll(t_seller_mansionModel model,List<string> lst_checkBox)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckByteCount("MansionName",model.MansionName,50);//E105
            validator.CheckIsDoubleByte("MansionName", model.MansionName, 50);//E107

            validator.CheckDate("StartDate", model.StartDate.ToString());//E108
            validator.CheckDate("EndDate", model.EndDate.ToString());//E108

            validator.CheckCompareDate("StartDate", model.StartDate.ToString(), model.EndDate.ToString());//E111
            validator.CheckCompareDate("EndDate", model.StartDate.ToString(), model.EndDate.ToString());//E111

            validator.CheckCheckboxLenght("CheckBoxError", lst_checkBox);//E112

            return validator.GetValidationResult();
        }
        public DataTable GetM_SellerMansionList(t_seller_mansionModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@Chk_Blue", SqlDbType.TinyInt){ Value = model.Chk_Blue.ToByte(0) },
                new SqlParameter("@Chk_Sky", SqlDbType.TinyInt){ Value = model.Chk_Sky.ToByte(0) },
                new SqlParameter("@Chk_Orange", SqlDbType.TinyInt){ Value = model.Chk_Orange.ToByte(0) },
                new SqlParameter("@Chk_Green", SqlDbType.TinyInt){ Value = model.Chk_Green.ToByte(0) },
                new SqlParameter("@Chk_Brown", SqlDbType.TinyInt){ Value = model.Chk_Brown.ToByte(0) },
                new SqlParameter("@Chk_Dark_Sky", SqlDbType.TinyInt){ Value = model.Chk_Dark_Sky.ToByte(0) },
                new SqlParameter("@Chk_Gray", SqlDbType.TinyInt){ Value = model.Chk_Gray.ToByte(0) },
                new SqlParameter("@Chk_Black", SqlDbType.TinyInt){ Value = model.Chk_Black.ToByte(0) },
                new SqlParameter("@Chk_Pink", SqlDbType.TinyInt){ Value = model.Chk_Pink.ToByte(0) },
                new SqlParameter("@MansionName", SqlDbType.TinyInt){ Value = model.MansionName.ToByte(0) },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull() },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() }
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_mansion_Select_M_SellerMansionData", sqlParams);
            return dt;
        }
    }
}
