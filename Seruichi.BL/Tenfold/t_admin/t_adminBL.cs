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
            if (dt.Rows.Count == 0)
            {
                errorcd = "E211"; //入力された値が正しくありません
                return false;
            }
            return true;
        }
    }
}
