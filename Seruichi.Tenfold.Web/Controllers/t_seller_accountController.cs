using Models.Tenfold.t_seller_account;
using Seruichi.Common;
using System;
using Seruichi.BL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.BL.Tenfold.t_seller_account;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_accountController : BaseController
    {
        // GET: t_seller_account
        public ActionResult Index()
        {
            return View();
        }
    }
}