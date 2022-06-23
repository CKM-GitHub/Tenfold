using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models.RealEstate.r_statistic;
using Seruichi.BL;
using Seruichi.BL.RealEstate.r_statistic;

namespace Seruichi.RealEstate.Web.Controllers
{
    public class r_statisticController : BaseController
    {
        // GET: r_statistic
        public ActionResult Index()
        {
            return View();
        }
    }
}