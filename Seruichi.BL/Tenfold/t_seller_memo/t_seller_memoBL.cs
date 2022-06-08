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
        public string get_t_sellerName(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD },
                new SqlParameter("@Type", SqlDbType.TinyInt){ Value = 0 }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_SellerInfo", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            foreach (DataRow row in dt.Rows)
            {
                row["SellerName"] = crypt.DecryptFromBase64(row.Field<string>("SellerName"), decryptionKey);
            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["SellerName"].ToString();
            else
                return string.Empty;
        }

        public DataTable get_t_seller_Info(t_seller_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@Type", SqlDbType.TinyInt){ Value = 1 }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_SellerInfo", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var e = dt.AsEnumerable();
            foreach (DataRow row in dt.Rows)
            {
                row["SellerKana"] = crypt.DecryptFromBase64(row.Field<string>("SellerKana"), decryptionKey);
                row["SellerName"] = crypt.DecryptFromBase64(row.Field<string>("SellerName"), decryptionKey);
                row["TownName"] = crypt.DecryptFromBase64(row.Field<string>("TownName"), decryptionKey);
                row["Address1"] = crypt.DecryptFromBase64(row.Field<string>("Address1"), decryptionKey);
                row["Address2"] = crypt.DecryptFromBase64(row.Field<string>("Address2"), decryptionKey);
                row["HousePhone"] = crypt.DecryptFromBase64(row.Field<string>("HousePhone"), decryptionKey);
                row["HandyPhone"] = crypt.DecryptFromBase64(row.Field<string>("HandyPhone"), decryptionKey);
                row["MailAddress"] = crypt.DecryptFromBase64(row.Field<string>("MailAddress"), decryptionKey);
            }
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

        public void Delete_Cmd(t_seller_memoModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerMemoSEQ", SqlDbType.VarChar){ Value = model.SellerMemoSEQ.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_memo_DeleteCmd", false, sqlParams);
        }
    }
}
