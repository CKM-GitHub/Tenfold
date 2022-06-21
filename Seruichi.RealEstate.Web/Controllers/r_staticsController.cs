using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_statics;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_statics;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_staticsController : BaseController
    {
        // GET: r_statics
        public ActionResult Index()
        {
            return View();
        }
    }
}