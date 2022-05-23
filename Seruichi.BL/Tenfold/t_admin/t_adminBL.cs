using Models.Tenfold.t_admin;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_admin
{
    public class t_adminBL
    {
        public bool CheckTenStaffCD(t_adminModel model, out string errorcd)
        {
            errorcd = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_admin_Select_M_TenfoldStaff_by_TenStaffCD", sqlParams);
            if (dt.Rows.Count > 0)
            {
                errorcd = "E211"; //入力された値が正しくありません
                return false;
            }
            return true;
        }

        public Dictionary<string, string> ValidateAll(t_adminModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckRequired("TenStaffCD", model.TenStaffCD);//E101
            validator.CheckRequired("TenStaffPW", model.TenStaffPW);//E101
            validator.CheckRequired("TenStaffName", model.TenStaffName);//E101

            if (!CheckTenStaffCD(model,out string aa))
                validator.GetValidationResult().Add("TenStaffCD", StaticCache.GetMessageText1("E211"));

            validator.CheckMaxLenght("TenStaffCD", model.TenStaffCD, 10);  //E105
            validator.CheckMaxLenght("TenStaffPW", model.TenStaffPW, 10);  //E105
            validator.CheckMaxLenght("TenStaffName", model.TenStaffName, 15);  //E105

            validator.CheckIsOnlyOneCharacter("TenStaffCD", model.TenStaffCD);  //E104
            validator.CheckIsOnlyOneCharacter("TenStaffPW", model.TenStaffPW);  //E104
            validator.CheckIsDoubleByte("TenStaffName", model.TenStaffName, 15);//E107

            return validator.GetValidationResult();
        }

        public DataTable Get_M_TenfoldStaff_By_LoginID(string loginID)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = loginID.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_admin_Select_M_TenfoldStaff", sqlParams);
            return dt;
        }
        public void Save_M_TenfoldStaff(t_adminModel model)
        {
            
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull()},
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() },
                new SqlParameter("@TenStaffName", SqlDbType.VarChar){ Value = model.TenStaffName.ToStringOrNull() },
                new SqlParameter("@InvalidFLG", SqlDbType.TinyInt){ Value =  model.InvalidFLG.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_admin_Insert_M_TenfoldStaff", false, sqlParams);
        }

        public void Update_M_TenfoldStaff(t_adminModel model)
        {

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull()},
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() },
                new SqlParameter("@TenStaffName", SqlDbType.VarChar){ Value = model.TenStaffName.ToStringOrNull() },
                new SqlParameter("@InvalidFLG", SqlDbType.TinyInt){ Value =  model.InvalidFLG.ToStringOrNull() },
                new SqlParameter("@DeleteFlG", SqlDbType.TinyInt){ Value =  model.DeleteFLG.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_admin_Update_M_TenfoldStaff", false, sqlParams);
        }
    }
}
