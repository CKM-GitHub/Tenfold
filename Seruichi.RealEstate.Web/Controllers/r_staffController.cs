using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.RealEstate.r_staff;
using Models.RealEstate.r_login;
using Models.RealEstate.r_staff;
using System.Data;
using Seruichi.BL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_staffController : BaseController
    {
        // GET: r_staff
        public ActionResult Index()
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if (!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            r_staffBL bl = new r_staffBL();
            List<r_staffModel> StaffList = new List<r_staffModel>();
            DataTable dt = bl.Get_M_REStaff_By_RealECD_IsAdmin(user.RealECD,user.REStaffCD);
            StaffList = (from DataRow dr in dt.Rows
                                select new r_staffModel()
                                {
                                    RealECD   = dr["RealECD"].ToString(),
                                    REFaceImage = dr["REFaceImage"].ToString() == "" ? dr["REFaceImage"].ToString() : Convert.ToBase64String((byte[])dr["REFaceImage"]),
                                    REStaffCD = dr["REStaffCD"].ToString(),
                                    REStaffName = dr["REStaffName"].ToString(),
                                    REIntroduction = dr["REIntroduction"].ToString(),
                                    REPassword = dr["REPassword"].ToString(),
                                    PermissionChat = dr["PermissionChat"].ToString(),
                                    PermissionSetting = dr["PermissionSetting"].ToString(),
                                    PermissionPlan = dr["PermissionPlan"].ToString(),
                                    PermissionInvoice = dr["PermissionInvoice"].ToString()
                                }).ToList();

            ViewBag.StaffInfo = StaffList;
            ViewBag.IsAdmin = user.REStaffCD;
            return View();
        }
        [HttpPost]
        public ActionResult Get_select_M_REStaff(r_staffModel model)
        {
            string errorcd = "";
            model.RealECD = base.GetOperator("RealECD");
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            if(!bl.Get_select_M_REStaff(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
                
            }
            return OKResult();
        }

        [HttpPost]
        public ActionResult Check_Update_M_REStaff(r_staffModel model)
        {
            List<Update_r_staffModel> List_to_Update= Get_Changes_Data(model);
            if (List_to_Update.Count > 0)
            {
                return OKResult();
            }
            return ErrorResult();
        }

        [HttpPost]
        public ActionResult Save_M_REStaff(r_staffModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
           
            string Dirtemp = @"C:\Temp";
            string DirSelichi = @"C:\Temp\Selichi";
            string Dirupload = @"C:\Temp\Selichi\UploadImage";
            if (!Directory.Exists(Dirtemp))
            {
                Directory.CreateDirectory(Dirtemp);
            }
            if (!Directory.Exists(DirSelichi))
            {
                Directory.CreateDirectory(DirSelichi);
            }
            if (!Directory.Exists(Dirupload))
            {
                Directory.CreateDirectory(Dirupload);
            }
            string UpdateRemark = string.Empty;
            List<Update_r_staffModel> List_to_Update = Get_Changes_Data(model);
            model.lst_StaffModel = List_to_Update;

            if (model.lst_StaffModel.Count > 0)
            {
               
                for (int i = 0; i < model.lst_StaffModel.Count; i++)
                {
                    if (!String.IsNullOrWhiteSpace(model.lst_StaffModel[i].REFaceImage))
                    {
                        //string Updatebase64result = model.lst_StaffModel[i].REFaceImage.Split(',')[1];
                        string Updatebase64result = model.lst_StaffModel[i].REFaceImage;
                        string UpdatefileName = "r_staff_" + model.lst_StaffModel[i].RealECD + "_" + model.lst_StaffModel[i].REStaffCD + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
                        model.lst_StaffModel[i].REFaceImage = byteArrayToImage(Updatebase64result, UpdatefileName);//LoadBase64(str);
                    }
                    else
                    {
                        model.lst_StaffModel[i].REFaceImage = null;
                    }

                    UpdateRemark += model.lst_StaffModel[i].REStaffCD + ",";
                    
                }
            }

            if (user.REStaffCD == "admin")
            {
                if (!String.IsNullOrWhiteSpace(model.REFaceImage))
                {
                    string Newbase64result = model.REFaceImage.Split(',')[1];
                
                    string NewfileName = "r_staff_" + model.RealECD + "_" + model.REStaffCD + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
                    model.REFaceImage = byteArrayToImage(Newbase64result, NewfileName);
                }
                else
                {
                    model.REFaceImage = null;
                }
            }
            model = Getlogdata(model, UpdateRemark);
            if (!bl.Save_M_REStaff(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
        private string byteArrayToImage(string base64,string FileName)
        {
            byte[] byteArrayIn = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Bitmap returnImage = new Bitmap(Image.FromStream(ms, true, true), 100, 100);
            string FilePath = @"C:\Temp\Selichi\UploadImage\" + FileName;
            returnImage.Save(FilePath);
            return FilePath;
        }

        private r_staffModel Getlogdata(r_staffModel model,string Remarks)
        {
            CommonBL bl = new CommonBL();
            //model.LoginKBN = 2;
            model.LoginID = base.GetOperator("UserID");
            model.RealECD = base.GetOperator("RealECD");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            //model.PageID = "r_staff";
            //model.ProcessKBN = "INSERT/UPDATE";
            if (!string.IsNullOrWhiteSpace(model.REStaffCD) && !string.IsNullOrWhiteSpace(Remarks))
            {
                model.Remarks = "INS=" + model.REStaffCD + "," + "UPD=" + Remarks.TrimEnd(',');
                model.ProcessKBN = "5";
            }
            else if(!string.IsNullOrWhiteSpace(model.REStaffCD) && string.IsNullOrWhiteSpace(Remarks))
            {
                model.Remarks = "INS=" + model.REStaffCD;
                model.ProcessKBN = "1";
            }
            else if(!string.IsNullOrWhiteSpace(Remarks))
            {
                model.Remarks = "UPD=" + Remarks.TrimEnd(',');
                model.ProcessKBN = "2";
            }
            return model;
        }


        private List<Update_r_staffModel> Get_Changes_Data(r_staffModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_staffBL bl = new r_staffBL();
            // List<r_staffModel> StaffList = new List<r_staffModel>();
            DataTable dt = bl.Get_M_REStaff_By_RealECD_IsAdmin(user.RealECD, user.REStaffCD);
            List<Update_r_staffModel> StaffList = (from DataRow dr in dt.Rows
                                                   select new Update_r_staffModel()
                                                   {
                                                       RealECD = dr["RealECD"].ToString(),
                                                       REFaceImage = dr["REFaceImage"].ToString() == "" ? dr["REFaceImage"].ToString() : Convert.ToBase64String((byte[])dr["REFaceImage"]),
                                                       REStaffCD = dr["REStaffCD"].ToString(),
                                                       REStaffName = dr["REStaffName"].ToString(),
                                                       REIntroduction = dr["REIntroduction"].ToString() == ""? null : dr["REIntroduction"].ToString(),
                                                       REPassword = dr["REPassword"].ToString(),
                                                       PermissionChat = dr["PermissionChat"].ToString(),
                                                       PermissionSetting = dr["PermissionSetting"].ToString(),
                                                       PermissionPlan = dr["PermissionPlan"].ToString(),
                                                       PermissionInvoice = dr["PermissionInvoice"].ToString()
                                                   }).ToList();

            string JSONStringDB = string.Empty;
            string JSONStringUpdate = string.Empty;


            List<Update_r_staffModel> listtoupdate = new List<Update_r_staffModel>();
            for (int i = 0; i < StaffList.Count; i++)
            {
                if (!String.IsNullOrWhiteSpace(model.lst_StaffModel[i].REFaceImage))
                {
                    string Updatebase64result = model.lst_StaffModel[i].REFaceImage.Split(',')[1];
                    model.lst_StaffModel[i].REFaceImage = Updatebase64result;
                }
                else
                {
                    model.lst_StaffModel[i].REFaceImage = null;
                }
                JSONStringDB = JsonConvert.SerializeObject(StaffList[i]);
                JSONStringUpdate = JsonConvert.SerializeObject(model.lst_StaffModel[i]);
                bool issame = JSONStringDB == JSONStringUpdate;
                issame = false;
                if (JSONStringDB != JSONStringUpdate)
                {
                    listtoupdate.Add(model.lst_StaffModel[i]);
                }
            }

            return listtoupdate;
        }
    }
}