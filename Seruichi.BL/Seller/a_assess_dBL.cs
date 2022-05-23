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
        public DataSet GetD_Screen_Info(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
             };
            DBAccess db = new DBAccess();
            var ds = db.SelectDataSet("pr_a_assess_d_Select_Screen_by_AssReqID", sqlParams);
            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            foreach (DataRow dr in ds.Tables[0].Rows)
                dr["SellerName"] = crypt.DecryptFromBase64(dr["SellerName"].ToString(), decryptionKey);
            return ds;
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
        public void InsertA_Assess(a_assess_dModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@AssID", SqlDbType.VarChar){ Value = model.AssID },
                new SqlParameter("@AssSEQ", SqlDbType.VarChar){ Value = model.AssSEQ },
                new SqlParameter("@AssReqID", SqlDbType.VarChar){ Value = model.AssReqID },
                new SqlParameter("@ProgressKBN", SqlDbType.VarChar){ Value = model.ProgressKBN },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD },
                new SqlParameter("@SellerCD", SqlDbType.VarChar){ Value = model.SellerCD },
                new SqlParameter("@SellerMansionID", SqlDbType.VarChar){ Value = model.SellerMansionID },
                new SqlParameter("@AssessAmount", SqlDbType.VarChar){ Value = model.AssessAmount },
                new SqlParameter("@IpAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@SellerName", SqlDbType.VarChar){ Value = model.SellerName },
                new SqlParameter("@AssessType1", SqlDbType.VarChar){ Value = model.AssessType1 },
                new SqlParameter("@AssessType2", SqlDbType.VarChar){ Value = model.AssessType2 },
                new SqlParameter("@ConditionSEQ", SqlDbType.VarChar){ Value = model.ConditionSEQ },
             };
            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_a_assess_d_Insert_D_IntroReq_D_AssReqProgress_D_AssessResult", false, sqlParams);
        }
    }
}
