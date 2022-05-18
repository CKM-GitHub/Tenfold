using Models.Tenfold.t_admin;
using Seruichi.BL.Tenfold.t_admin;
using Seruichi.Common;
using System;
using System.Collections.Generic;
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
            //if (!validator.CheckNullOrEmpty("TenStaffCD", model.TenStaffCD, out errorcd))
            //{
            //    return ErrorMessageResult(errorcd);
            //}

            t_adminBL bl = new t_adminBL();
            if (!bl.CheckTenStaffCD(model, out errorcd))
            {
                return ErrorMessageResult(errorcd);
            }
            return OKResult();
        }
    }
}