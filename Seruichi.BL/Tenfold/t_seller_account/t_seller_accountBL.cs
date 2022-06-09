using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Tenfold.t_seller_account;

namespace Seruichi.BL.Tenfold.t_seller_account
{
    public class t_seller_accountBL
    {
      
        public DataTable GetM_SellerBy_SellerCD(t_seller_accountModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_assessment_Select_M_Seller", sqlParams);

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

      
    }
}
