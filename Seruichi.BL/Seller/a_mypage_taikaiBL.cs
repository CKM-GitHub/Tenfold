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

        public bool Update_M_Seller_Data(a_mypage_taikaiModel model,out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD},
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress},
                new SqlParameter("@loginName", SqlDbType.VarChar){ Value = model.LoginName}
            };
            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_mypage_taikai_Update_M_Seller_Data", false, sqlParams);
            }
            catch(ExclusionException ex)
            {
                return false;
            }
        }
    }

        
       

}
