using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seruichi.Common;
using Models.Tenfold.t_seller_new;
using Seruichi.BL;

namespace Seruichi.Tenfold.Web.Controllers
{
    public class t_seller_newController : BaseController
    {
        // GET: t_seller_new
        public ActionResult Index()
        {
            CommonBL cmmbl = new CommonBL();
            ViewBag.PrefDropDownListItems = cmmbl.GetDropDownListItemsOfAllPrefecture();
            return View();
        }
    }
}