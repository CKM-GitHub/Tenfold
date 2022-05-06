using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Seruichi.BL
{
    public class a_mypage_whisBL
    {
        public DataTable GetD_SellerPossibleData(string sellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = sellerCD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt= db.SelectDatatable("pr_a_mypage_whis_Select_D_SellerPossible_by_SellerCD", sqlParams);
          

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            var dr = dt.Rows[0];
            return dt;
        }
      }
}
