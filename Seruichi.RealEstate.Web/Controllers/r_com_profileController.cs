using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_com_profile;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_com_profile;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_com_profileController : Controller
    {
        // GET: r_com_profile
        public ActionResult Index()
        {
            return View();
        }
    }
}