using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Seruichi.Common;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_config;

namespace Seruichi.BL.Tenfold.t_seller_config
{
    public class t_seller_configBL
    {
        public DataTable SelectFromMultipurpose(t_seller_configModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@DataID", SqlDbType.Int){ Value = model.DataID },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_config_SelectFromMultipurpose", sqlParams);
            return dt;
        }

        public void InsertUpdateToMultipurpose(t_seller_configModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@DataID", SqlDbType.Int){ Value = model.DataID },
                new SqlParameter("@Num1", SqlDbType.Int){ Value = model.Num },
                new SqlParameter("@Mode", SqlDbType.Int){ Value = model.Mode }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_config_InsertUpdateMultipurpose", false, sqlParams);
        }

        public void Insert_L_Log(l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@Page", SqlDbType.VarChar){ Value = model.Page },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.Processing },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_Tenfold_Insert_L_Log", false, sqlParams);
        }
    }
}
