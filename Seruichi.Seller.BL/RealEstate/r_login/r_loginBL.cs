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
    }
}
