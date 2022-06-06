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
        public DataTable GetM_SellerBy_SellerCD(string SellerCD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = SellerCD }
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_seller_account_Select_M_Seller_By_SellerCD", sqlParams);
            return dt;
        }
    }
}
