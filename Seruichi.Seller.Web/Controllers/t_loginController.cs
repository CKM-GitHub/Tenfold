using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Seruichi.Common;
using Seruichi.BL;
using System.Web.Mvc;
using Models.Login;

namespace Seruichi.Seller.Web.Controllers
{
    public class t_loginController : BaseController
    {
        // GET: t_login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult checkIDpsw(t_loginModel model)
        {
            if (model == null)
            {
                return BadRequestResult();
            }

            Validator validator = new Validator();
            string errorcd = "";
            string outPrefCD = "";

            if (!validator.CheckIsHalfWidth(model.TenStaffCD, 10, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            if (!validator.CheckIsHalfWidth(model.TenStaffPW, 10, RegexFormat.Number, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }

            t_loginBL bl = new t_loginBL();
            if (!bl.CheckPrefecturesByIdpsw(model.TenStaffCD, model.TenStaffPW, out errorcd, out outPrefCD))
            {
                return ErrorMessageResult(errorcd);
            }
            else
            {
                return OKResult();
            }


            }
    }
}