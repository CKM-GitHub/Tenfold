using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.RealEstate.r_login;

namespace Seruichi.BL.RealEstate.r_login
{
    public class r_loginBL
    {
        public bool checkRealECD(r_loginModel model, out string errorcd)
        {
            errorcd = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_login_Select_M_RealEstate_by_RealECD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E311"; //入力された値が正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["RealECD"].ToStringOrEmpty()))
            {
                errorcd = "E311"; //査定サービスの対象外地域です
                return false;
            }

            return true;
        }
        public bool checkREStaffCD(r_loginModel model, out string errorcd, out string rePassword)
        {
            errorcd = "";
            rePassword = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E313"; //入力された値が正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["REPassword"].ToStringOrEmpty()))
            {
                errorcd = "E313"; //査定サービスの対象外地域です
                return false;
            }
            rePassword = dr["REPassword"].ToStringOrEmpty();
            if (dt.Rows.Count > 0)
            {
                model.REPassword= dt.Rows[0]["REPassword"].ToString();
                model.PermissionChat = dt.Rows[0]["PermissionChat"].ToByte();
                model.PermissionSetting = dt.Rows[0]["PermissionChat"].ToByte();
                model.PermissionPlan = dt.Rows[0]["PermissionChat"].ToByte();
                model.PermissionInvoice = dt.Rows[0]["PermissionChat"].ToByte();
            }
            return true;
        }
        public string GetREStaffNamebyREStaffCD(string REStaffCD)
        {
            string staffName = string.Empty;
            var sqlParm = new SqlParameter("@REStaffCD", SqlDbType.VarChar) { Value = REStaffCD };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_login_select_REStaffName_by_REStaffCD", sqlParm);
            if (dt.Rows.Count > 0)
            {
                staffName = dt.Rows[0]["REStaffName"].ToString();
            }
            return staffName;
        }
        public void Insert_L_Login(r_loginModel model, string ipaddress)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.REStaffName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = ipaddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_login_Insert_L_Login", false, sqlParams);
        }
    }
}
