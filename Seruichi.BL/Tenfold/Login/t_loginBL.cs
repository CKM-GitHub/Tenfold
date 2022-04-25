using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.Login;

namespace Seruichi.BL.Tenfold.Login

{
    public class t_loginBL
    {
        public Dictionary<string, string> ValidateAll(t_loginModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckRequired("email", model.TenStaffCD);//E101
            validator.CheckRequired("password", model.TenStaffPW);//E101

            validator.CheckIsOnlyOneCharacter("email", model.TenStaffCD);//E104
            validator.CheckIsOnlyOneCharacter("password", model.TenStaffPW);//E104
           
            validator.CheckByteCount("email", model.TenStaffCD, 10);  //E105
            validator.CheckByteCount("password", model.TenStaffPW, 10);  //E105


            
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
