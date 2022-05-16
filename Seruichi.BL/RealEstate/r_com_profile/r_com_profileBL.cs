using System;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_com_profile;
using System.Collections.Generic;
using Models;
using System.Linq;

namespace Seruichi.BL.RealEstate.r_com_profile
{
    public class r_com_profileBL
    {
        public DataTable get_r_com_profileData(r_com_profileModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToString() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_com_profile_getDetailData", sqlParams);
            return dt;
        }

        public bool update_r_com_profileData(r_com_profileModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@BusinessHours", SqlDbType.TinyInt){ Value = model.BusinessHours.ToByte(0)},
                new SqlParameter("@CompanyHoliday", SqlDbType.VarChar){ Value = model.CompanyHoliday.ToStringOrNull() },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = model.Password.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
            };

            DBAccess db = new DBAccess();
            return db.InsertUpdateDeleteData("pr_r_com_profile_updateData", false, sqlParams);
        }

        public void Insertr_com_profile_L_Log(r_com_profileModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_RealEstate_Insert_L_Log", false, sqlParams);
        }

    }
}
