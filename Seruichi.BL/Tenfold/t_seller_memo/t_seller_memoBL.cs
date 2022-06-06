using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.t_seller_memo;

namespace Seruichi.BL.Tenfold.t_seller_memo
{
    public class t_seller_memoBL
    {
        public DataTable get_t_seller_Info(t_seller_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_SellerInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_seller_memo_DisplayData(t_seller_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_memo_getDisplayData", sqlParams);

            return dt;
        }

        public void Insert_L_Log(t_seller_memoModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
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
