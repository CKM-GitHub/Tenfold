using Models;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Seruichi.BL.Seller
{

    public class a_mypage_taikaiBL
    {
        public a_mypage_taikaiModel Get_Count(string UserID)
        {
            var sqlParms = new SqlParameter[]
            {
                new SqlParameter("@SellerCD",SqlDbType.VarChar){Value = UserID}
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_plus_Select_D_SellerPossible_bySellerCD", sqlParms);
            a_mypage_taikaiModel model = DataTableExtentions.ToEntity<a_mypage_taikaiModel>(dt.Rows[0]);
            return model;

        }
    }

        
       

}
