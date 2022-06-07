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
using System.Data.SqlTypes;

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
            byte[] Newphoto = {};
            byte[] Updatephoto = {};

            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("RealECD", typeof(String));
            dtUpdate.Columns.Add("REStaffCD", typeof(String));  
            dtUpdate.Columns.Add("PermissionChat", typeof(int));
            dtUpdate.Columns.Add("PermissionSetting", typeof(int));
            dtUpdate.Columns.Add("PermissionPlan", typeof(int));
            dtUpdate.Columns.Add("PermissionInvoice", typeof(int));
            dtUpdate.Columns.Add("REPassword", typeof(String));
            dtUpdate.Columns.Add("REStaffName", typeof(String));
            dtUpdate.Columns.Add("REIntroduction", typeof(String));


            DataTable dtUpdateImage = new DataTable();
            dtUpdateImage.Columns.Add("RealECD", typeof(String));
            dtUpdateImage.Columns.Add("REStaffCD", typeof(String));
            DataColumn column = new DataColumn("REFaceImage"); //Create the column.
            column.DataType = System.Type.GetType("System.Byte[]"); //Type byte[] to store image bytes.
            column.AllowDBNull = true;
            column.Caption = "My Image";
            dtUpdateImage.Columns.Add(column);

            if (model.lst_StaffModel.Count > 0)
            {
                for (int i = 0; i < model.lst_StaffModel.Count; i++)
                {
                    DataRow rowImg = dtUpdateImage.NewRow();
                    DataRow row = dtUpdate.NewRow();
                    row["RealECD"] = model.lst_StaffModel[i].RealECD;
                    rowImg["RealECD"] = model.lst_StaffModel[i].RealECD;
                    if (!String.IsNullOrWhiteSpace(model.lst_StaffModel[i].REFaceImage))
                    {
                        Updatephoto = GetPhoto(model.lst_StaffModel[i].REFaceImage);
                        rowImg["REFaceImage"] = Updatephoto;
                    }
                    else
                    {
                        rowImg["REFaceImage"] = DBNull.Value;
                    }
                    rowImg["REStaffCD"] = model.lst_StaffModel[i].REStaffCD;
                    row["REStaffCD"] = model.lst_StaffModel[i].REStaffCD;
                    row["REStaffName"] = model.lst_StaffModel[i].REStaffName;
                    row["REIntroduction"] = model.lst_StaffModel[i].REIntroduction;
                    row["REPassword"] = model.lst_StaffModel[i].REPassword;
                    row["PermissionChat"] = model.lst_StaffModel[i].PermissionChat;
                    row["PermissionSetting"] = model.lst_StaffModel[i].PermissionSetting;
                    row["PermissionPlan"] = model.lst_StaffModel[i].PermissionPlan;
                    row["PermissionInvoice"] = model.lst_StaffModel[i].PermissionInvoice;
                    dtUpdate.Rows.Add(row);
                    dtUpdateImage.Rows.Add(rowImg);
                }
             }

            if (model.LoginID =="admin")
            {
                if (!String.IsNullOrWhiteSpace(model.REFaceImage))
                {
                    Newphoto = GetPhoto(model.REFaceImage);
                }
            }

            //dtUpdateImage.TableName = "test";
            //System.IO.StringWriter writer = new System.IO.StringWriter();
            //dtUpdateImage.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            //string XMLresult = writer.ToString();

            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@REFaceImage", SqlDbType.Image,Newphoto.Length){ Value = Newphoto},
                new SqlParameter("@RealECD", SqlDbType.VarChar){ Value = model.RealECD.ToStringOrNull()},
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = model.REStaffCD.ToStringOrNull() },
                new SqlParameter("@REPassword", SqlDbType.VarChar){ Value = model.REPassword.ToStringOrNull() },
                new SqlParameter("@REStaffName", SqlDbType.VarChar){ Value =  model.REStaffName.ToStringOrNull() },
                new SqlParameter("@REIntroduction", SqlDbType.VarChar){ Value =  model.REIntroduction.ToStringOrNull() },
                new SqlParameter("@PermissionChat", SqlDbType.TinyInt){ Value =  model.PermissionChat.ToByte() },
                new SqlParameter("@PermissionSetting", SqlDbType.TinyInt){ Value =  model.PermissionSetting.ToByte() },
                new SqlParameter("@PermissionPlan", SqlDbType.TinyInt){ Value =  model.PermissionPlan.ToByte() },
                new SqlParameter("@PermissionInvoice", SqlDbType.TinyInt){ Value =  model.PermissionInvoice.ToByte() },
                new SqlParameter("@LoginName", SqlDbType.VarChar){ Value =   model.LoginName.ToStringOrNull() },
                new SqlParameter("@Operator", SqlDbType.VarChar){ Value = model.LoginID },
                new SqlParameter("@IPAddress", SqlDbType.VarChar){ Value =  model.IPAddress },
                new SqlParameter("@Remarks", SqlDbType.VarChar){ Value =  model.Remarks },
                new SqlParameter("@ProcessKBN", SqlDbType.TinyInt){ Value =  model.ProcessKBN.ToByte() },
             //new SqlParameter("@RealECD", SqlDbType.VarChar){ Value =  model.RealECD },
               new SqlParameter("@M_REStaff", SqlDbType.Structured){ TypeName = "dbo.R_REStaff", Value = dtUpdate },
               new SqlParameter("@M_REImage", SqlDbType.Structured){ TypeName = "dbo.R_REFaceImage", Value = dtUpdateImage },
             // SqlParameter("@xml", SqlDbType.Xml){Value = XMLresult },
           
               
        };

            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_r_staff_Insert_M_REStaff", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
            //catch (Exception ex)
            //{
            //    string str = ex.ToString();
            //    return false;
            //}
        }
        public static byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
            filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            byte[] photo = reader.ReadBytes((int)stream.Length);
            reader.Close();
            stream.Close();

            return photo;
        }
    }
}
