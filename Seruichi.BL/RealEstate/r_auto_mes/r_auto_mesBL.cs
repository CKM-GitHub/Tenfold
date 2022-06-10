using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_auto_mes;

namespace Seruichi.BL.RealEstate.r_auto_mes
{
    public class r_auto_mesBL
    {
        
        public DataTable Get_M_REMessage_By_RealECD(string RealECD)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD",SqlDbType.VarChar) { Value = RealECD },
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_auto_mes_Select_M_REMessage_By_RealECD", sqlParams);
            return dt;
        }

        public void Delete_REMessage_Data(r_auto_mesModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@MessageSEQ", SqlDbType.Int) { Value = model.MessageSEQ },
                new SqlParameter("@Processing", SqlDbType.VarChar) { Value = model.ProcessKBN },
                new SqlParameter("@Operator", SqlDbType.VarChar) { Value = model.LoginID },
                new SqlParameter("@LoginName", SqlDbType.VarChar) { Value = model.LoginName },
                new SqlParameter("@IPAddress", SqlDbType.VarChar) { Value = model.IPAddress },
                new SqlParameter("@Remarks", SqlDbType.VarChar) { Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_auto_mes_Delete_REMessageData_Insert_L_Log", false, sqlParams);
        }

        public void Update_REMessageValidFlg_Data(r_auto_mesModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@ValidFlg", SqlDbType.TinyInt) { Value = model.ValidFlg },
                new SqlParameter("@Operator", SqlDbType.VarChar) { Value = model.LoginID },
                new SqlParameter("@IPAddress", SqlDbType.VarChar) { Value = model.IPAddress },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_auto_mes_Update_REMessageValidFlgData", false, sqlParams);
        }

        public void UpdateData_REMessage_Data(r_auto_mesModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@MessageSEQ", SqlDbType.Int) { Value = model.MessageSEQ },
                new SqlParameter("@MessageTitle", SqlDbType.VarChar) { Value = model.MessageTitle },
                new SqlParameter("@MessageTEXT", SqlDbType.VarChar) { Value = model.MessageTEXT },
                new SqlParameter("@ValidFlg", SqlDbType.TinyInt) { Value = model.ValidFlg },
                new SqlParameter("@Operator", SqlDbType.VarChar) { Value = model.LoginID },
                new SqlParameter("@LoginName", SqlDbType.VarChar) { Value = model.LoginName },
                new SqlParameter("@IPAddress", SqlDbType.VarChar) { Value = model.IPAddress },
                new SqlParameter("@Remarks", SqlDbType.VarChar) { Value = model.Remarks },

             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_auto_mes_Update_REMessage_Data", false, sqlParams);
        }

        
        public void Insert_REMessage_Data(r_auto_mesModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@MessageSEQ", SqlDbType.Int) { Value = model.MessageSEQ },
                new SqlParameter("@MessageTitle", SqlDbType.VarChar) { Value = model.MessageTitle },
                new SqlParameter("@MessageTEXT", SqlDbType.VarChar) { Value = model.MessageTEXT },
                new SqlParameter("@ValidFlg", SqlDbType.TinyInt) { Value = model.ValidFlg },
                new SqlParameter("@Operator", SqlDbType.VarChar) { Value = model.LoginID },
                new SqlParameter("@LoginName", SqlDbType.VarChar) { Value = model.LoginName },
                new SqlParameter("@IPAddress", SqlDbType.VarChar) { Value = model.IPAddress },
                new SqlParameter("@Remarks", SqlDbType.VarChar) { Value = model.Remarks },

             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_auto_mes_Insert_REMessage_Data", false, sqlParams);
        }

    }
}
