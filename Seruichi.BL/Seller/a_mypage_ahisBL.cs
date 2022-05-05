using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Seller;
namespace Seruichi.BL.Seller
{
    public class a_mypage_ahisBL
    {

        public DataTable GetD_AssReqProgressList(a_mypage_ahisModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },

             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_mypage_ahis_Select_D_AssReqProgress", sqlParams);
            return dt; 
        }
        public void InsertD_AssReqProgress_L_Log(a_mypage_ahisModel_l_log_Model model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LogDateTime", SqlDbType.VarChar){ Value = model.LogDateTime },
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0) },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.SellerCD.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@PageID", SqlDbType.VarChar){ Value = model.PageID },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.ProcessKBN },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_seller_mansion_list_Insert_L_Log", false, sqlParams);
        }
    }


}
