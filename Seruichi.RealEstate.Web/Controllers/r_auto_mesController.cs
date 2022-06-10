using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_auto_mes;
using Models.RealEstate.r_auto_mes;
using System.Data;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_auto_mesController : Controller
    {
        // GET: r_auto_mes
        public ActionResult Index()
        {
            
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            if(!SessionAuthenticationHelper.ValidateUser(user))
            {
                return RedirectToAction("Index", "r_login");
            }
            r_auto_mesBL bl = new r_auto_mesBL();
            List<r_auto_mesModel> MesModel = new List<r_auto_mesModel>();
            DataTable dt = bl.Get_M_REMessage_By_RealECD(user.RealECD);
            MesModel = (from DataRow dr in dt.Rows
                        select new r_auto_mesModel()
                        {
                            RealECD = dr["RealECD"].ToString(),
                            SEQ = dr["SEQ"].ToString(),
                            MessageSEQ = dr["MessageSEQ"].ToString(),
                            MessageTitle = dr["MessageTitle"].ToString(),                            
                            MessageTEXT = dr["MessageTEXT"].ToString(),
                            Applying = dr["Applying"].ToString()
                        }).ToList();

            ViewBag.AutoMsg = MesModel;
            ViewBag.IsSetting = user.PermissionSetting;

            return View();
        }
    }
}