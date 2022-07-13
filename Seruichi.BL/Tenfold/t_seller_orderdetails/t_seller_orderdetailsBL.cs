using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seruichi.BL.Tenfold.t_seller_orderdetails
{
   public class t_seller_orderdetailsBL
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
        public string get_t_sellerPossibleTime(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_Select_M_Seller_By_SellerCD", sqlParams);


            foreach (DataRow row in dt.Rows)
            {
                row["PossibleTimes"] = row.Field<int>("PossibleTimes");
            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["PossibleTimes"].ToString();
            else
                return string.Empty;
        }
        public DataTable get_t_seller_Info(string model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.ToStringOrNull() },
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
        public DataTable get_D_SellerPossibleData(string model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_orderdetails_Select_D_SellerPossible_by_SellerCD", sqlParams);

            string decryptionKey = StaticCache.GetDataCryptionKey();
            var dr = dt.Rows[0];
            return dt;
        }

    }
}
