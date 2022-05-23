using Models.Tenfold.t_admin;
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
            t_adminBL bl = new t_adminBL();
            List<t_adminModel> lst = new List<t_adminModel>();
            DataTable dt = new DataTable();
            if (GetOperator() != "admin")
                dt = bl.Get_M_TenfoldStaff_By_LoginID("not_admin");
            else
                dt = bl.Get_M_TenfoldStaff_By_LoginID("admin");
            lst = (from DataRow dr in dt.Rows
                   select new t_adminModel()
                   {
                       TenStaffCD = dr["TenStaffCD"].ToString(),
                       TenStaffPW = dr["TenStaffPW"].ToString(),
                       TenStaffName = dr["TenStaffName"].ToString(),
                       InvalidFLG = dr["InvalidFLG"].ToString() == "1" ? "checked" : "unchecked",
                   }).ToList();
            ViewBag.M_TenfoldStaff_list = lst;
            if (lst[0].TenStaffCD == "admin")
                ViewBag.AdminPW = lst[0].TenStaffPW;
            else ViewBag.AdminPW = "";
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
            bl.Save_M_TenfoldStaff(model);
            return OKResult();
        }
    }
}