using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Seruichi.BL.Seller
{
    public class a_mypage_plusBL
    {
            public DataTable Get_Calculate_extra_charge(a_mypage_plusModel model)
           {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@Change_Count", SqlDbType.VarChar){ Value = model.Change_Count}
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_plus_Select_M_MultPurpose_Calculate_Extra_Charges", sqlParams);
            return dt;
        }
        
        public a_mypage_plusModel Get_Possible_Time(string UserID)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = UserID }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_plus_Select_D_SellerPossible_bySellerCD", sqlParams);
            a_mypage_plusModel model = DataTableExtentions.ToEntity<a_mypage_plusModel>(dt.Rows[0]);
            return model;
        }
    }
}
