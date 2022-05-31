using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.RealEstate.r_staff;
using Models;
using System.IO;
using System.Xml.Serialization;

namespace Seruichi.BL.RealEstate.r_staff
{
   public class r_staffBL
    {
        public Dictionary<string, string> ValidateAll(r_staffModel model)
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            //new M_REStaff
            if (!string.IsNullOrEmpty(model.REStaffCD))
            {
                if (!Get_select_M_REStaff(model, out string aa))
                    validator.GetValidationResult().Add("REStaffCD", StaticCache.GetMessageText1("E314"));

                validator.CheckRequired("REStaffCD", model.REStaffCD);//E101
                validator.CheckRequired("REStaffName", model.REStaffName);//E101
                validator.CheckRequired("REPassword", model.REPassword);//E101

                validator.CheckMaxLenght("REStaffCD", model.REStaffCD, 10);  //E105
                validator.CheckMaxLenght("REStaffName", model.REStaffName, 30);  //E105
                validator.CheckMaxLenght("REIntroduction", model.REIntroduction, 500);  //E105
                validator.CheckMaxLenght("REPassword", model.REPassword, 20);  //E105

                validator.CheckIsOnlyOneCharacter("REStaffCD", model.REStaffCD);  //E104
                validator.CheckIsOnlyOneCharacter("REPassword", model.REPassword);  //E104
                

                validator.CheckMinLenght("REPassword", model.REPassword, 8); //E110
            }
            //update M_REStaff
            int i = 1;
            foreach (var item in model.lst_StaffModel)
            {
                validator.CheckRequired("REStaffCD_" + i, item.REStaffCD);
                validator.CheckMaxLenght("REStaffCD_" + i, item.REStaffCD, 10);
                validator.CheckIsOnlyOneCharacter("REStaffCD_" + i, item.REStaffCD);

                validator.CheckRequired("REStaffName_" + i, item.REStaffName);
                validator.CheckMaxLenght("REStaffName_" + i, item.REStaffName, 30);


                validator.CheckMaxLenght("REIntroduction_" + i, item.REIntroduction, 500);

                validator.CheckRequired("REPassword_" + i, item.REPassword);
                validator.CheckIsOnlyOneCharacter("REPassword_" + i, item.REPassword);
                validator.CheckMaxLenght("REPassword_" + i, item.REPassword,20);
                validator.CheckMinLenght("REPassword_" + i, item.REPassword, 8); 

                i++;
            }
            return validator.GetValidationResult();
        }
        public DataTable Get_M_REStaff_By_RealECD_IsAdmin(string RealECD,string REStaffCD)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = RealECD },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){Value = REStaffCD },
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_staff_Select_M_REStaff_By_RealECD_IsAdmin", sqlParams);
            return dt;
        }
        
       public bool Get_select_M_REStaff(r_staffModel model, out string errorcd)
        {
            errorcd = "";
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@RealECD", SqlDbType.VarChar) { Value = model.RealECD.ToStringOrNull() },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){Value = model.REStaffCD.ToStringOrNull()},
             };

            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_r_login_Select_M_REStaff_by_RealECD_and_REStaffCD", sqlParams);
            if(dt.Rows.Count>0)
            {
                errorcd = "E314"; 
                return false;
            }
            return true;
        }

     
        public bool Save_M_REStaff(r_staffModel model, out string msgid)
        {
            msgid = "";

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@REFaceImage", SqlDbType.VarChar){ Value = model.REFaceImage.ToStringOrNull()},
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull()},
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToStringOrNull() },
                new SqlParameter("@REPassword", SqlDbType.VarChar){ Value = model.REPassword.ToStringOrNull() },
                new SqlParameter("@REStaffName", SqlDbType.TinyInt){ Value =  model.REStaffName.ToStringOrNull() },
                new SqlParameter("@REIntroduction", SqlDbType.TinyInt){ Value =  model.REIntroduction.ToStringOrNull() },
                new SqlParameter("@PermissionChat", SqlDbType.TinyInt){ Value =  model.PermissionChat.ToStringOrNull() },
                new SqlParameter("@PermissionSetting", SqlDbType.TinyInt){ Value =  model.PermissionSetting.ToStringOrNull() },
                new SqlParameter("@PermissionPlan", SqlDbType.TinyInt){ Value =  model.PermissionPlan.ToStringOrNull() },
                new SqlParameter("@PermissionInvoice", SqlDbType.TinyInt){ Value =  model.PermissionInvoice.ToStringOrNull() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =   model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value =  model.IPAddress },
                //new SqlParameter("@RealECD", SqlDbType.VarChar){ Value =  model.RealECD },
                new SqlParameter("@M_REStaff", SqlDbType.Structured){ TypeName = "dbo.R_REStaff", Value = model.lst_StaffModel.ToDataTable() },
               
             };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_staff_Insert_M_REStaff", false, sqlParams);
            }
            catch (ExclusionException)
            {
                //msgid = "S004"; //他端末エラー
                return false;
            }
        }

        //public void Update_M_REStaff(r_staffModel model)
        //{
        //    var sqlParams = new SqlParameter[]
        //     {
        //        new SqlParameter("@TenStaffCD", SqlDbType.VarChar){ Value = model.TenStaffCD.ToStringOrNull()},
        //        new SqlParameter("@TenStaffPW", SqlDbType.VarChar){ Value = model.TenStaffPW.ToStringOrNull() },
        //        new SqlParameter("@TenStaffName", SqlDbType.VarChar){ Value = model.TenStaffName.ToStringOrNull() },
        //        new SqlParameter("@InvalidFLG", SqlDbType.TinyInt){ Value =  model.InvalidFLG.ToStringOrNull() },
        //        new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =  model.LoginName.ToStringOrNull() },
        //        new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.Operator.ToStringOrNull() },
        //        new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value = model.IPAddress }
        //     };

        //    DBAccess db = new DBAccess();
        //    db.InsertUpdateDeleteData("pr_r_staff_Update_M_REStaff", false, sqlParams);
        //}
    }
}
