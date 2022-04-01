using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.Login;

namespace Seruichi.BL.Tenfold.Login

{
    public class t_loginBL
    {
        //private CommonBL commonBL = new CommonBL();

        //public bool CheckPrefecturesByIdpsw(string TenStaffCD, string TenStaffPW, out string errorcd, out string outPrefCD)
        //{
        //    errorcd = "";
        //    outPrefCD = "";

        //    var sqlParams = new SqlParameter[]
        //    {
        //        new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = TenStaffCD.ToStringOrNull() },
        //        new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = TenStaffPW.ToStringOrNull() }
        //    };

        //    DBAccess db = new DBAccess();
        //    var dt = db.SelectDatatable("pr_t_login_Select_PrefecturesByIdpsw", sqlParams);
        //    if (dt.Rows.Count == 0)
        //    {
        //        errorcd = "E103"; //入力された値が正しくありません
        //        return false;
        //    }
        //    return true;
        //}


        public Dictionary<string, string> ValidateAll(t_loginModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckRequired("TenStaffCD", model.TenStaffCD);
            validator.CheckRequired("TenStaffPW", model.TenStaffPW);
            validator.CheckIsHalfWidth("TenStaffCD", model.TenStaffCD, 10);
            validator.CheckIsHalfWidth("TenStaffPW", model.TenStaffPW, 10);
            return validator.GetValidationResult();
        }


        public DataTable GetM_TenfoldStaff(t_loginModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull() },
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_login_Select_M_TenfoldStaff", sqlParams);
            return dt;
        }
    }
}
