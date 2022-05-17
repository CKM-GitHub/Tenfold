using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Seller;
using System.Data.SqlClient;
using System.Data;
using Seruichi.Common;

namespace Seruichi.BL.Seller
{//pr_a_assess_d_Select_MansionInfo_by_AssReqID
    public class a_assess_dBL 
    {
        public DataTable GetD_Mansion_Info(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID }, 
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD }, 
             }; 
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_assess_d_Select_MansionInfo_by_AssReqID", sqlParams);
            return dt;
        }
        public DataTable GetD_Spec_Info(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
             };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_assess_d_Select_SpecInfo_by_AssReqID", sqlParams);
            return dt;
        }
        public DataTable GetD_MansionRank_Info(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID },
                //new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
             };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_assess_d_Select_MansionRank_by_AssReqID", sqlParams);
            return dt;
        }
        public DataTable GetD_AreaRank_Info(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID },
                //new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
             };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_a_assess_d_Select_AreaRank_by_AssReqID", sqlParams);
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
