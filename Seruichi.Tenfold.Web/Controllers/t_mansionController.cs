using Seruichi.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_mansionController : BaseController
    {
        // GET: t_mansion
        public ActionResult Index()
        {
            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            return View();
        }
    }
}