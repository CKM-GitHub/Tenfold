using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.RealEstate.r_temp_mes;
using Seruichi.Common;

namespace Seruichi.BL.RealEstate.r_temp_mes
{
    public class r_temp_mesBL
    {
        public DataTable Get_M_REMessage_By_RealECD(string RealECD)
        {
          var sqlParams = new SqlParameter[]
          {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
          };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_temp_mes_Select_M_REMessage_By_RealECD", sqlParams);
            return dt;
        }

        public bool Insert_M_REMessage(r_temp_mesModel model, out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull()},
                new SqlParameter("@MessageTitle", SqlDbType.VarChar){ Value = model.MessageTitle.ToStringOrNull() },
                new SqlParameter("@MessageText", SqlDbType.VarChar){ Value = model.MessageTEXT.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
             };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_temp_mes_Insert_M_REMessage", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
        }

        public bool Update_M_REMessage(r_temp_mesModel model, out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull()},
                new SqlParameter("@MessageSEQ", SqlDbType.Int){ Value = model.MessageSEQ},
                new SqlParameter("@MessageTitle", SqlDbType.VarChar){ Value = model.MessageTitle.ToStringOrNull() },
                new SqlParameter("@MessageText", SqlDbType.VarChar){ Value = model.MessageTEXT.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
             };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_temp_mes_Update_M_REMessage", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
        }

        public bool Delete_M_REMessage(r_temp_mesModel model, out string msgid)
        {
            msgid = "";
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull()},
                new SqlParameter("@MessageSEQ", SqlDbType.Int){ Value = model.MessageSEQ},
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value = model.LoginName.ToStringOrNull()},
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID},
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress},
             };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_temp_mes_Delete_M_REMessage", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
        }
    }
}
