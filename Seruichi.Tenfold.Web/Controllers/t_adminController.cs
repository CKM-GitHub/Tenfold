using Models.Tenfold.t_admin;
using Newtonsoft.Json;
using Seruichi.BL.Tenfold.t_admin;
using Seruichi.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_adminController : BaseController
    {
        // GET: t_admin
        public ActionResult Index()
        {
            string flg = GetSuperAdminFLG();
            if (flg != "1")
            {
                return RedirectToAction("Login", "t_login");
            }
            t_adminBL bl = new t_adminBL();
            List<t_adminModel> lst = new List<t_adminModel>();
            DataTable dt = bl.Get_M_TenfoldStaff_By_LoginID("not_admin");            
            DataTable dt_admin = bl.Get_M_TenfoldStaff_By_LoginID("admin");
            lst = (from DataRow dr in dt.Rows
                   select new t_adminModel()
                   {
                       TenStaffCD = dr["TenStaffCD"].ToString(),
                       TenStaffPW = dr["TenStaffPW"].ToString(),
                       TenStaffName = dr["TenStaffName"].ToString(),
                       InvalidFLG = dr["InvalidFLG"].ToString() == "1" ? "checked" : "unchecked",
                   }).ToList();
            ViewBag.M_TenfoldStaff_list = lst;            
            ViewBag.AdminPW = dt_admin.Rows[0]["TenStaffPW"];            
            return View();
        }
        [HttpPost]
        public ActionResult CheckTenStaffCD(t_adminModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }
            Validator validator = new Validator();
            string errorcd = "";
            
            t_adminBL bl = new t_adminBL();
            if (!bl.CheckTenStaffCD(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
        [HttpPost]
        public ActionResult Save_M_TenfoldStaff(t_adminModel model)
        {
            t_adminBL bl = new t_adminBL();
            var validationResult = bl.ValidateAll(model);
            if (validationResult.Count > 0)
            {
                return ErrorResult(validationResult);
            }
            model.LoginName = GetLoginStaffName();
            model.Operator = GetOperator();
            model.IPAddress = GetClientIP();
            
            DataTable dt_admin = bl.Get_M_TenfoldStaff_By_LoginID("admin");

            List<Update_t_adminModel> ls_update = Compare_list(model);
            if (ls_update.Count > 0 && !string.IsNullOrEmpty(model.TenStaffCD))
            {
                
                model.Processing = "5";
                model.Remark = "INS=" + model.TenStaffCD + "," + "UPD=" + string.Join(",", ls_update.Select(x => x.TenStaffCD).ToArray());
            }
            else if (ls_update.Count > 0)
            {
                model.Processing = "2";
                model.Remark = "UPD=" + string.Join(",", ls_update.Select(x => x.TenStaffCD).ToArray());
            }
            else if (!string.IsNullOrEmpty(model.TenStaffCD))
            {
                model.Processing = "1";
                model.Remark = "INS=" + model.TenStaffCD;
            }
            else if(dt_admin.Rows[0]["TenStaffPW"].ToString() != model.AdminPassword)
            {
                model.Processing = "2";
                model.Remark = "";
            }
            if (!bl.Save_M_TenfoldStaff(model, out string errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }

        public List<Update_t_adminModel> Compare_list(t_adminModel model)
        {
            t_adminBL bl = new t_adminBL();
            List<Update_t_adminModel> lst_db= new List<Update_t_adminModel>();
            List<Update_t_adminModel> lst_form = model.lst_AdminModel;
            DataTable dt = bl.Get_M_TenfoldStaff_By_LoginID("not_admin");
            lst_db = (from DataRow dr in dt.Rows
                      select new Update_t_adminModel()
                      {
                          TenStaffCD = dr["TenStaffCD"].ToString(),
                          TenStaffPW = dr["TenStaffPW"].ToString(),
                          TenStaffName = dr["TenStaffName"].ToString(),
                          InvalidFLG = dr["InvalidFLG"].ToString(),
                          DeleteFLG = string.IsNullOrEmpty(dr["DeleteOperator"].ToString()) ? "0": "1"
                      }).ToList();

            string JSONStringDB = string.Empty;
            string JSONStringUpdate = string.Empty;
            List<Update_t_adminModel> lst_update = new List<Update_t_adminModel>();
            for (int i = 0; i < lst_db.Count; i++)
            {
                JSONStringDB = JsonConvert.SerializeObject(lst_db[i]);
                JSONStringUpdate = JsonConvert.SerializeObject(lst_form[i]);               
                if (JSONStringDB != JSONStringUpdate)
                {
                    lst_update.Add(lst_form[i]);
                }
            }
            return lst_update;
        }

        [HttpPost]
        public ActionResult Check_Insert_OR_Update_t_admin(t_adminModel model)
        {
            List<Update_t_adminModel> List_to_Update = Compare_list(model);

            t_adminBL bl = new t_adminBL();
            DataTable dt_admin = bl.Get_M_TenfoldStaff_By_LoginID("admin");

            if (List_to_Update.Count > 0 || !string.IsNullOrEmpty(model.TenStaffCD) || (dt_admin.Rows[0]["TenStaffPW"].ToString() != model.AdminPassword))
            {
                return OKResult();
            }
            return ErrorResult();
        }
        
    }
}