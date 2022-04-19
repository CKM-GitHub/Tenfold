using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seruichi.RealState.Web.Controllers
{
    [AllowAnonymous]
    public class r_loginController : Controller
    {
        // GET: r_login
        public ActionResult Index()
        {
            return View();
        }
    }
}