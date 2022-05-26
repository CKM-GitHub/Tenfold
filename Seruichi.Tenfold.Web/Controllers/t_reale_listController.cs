using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Seruichi.BL.Tenfold.t_reale_list;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_reale_listController : BaseController
    {
        // GET: t_reale_list
        public ActionResult Index()
        {
            t_reale_listBL bl = new t_reale_listBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListForPref();
            return View();
        }
    }
}