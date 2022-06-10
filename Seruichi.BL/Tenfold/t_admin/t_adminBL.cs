using Models.Tenfold.t_admin;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Seruichi.BL.Tenfold.t_admin
{
    public class t_adminBL
    {
        public bool CheckTenStaffCD(t_adminModel model, out string errorcd)
        {
            errorcd = "";

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_admin_Select_M_TenfoldStaff_by_TenStaffCD", sqlParams);
            if (dt.Rows.Count > 0)
            {
                errorcd = "E211"; //入力された値が正しくありません
                return false;
            }
            return true;
        }

        public Dictionary<string, string> ValidateAll(t_adminModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //new M_tenfoldStaff
            if(!string.IsNullOrEmpty(model.TenStaffCD))
            {
                if (!CheckTenStaffCD(model, out string aa))
                    validator.GetValidationResult().Add("TenStaffCD", StaticCache.GetMessageText1("E211"));

                validator.CheckRequired("TenStaffPW", model.TenStaffPW);//E101
                validator.CheckRequired("TenStaffName", model.TenStaffName);//E101

                validator.CheckMaxLenght("TenStaffCD", model.TenStaffCD, 10);  //E105
                validator.CheckMaxLenght("TenStaffPW", model.TenStaffPW, 10);  //E105
                validator.CheckMaxLenght("TenStaffName", model.TenStaffName, 15);  //E105

                validator.CheckIsOnlyOneCharacter("TenStaffCD", model.TenStaffCD);  //E104
                validator.CheckIsOnlyOneCharacter("TenStaffPW", model.TenStaffPW);  //E104
                validator.CheckIsDoubleByte("TenStaffName", model.TenStaffName, 15);//E107
            }
            //update M_TenfoldStaff
            int i = 1;
            foreach (var item in model.lst_AdminModel)
            {
                validator.CheckRequired("txtpw_" + i, item.TenStaffPW);
                validator.CheckMaxLenght("txtpw_"+i, item.TenStaffPW, 10); 
                validator.CheckIsOnlyOneCharacter("txtpw_" + i, item.TenStaffPW);

                validator.CheckRequired("txtname_" + i, item.TenStaffName);
                validator.CheckIsDoubleByte("txtname_" + i, item.TenStaffName,15);
                validator.CheckMaxLenght("txtname_" + i, item.TenStaffName,15); 
                i++;
            }
            //update Admin Password
            validator.CheckRequired("txtPassword", model.AdminPassword);
            validator.CheckIsOnlyOneCharacter("txtPassword", model.AdminPassword);
            validator.CheckMinLenght("txtPassword", model.AdminPassword, 8);
            validator.CheckMaxLenght("txtPassword", model.AdminPassword,30);

            validator.CheckRequired("txtConfirmPassword", model.AdminConfirmPassword);
            validator.CheckComparePassword("txtConfirmPassword", model.AdminPassword, model.AdminConfirmPassword);

            return validator.GetValidationResult();
        }

        public DataTable Get_M_TenfoldStaff_By_LoginID(string loginID)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@LoginID", SqlDbType.VarChar){ Value = loginID.ToStringOrNull() }
            };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_admin_Select_M_TenfoldStaff", sqlParams);
            return dt;
        }
        public string ListToXML(List<Update_t_adminModel> lToSerialize)
        {
            using (var strWritr = new StringWriter(new StringBuilder()))
            {
                var serializer = new XmlSerializer(typeof(List<Update_t_adminModel>));
                serializer.Serialize(strWritr, lToSerialize);
                return strWritr.ToString();
            }
        }
        public bool Save_M_TenfoldStaff(t_adminModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull()},
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() },
                new SqlParameter("@TenStaffName", SqlDbType.VarChar){ Value = model.TenStaffName.ToStringOrNull() },
                new SqlParameter("@InvalidFLG", SqlDbType.TinyInt){ Value =  model.InvalidFLG.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
                new SqlParameter("@M_TenfoldStaff", SqlDbType.Structured){ TypeName = "dbo.T_TenfoldStaff", Value = model.lst_AdminModel.ToDataTable() },
                new SqlParameter("@AdminTenStaffPW", SqlDbType.VarChar){ Value = model.AdminPassword },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value =  model.Remark },
                new SqlParameter("@ProcessKBN", SqlDbType.TinyInt){ Value =  model.Processing.ToByte() },
             };

            try
            {
               DBAccess db = new DBAccess();
               return  db.InsertUpdateDeleteData("pr_t_admin_Insert_M_TenfoldStaff", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        public void Update_M_TenfoldStaff(t_adminModel model)
        {
           // string xml = ToXml(model.lst_AdminModel);
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull()},
                new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() },
                new SqlParameter("@TenStaffName", SqlDbType.VarChar){ Value = model.TenStaffName.ToStringOrNull() },
                new SqlParameter("@InvalidFLG", SqlDbType.TinyInt){ Value =  model.InvalidFLG.ToStringOrNull() },               
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress },
             };

            DBAccess db = new DBAccess();
            db.InsertUpdateDeleteData("pr_t_admin_Update_M_TenfoldStaff", false, sqlParams);
        }
       
    }
}
