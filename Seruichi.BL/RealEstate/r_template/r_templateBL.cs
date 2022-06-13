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
        public void Delete_R_templateDate(r_templateModel model)
        {

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD},
                new SqlParameter("@TemplateNo", SqlDbType.VarChar){ Value = model.TemplateNo },
                new SqlParameter("@TemplateKBN", SqlDbType.TinyInt){ Value = model.TemplateKBN },
            
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_r_template_DeleteData", false, sqlParams);
        }
        public void Insert_L_Log( r_templateModel model)
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
    }
}
