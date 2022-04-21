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

        public bool checkRealECD(string RealECD, out string errorcd)
        {
            errorcd = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = RealECD.ToStringOrNull() }
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
        public bool checkREStaffCD(string RealECD, string REStaffCD, out string errorcd, out string rePassword)
        {
            errorcd = "";
            rePassword = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = RealECD.ToStringOrNull() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = REStaffCD.ToStringOrNull() }
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
        public void Insert_L_Login(r_login_l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_login_Insert_L_Login", false, sqlParams);
        }
    }
}
