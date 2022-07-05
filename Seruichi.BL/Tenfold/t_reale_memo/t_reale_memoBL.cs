using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.Tenfold.t_reale_memo;

namespace Seruichi.BL.Tenfold.t_reale_memo
{
    public class t_reale_memoBL
    {
        public DataTable get_t_reale_CompanyInfo(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_CompanyCountingInfo(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_get_CompanyCountingInfo", sqlParams);

            return dt;
        }

        public DataTable get_t_reale_memo_DisplayData(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_reale_memo_getDisplayData", sqlParams);

            return dt;
        }

        public void Insert_L_Log(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@LoginKBN", SqlDbType.TinyInt){ Value = model.LoginKBN.ToByte(0)},
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@Page", SqlDbType.VarChar){ Value = model.Page },
                new SqlParameter("@Processing", SqlDbType.VarChar){ Value = model.Processing },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value = model.Remarks },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_Tenfold_Insert_L_Log", false, sqlParams);
        }

        public void Modify_MemoText(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@REMemoSEQ", SqlDbType.VarChar){ Value = model.REMemoSEQ.ToStringOrNull() },
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@ParentChildKBN", SqlDbType.VarChar){ Value = model.ParentChildKBN.ToStringOrNull() },
                new SqlParameter("@ParentSEQ", SqlDbType.VarChar){ Value = model.ParentSEQ.ToStringOrNull() },
                new SqlParameter("@MemoText", SqlDbType.VarChar){ Value = model.MemoText.ToStringOrNull() },
                new SqlParameter("@Type", SqlDbType.VarChar){ Value = model.Type.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_reale_memo_ModifyMemoText", false, sqlParams);
        }

        public void Delete_MemoText(t_reale_memoModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@REMemoSEQ", SqlDbType.VarChar){ Value = model.REMemoSEQ.ToStringOrNull() },
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = model.LoginID.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_reale_memo_DeleteMemoText", false, sqlParams);
        }
    }
}
