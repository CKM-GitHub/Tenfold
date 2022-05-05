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

        public bool Insert_D_SellerPossible_OK(a_mypage_plusModel model, out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD},
                new SqlParameter("@PossibleTimes", SqlDbType.Int){ Value = model.Possible_Time},
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress},
                new SqlParameter("@ChangeCount", SqlDbType.VarChar){ Value = model.Change_Count},
                new SqlParameter("@ChangeFee", SqlDbType.VarChar){ Value = model.ChangeFee},
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName}
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_mypage_plus_Update_M_Seller_by_SellerCD", false, sqlParams);
                
            }
            catch (ExclusionException)
            {
                return false;
            }
        }

        public bool Insert_D_SellerPossible_NG(a_mypage_plusModel model, out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD},
                new SqlParameter("@PossibleTimes", SqlDbType.VarChar){ Value = model.Possible_Time},
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress},
                new SqlParameter("@ChangeCount", SqlDbType.VarChar){ Value = model.Change_Count},
                new SqlParameter("@ChangeFee", SqlDbType.VarChar){ Value = model.ChangeFee},
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName}
            };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_a_mypage_plus_Update_M_Seller_by_SellerCD_NG", false, sqlParams);

            }
            catch (ExclusionException)
            {
                return false;
            }
        }
    }
}
