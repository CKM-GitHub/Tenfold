using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Seruichi.Common;
using Models.RealEstate.r_auto_mes;
using Models.RealEstate.r_template;

namespace Seruichi.BL.RealEstate.r_template
{
  public  class r_templateBL
    {
        public DataSet Get_templateData()
        {
            var sqlParams = new SqlParameter[]
            { 
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDataSet("pr_r_template_Select_DisplayData", sqlParams);
            return dt;
        } 

        public void Insert_REMessage_Data(r_auto_mesModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD },
                new SqlParameter("@MessageSEQ", SqlDbType.VarChar) { Value = model.MessageSEQ },
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
