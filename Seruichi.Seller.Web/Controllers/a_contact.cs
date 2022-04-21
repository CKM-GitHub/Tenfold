using Models;
using Seruichi.BL;
using Seruichi.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.Seller.Web.Controllers
{
    public class a_contactController : BaseController
    {
        // GET: a_contact
        [HttpGet]
        public ActionResult Index()
        {
            if (SessionAuthenticationHelper.GetUserFromSession() == null)
            {
                Session.Clear();
                SessionAuthenticationHelper.CreateAnonymousUser();
            }

            CommonBL bl = new CommonBL();
            ViewBag.PrefDropDownListItems = bl.GetDropDownListItemsOfPrefecture();
            return View();
        }
    }
}