using Seruichi.Common;
using System;
using Seruichi.BL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Seruichi.BL.Tenfold.t_assess_guide;
using Models.Tenfold.t_assess_guide; 
using System.Data.SqlClient;
using Models;

namespace Seruichi.BL.Tenfold.t_assess_guide
{
  public  class t_assess_guideBL
    {
        public Dictionary<string, string> ValidateAll(t_assess_guideModel model )
        {
            ValidatorAllItems validator = new ValidatorAllItems();

            validator.CheckDate("StartDate", model.StartDate);//E108
            validator.CheckDate("EndDate", model.EndDate);//E108

            validator.CheckCompareDate("StartDate", model.StartDate, model.EndDate);//E111
            validator.CheckCompareDate("EndDate", model.StartDate, model.EndDate);//E111
             

            return validator.GetValidationResult();
        }
        //get_t_asses_guide_DisplayData
        public DataTable get_t_asses_guide_DisplayData(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@Key", SqlDbType.VarChar){ Value = model.Key },
                new SqlParameter("@Kanritantou", SqlDbType.VarChar){ Value = model.Kanritantou },
                new SqlParameter("@Range", SqlDbType.VarChar){ Value = model.Range.ToStringOrNull()== "買取依頼日" ?  "0" : "1"  },
                new SqlParameter("@StartDate", SqlDbType.VarChar){ Value = model.StartDate.ToStringOrNull() },
                new SqlParameter("@EndDate", SqlDbType.VarChar){ Value = model.EndDate.ToStringOrNull() },
                new SqlParameter("@MiShouri", SqlDbType.TinyInt){ Value = model.Chk_MiShouri.ToByte(0) },
                new SqlParameter("@Shouri", SqlDbType.TinyInt){ Value = model.Chk_Shouri.ToByte(0) },
                new SqlParameter("@YouKakunin", SqlDbType.TinyInt){ Value = model.Chk_YouKakunin.ToByte(0) },
                new SqlParameter("@HouShuu", SqlDbType.TinyInt){ Value = model.Chk_HouShuu.ToByte(0) },
                new SqlParameter("@Soukyaku", SqlDbType.TinyInt){ Value = model.Chk_Soukyaku.ToStringOrNull() }, 
            };
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_guide_temp", sqlParams);

            AESCryption crypt = new AESCryption();
            string decryptionKey = StaticCache.GetDataCryptionKey();
            foreach (DataRow row in dt.Rows)
            {
                row["売主名"] = crypt.DecryptFromBase64(row.Field<string>("売主名"), decryptionKey);
            }
            return dt;
        }
        public List<DropDownListItem> GetDropDownListItems( )
        {  
            var items = new List<DropDownListItem>(); 
            DBAccess db = new DBAccess();
            var dt = db.SelectDatatable("pr_t_assess_guide_tenfoldStaff",  new SqlParameter[]{ });

            foreach (DataRow dr in dt.Rows)
            {
                var option = new DropDownListItem()
                {
                    Value = dr["TenStaffCD"].ToStringOrEmpty(),
                    DisplayText = dr["TenStaffName"].ToStringOrEmpty()
                };
                items.Add(option);
            }

            return items;
        }

        public DataTable CreateGuideAttach(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
                    {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID },
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@UserCD", SqlDbType.VarChar){ Value = model.UserCD  },
                new SqlParameter("@AttachFileName", SqlDbType.VarChar){ Value = model.AttachFileName.Replace("\r\n","").Split('.').First() },
                new SqlParameter("@AttachFileType", SqlDbType.VarChar){ Value = model.AttachFileType.Replace("\r\n","")},
                new SqlParameter("@AttachFileSize", SqlDbType.VarChar){ Value = model.AttachSize.Replace("\r\n","") },
                    };
            try
            {
                DBAccess db = new DBAccess();
                return db.SelectDatatable("pr_t_assess_guide_CreateGuideAttach_temp", sqlParams);
            }
            catch (ExclusionException)
            {
                return null;
            }
        }
        public DataTable get_t_assess_guide_AttachFiles(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
                    {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID }
                    };
            try
            {
                DBAccess db = new DBAccess();
                return db.SelectDatatable("pr_t_assess_guide_SelectAttachment_temp", sqlParams);
            }
            catch (ExclusionException)
            {
                return null;
            }
        } 
        public bool CreateGuideAttachZipped(t_assess_guideModel model)
        {
            AESCryption crypt = new AESCryption();
            string cryptionKey = StaticCache.GetDataCryptionKey();
            var sqlParams = new SqlParameter[]
                    {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID },
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@UserCD", SqlDbType.VarChar){ Value = model.UserCD  },
                new SqlParameter("@AttachFileUnzipPW", SqlDbType.VarChar){ Value = crypt.EncryptToBase64(model.AttachFileUnzipPW , cryptionKey).ToStringOrNull() }, //ZippedFileName
                new SqlParameter("@ZippedFileName", SqlDbType.VarChar){ Value = model.ZippedFileName }, //
                    };
            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_t_assess_guide_CreateGuideAttachZipped_temp", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
        }
        //pr_t_assess_guide_SelectAttachment_temp
        //CreateGuideAttachZippedFilePath
        public bool CreateGuideAttachZippedFilePath(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID },
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@UserCD", SqlDbType.VarChar){ Value = model.UserCD  },
                new SqlParameter("@AttachFilePath", SqlDbType.VarChar){ Value =  model.AttachFilePath },  
             };
            try
            {
                DBAccess db = new DBAccess();
                return db.InsertUpdateDeleteData("pr_t_assess_guide_CreateGuideAttachZippedFilePath_temp", false, sqlParams);
            }
            catch (ExclusionException)
            {
                return false;
            }
        }
        public DataTable DeleteAttachZippedFilePath(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID },
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@AttachFileUserCD", SqlDbType.VarChar){ Value = model.UserCD  },
             };
            try
            {
                DBAccess db = new DBAccess();
                return db.SelectDatatable("pr_t_assess_guide_AttachFileDelete_temp" , sqlParams);
            }
            catch (ExclusionException)
            {
                return null;
            }
        }
        public DataTable DownAttachZippedFilePath(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@IntroReqID", SqlDbType.VarChar){ Value = model.IntroReqID },
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@AttachFileUserCD", SqlDbType.VarChar){ Value = model.UserCD  },
             };
            try
            {    
                DBAccess db = new DBAccess();
                var dt = db.SelectDatatable("pr_t_assess_guide_AttachFileDown_temp", sqlParams);
           
       
                AESCryption crypt = new AESCryption();
                string decryptionKey = StaticCache.GetDataCryptionKey();
                foreach (DataRow row in dt.Rows)
                {
                    row["AttachFileUnzipPW"] = crypt.DecryptFromBase64(row.Field<string>("AttachFileUnzipPW"), decryptionKey);
                }
                return dt;
            }
            catch (ExclusionException)
            {
                return null;
            }
        }
        public bool AttachFileDownLog(t_assess_guideModel model)
        {
            var sqlParams = new SqlParameter[]
           {
                new SqlParameter("@AttachSEQ", SqlDbType.VarChar){ Value = model.AttachSEQ },
                new SqlParameter("@AttachFileDownUserCD", SqlDbType.VarChar){ Value = model.UserCD },
           };
            try
            {
                DBAccess db = new DBAccess();

                return db.InsertUpdateDeleteData("pr_t_assess_guide_AttachFileDownLog_temp",false, sqlParams) ;
            }
            catch (ExclusionException)
            {
                return false;
            }
        }


        public bool CheckExistZippedName(string ZippedName)
        {
            var sqlParams = new SqlParameter[]
             {
                new SqlParameter("@ZippedName", SqlDbType.VarChar){ Value = ZippedName }, 
             };
            try
            {
                DBAccess db = new DBAccess();
                return db.SelectDatatable("pr_t_assess_guide_CheckExistZippedName_temp", sqlParams).Rows.Count > 0;
            }
            catch (ExclusionException)
            {
                return false;
            }
        }
        public bool CheckUserPermission(string RECD, string ReStaffCD)
        {
            var sqlParams = new SqlParameter[]
         {
                new SqlParameter("@RECD", SqlDbType.VarChar){ Value =RECD },
                new SqlParameter("@REStaffCD", SqlDbType.VarChar){ Value = ReStaffCD },
         };
            try
            {
                DBAccess db = new DBAccess();
                var dt= db.SelectDatatable("pr_t_assess_guide_CheckPermission_temp", sqlParams);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Permission"].ToString() == "1")
                    {
                        return true;
                    }
                    
                }
            }
            catch (ExclusionException)
            {
             
            }
            return false;
        }



    }

}
