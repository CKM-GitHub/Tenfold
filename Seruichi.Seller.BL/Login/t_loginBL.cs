using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;

namespace Seruichi.BL
{
    public class t_loginBL
    {
        private CommonBL commonBL = new CommonBL();


        public bool CheckPrefecturesByIdpsw(string TenStaffCD, string TenStaffPW, out string errorcd, out string outPrefCD)
        {
            errorcd = "";
            outPrefCD = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = TenStaffCD.ToStringOrNull() },
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = TenStaffPW.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_login_Select_PrefecturesByIdpsw", sqlParams);
            if (dt.Rows.Count == 0)
            {
                errorcd = "E103"; //入力された値が正しくありません
                return false;
            }

            var dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["RegionCD"].ToStringOrEmpty()))
            {
                errorcd = "E201"; //査定サービスの対象外地域です
                return false;
            }

            outPrefCD = dr["PrefCD"].ToStringOrEmpty();
            return true;
        }
    }
}
