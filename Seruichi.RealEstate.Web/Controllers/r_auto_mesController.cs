using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_login;
using Seruichi.BL.RealEstate.r_auto_mes;
using Models.RealEstate.r_auto_mes;
using System.Data;
using System.Threading.Tasks;
using Seruichi.BL;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_auto_mesController : BaseController
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

        [HttpPost]
        public ActionResult DeleteData(r_auto_mesModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            model.RealECD = user.RealECD;
            model.LoginID = base.GetOperator("UserID");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();
            r_auto_mesBL bl = new r_auto_mesBL();
            bl.Delete_REMessage_Data(model);
            return OKResult();
        }

        [HttpPost]
        public ActionResult InsertUpdateData(r_auto_mesModel model)
        {
            r_loginModel user = SessionAuthenticationHelper.GetUserFromSession();
            r_auto_mesBL bl = new r_auto_mesBL();
            model.RealECD = user.RealECD;
            model.LoginID = base.GetOperator("UserID");
            model.LoginName = base.GetOperator("UserName");
            model.IPAddress = base.GetClientIP();

            if (model.ValidFlg == 1)
            {
                model.ValidFlg = 1;
                bl.Update_REMessageValidFlg_Data(model);
            }

            if (model.Mode == "2")
            {
                bl.UpdateData_REMessage_Data(model);
            }
            else
            {
                bl.Insert_REMessage_Data(model);
            }
            
            return OKResult();
        }

    }
}